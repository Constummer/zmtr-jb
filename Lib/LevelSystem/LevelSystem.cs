using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using JailbreakExtras.Lib.Configs;
using JailbreakExtras.Lib.Database.Models;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private Dictionary<ulong, PlayerLevel> PlayerLevels = new();
    private Dictionary<int, string> LevelPermissions = new();
    private Dictionary<string, int> LevelPermissionsChecker = new();
    private List<ulong> LevelTagDisabledPlayers = new();

    //++ biri sunucuya girince dbye bakilcak kaydi var mi diye, varsa playerLevels modeline eklenecek
    //++ yoksa player !seviyeal, !slotol gibi bisi yazana kadar playerLevels'e eklenmicek, yazinca database e kaydi atilcak
    //++ 60 snde bir playerlevels lere 1 xp eklencek ve en son dbye kaydedilcek
    //++ biri disconnect atinca son datasi kaydedilip playerLevelsten silincek
    //++ markete YEni secenek eklenecek ve tp alabilecek ratio configten okunacak, ornegin 10_000 kredi = 100 tp
    //++ 60 sndebir tp vermede, hediyetp de,seviyever de configdeki datalara gore playerin xpsine uyan admin permission verecek veya alacak
    //tum komutlara seviyelere gore permission check eklenecek

    private bool HasLevelPermissionToActivate(ulong? steamId, string perm)
    {
        if (steamId.HasValue == false || steamId.Value == 0)
            return false;
        return PlayerLevels.TryGetValue(steamId.Value, out var playerLevel)
            && LevelPermissionsChecker.TryGetValue(perm, out int requiredXp) && playerLevel.Xp >= requiredXp;
    }

    private bool HasLevelPermissionToActivate(int xp, string perm)
    {
        return LevelPermissionsChecker.TryGetValue(perm, out int requiredXp) && xp >= requiredXp;
    }

    private LevelGiftConfig? GetPlayerLevelConfig(int xp)
    {
        return Config.Level.LevelGifts.Where(x => x.Xp < xp).OrderByDescending(x => x.Xp).FirstOrDefault();
    }

    private List<string> GetLevelPermissions(int xp)
    {
        List<string> pairsUpTo = new();

        foreach (var kvp in LevelPermissions)
        {
            if (kvp.Key <= xp)
            {
                pairsUpTo.Add(kvp.Value);
            }
        }

        return pairsUpTo;
    }

    private bool LevelSystemPlayer(CCSPlayerController player, CommandInfo info, bool isSayTeam)
    {
        if (LevelTagDisabledPlayers.Contains(player.SteamID))
        {
            return false;
        }

        if (PlayerLevels.TryGetValue(player.SteamID, out var item))
        {
            var config = GetPlayerLevelConfig(item.Xp);
            if (config != null)
            {
                PrintMsgCustom(player, info.GetArg(1), isSayTeam, config);
                return true;
            }
        }
        return false;
    }

    private static void PrintMsgCustom(CCSPlayerController player, string requestMsg, bool isSayTeam, LevelGiftConfig? config)
    {
        var deadStr = player.PawnIsAlive == false ? $"{CC.R}*ÖLÜ*" : "";
        var team = GetTeam(player);
        var teamStr = isSayTeam ? team switch
        {
            CounterStrikeSharp.API.Modules.Utils.CsTeam.CounterTerrorist => $"{CC.B}[GARDİYAN]",
            CounterStrikeSharp.API.Modules.Utils.CsTeam.Terrorist => $"{CC.R}[MAHKÛM]",
            CounterStrikeSharp.API.Modules.Utils.CsTeam.Spectator => $"{CC.P}[SPEC]",
            _ => ""
        } : "";
        string msg;
        if (config?.ClanTag != null)
        {
            msg = $" {deadStr} {teamStr} {CC.Ol}{config.ClanTag} {CC.Gr}{player.PlayerName} {CC.W}: {requestMsg}";
        }
        else
        {
            msg = $" {deadStr} {teamStr} {CC.Or}{player.PlayerName} {CC.W}: {requestMsg}";
        }
        Server.PrintToConsole(msg);
        if (isSayTeam)
        {
            GetPlayers(team).ToList()
                .ForEach(x => x.PrintToChat(msg));
        }
        else
        {
            Server.PrintToChatAll(msg);
        }
    }

    private void RemoveFromLevelSystem(CCSPlayerController? player)
    {
        if (PlayerLevels.TryGetValue(player.SteamID, out var level))
        {
            if (ValidateCallerPlayer(player, false) == false) return;
            PlayerLevels.Remove(player.SteamID);
            DeletePlayerLevelData(player.SteamID);
            LevelTagDisabledPlayers = LevelTagDisabledPlayers.Where(x => x != player.SteamID).ToList();
            player.Clan = null;
            AddTimer(0.2f, () =>
            {
                if (ValidateCallerPlayer(player, false) == false) return;
                Utilities.SetStateChanged(player, "CCSPlayerController", "m_szClan");
                Utilities.SetStateChanged(player, "CBasePlayerController", "m_iszPlayerName");
            });
        }
    }

    private void CheckAllLevelTags()
    {
        foreach (var item in PlayerLevels.ToList())
        {
            var player = GetPlayers().Where(x => x.SteamID == item.Key).FirstOrDefault();
            if (player != null)
            {
                if (player.SteamID == KomutcuAdminId)
                {
                    continue;
                }
                if (LevelTagDisabledPlayers.Contains(item.Key))
                {
                    continue;
                }

                var config = GetPlayerLevelConfig(item.Value.Xp);
                if (config != null)
                {
                    if (config.ClanTag != player.Clan)
                    {
                        if (ValidateCallerPlayer(player, false) == false) return;
                        player.Clan = config.ClanTag;
                        AddTimer(0.2f, () =>
                        {
                            if (ValidateCallerPlayer(player, false) == false) return;
                            Utilities.SetStateChanged(player, "CCSPlayerController", "m_szClan");
                            Utilities.SetStateChanged(player, "CBasePlayerController", "m_iszPlayerName");
                        });
                    }
                }
            }
        }
    }

    private void DeletePlayerLevelData(ulong steamId)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
                var cmd = new MySqlCommand(@$"Delete From `PlayerLevel` WHERE `SteamId` = @SteamId;", con);

                cmd.Parameters.AddWithValue("@SteamId", steamId);
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    private void InsertAndGetPlayerLevelData(ulong steamId, bool mustExist = false, string playerName = null)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
                var cmd = new MySqlCommand(@$"SELECT `SteamId`, `Xp`,`TagDisable`,`GivenRewards` FROM `PlayerLevel` WHERE `SteamId` = @SteamId;", con);
                cmd.Parameters.AddWithValue("@SteamId", steamId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var data = new PlayerLevel(steamId)
                        {
                            Xp = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                            GivenRewards = reader.IsDBNull(3) ? "" : reader.GetString(3)
                        };
                        if (PlayerLevels.TryGetValue(steamId, out var old) == false)
                        {
                            PlayerLevels.Add(steamId, data);
                        }
                        if ((reader.IsDBNull(2) ? false : reader.GetBoolean(2)) == true)
                        {
                            LevelTagDisabledPlayers.Add(steamId);
                        }
                        return;
                    }
                }
                if (mustExist)
                {
                    return;
                }
                PlayerLevels.Add(steamId, new(steamId) { Xp = 0 });
                cmd = new MySqlCommand(@$"INSERT INTO `PlayerLevel`
                                          (SteamId, Xp,TagDisable)
                                          VALUES (@SteamId, 0, 0);", con);

                cmd.Parameters.AddWithValue("@SteamId", steamId);
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    private void GivePlayerRewards(ulong steamid, string playerName)
    {
        try
        {
            if (PlayerLevels.TryGetValue(steamid, out var playerLevelData) == false)
            {
                return;
            }
            var givenRewards = playerLevelData.GivenRewards;
            var xp = playerLevelData.Xp;

            var levels = GetLevelPermissions(xp);

            if (levels == null || levels.Count == 0)
            {
                return;
            }

            levels = levels.Where(x => string.IsNullOrWhiteSpace(x) == false && x.Contains("@css/seviye"))
                           .Select(x => x.Split("@css/seviye")[1])
                           .ToList();

            if (string.IsNullOrWhiteSpace(givenRewards) == false)
            {
                var splitted = givenRewards.Split(',');

                levels = levels.Where(x => splitted.Contains(x) == false).ToList();
            }
            if (levels.Count == 0)
            {
                return;
            }
            foreach (var item in levels)
            {
                var reward = GetLevelReward(item);

                if (reward > 0)
                {
                    var data = GetPlayerMarketModel(steamid);

                    data.Model!.Credit += reward;

                    PlayerMarketModels[steamid] = data.Model;
                    Server.PrintToChatAll($"{Prefix}{CC.Ol}{playerName} {CC.W}seviye {CC.B}{item} {CC.W}ödülü olan {CC.P}{reward}{CC.W} kredisini aldı!");
                }
                if (string.IsNullOrWhiteSpace(givenRewards))
                {
                    givenRewards = item;
                }
                else
                {
                    givenRewards += $",{item}";
                }
            }
            playerLevelData.GivenRewards = givenRewards;
            PlayerLevels[steamid] = playerLevelData;

            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
                var cmd = new MySqlCommand(@$"UPDATE `PlayerLevel`
                                          SET `GivenRewards` = @GivenRewards
                                          WHERE `SteamId` = @SteamId;", con);

                cmd.Parameters.AddWithValue("@SteamId", steamid);
                cmd.Parameters.AddWithValue("@GivenRewards", givenRewards);

                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    private int GetLevelReward(string item)
    {
        return Config?.Level?.LevelGifts?.Where(x => x?.Permission == $"@css/seviye{item}")?.Select(x => x?.CreditReward)?.FirstOrDefault() ?? 0;
    }

    private async Task UpdatePlayerLevelData(ulong steamId, int xp)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
                var cmd = new MySqlCommand(@$"UPDATE `PlayerLevel`
                                          SET `Xp` = @Xp
                                          WHERE `SteamId` = @SteamId;", con);

                cmd.Parameters.AddWithValue("@SteamId", steamId);
                cmd.Parameters.AddWithValue("@Xp", xp);

                await cmd.ExecuteNonQueryAsync();
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    private void UpdatePlayerLevelTagDisableData(ulong steamId, bool disable)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
                var cmd = new MySqlCommand(@$"UPDATE `PlayerLevel`
                                          SET `TagDisable` = @TagDisable
                                          WHERE `SteamId` = @SteamId;", con);

                cmd.Parameters.AddWithValue("@SteamId", steamId);
                cmd.Parameters.AddWithValue("@TagDisable", disable);

                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    private void SetLevelPermissionDictionary()
    {
        LevelPermissions?.Clear();
        LevelPermissionsChecker?.Clear();
        LevelPermissions = new();
        LevelPermissionsChecker = new();

        foreach (var item in Config.Level.LevelGifts)
        {
            LevelPermissions.Add(item.Xp, item.Permission);

            if (item.Permission != null)
                LevelPermissionsChecker.Add(item.Permission, item.Xp);
        }
    }
}