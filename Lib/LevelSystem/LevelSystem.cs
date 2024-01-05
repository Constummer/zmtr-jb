﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using JailbreakExtras.Lib.Configs;
using JailbreakExtras.Lib.Database.Models;
using Microsoft.Extensions.Logging;
using MySqlConnector;

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

    private bool HasLevelPermissionToActivate(ulong steamId, string perm)
    {
        return PlayerLevels.TryGetValue(steamId, out var playerLevel)
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
                var deadStr = player.PawnIsAlive == false ? "*ÖLÜ*" : "";
                var teamSayStr = isSayTeam ? "[TAKIM]" : "";
                var teamStr = GetTeam(player) switch
                {
                    CounterStrikeSharp.API.Modules.Utils.CsTeam.CounterTerrorist => "[GARDİYAN]",
                    CounterStrikeSharp.API.Modules.Utils.CsTeam.Terrorist => "[MAHKÛM]",
                    CounterStrikeSharp.API.Modules.Utils.CsTeam.Spectator => "[SPEC]",
                    _ => ""
                };
                Server.PrintToChatAll($" {CC.Ol}{deadStr}{teamSayStr}{teamStr}{config.ClanTag} {CC.Gr}{player.PlayerName} {CC.W}: {info.GetArg(1)}");
                return true;
            }
        }
        return false;
    }

    private void RemoveFromLevelSystem(CCSPlayerController? player)
    {
        if (PlayerLevels.TryGetValue(player.SteamID, out var level))
        {
            PlayerLevels.Remove(player.SteamID);
            DeletePlayerLevelData(player.SteamID);
            LevelTagDisabledPlayers = LevelTagDisabledPlayers.Where(x => x != player.SteamID).ToList();
            player.Clan = null;
            AddTimer(0.2f, () =>
            {
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
                        player.Clan = config.ClanTag;
                        AddTimer(0.2f, () =>
                        {
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
        var con = Connection();
        if (con == null)
        {
            return;
        }

        try
        {
            var cmd = new MySqlCommand(@$"Delete From `PlayerLevel` WHERE `SteamId` = @SteamId;", con);

            cmd.Parameters.AddWithValue("@SteamId", steamId);
            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    private void InsertAndGetPlayerLevelData(ulong steamId, bool mustExist = false)
    {
        var con = Connection();
        if (con == null)
        {
            return;
        }

        try
        {
            var cmd = new MySqlCommand(@$"SELECT `SteamId`, `Xp` FROM `PlayerLevel` WHERE `SteamId` = @SteamId;", con);
            cmd.Parameters.AddWithValue("@SteamId", steamId);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    var data = new PlayerLevel(steamId)
                    {
                        Xp = reader.IsDBNull(1) ? 0 : reader.GetInt32(1)
                    };
                    if (PlayerLevels.TryGetValue(steamId, out var old) == false)
                    {
                        PlayerLevels.Add(steamId, data);
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
                                          (SteamId, Xp)
                                          VALUES (@SteamId, 0);", con);

            cmd.Parameters.AddWithValue("@SteamId", steamId);
            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    private async Task UpdatePlayerLevelData(PlayerLevel item)
    {
        var con = Connection();
        if (con == null)
        {
            return;
        }

        try
        {
            var cmd = new MySqlCommand(@$"UPDATE `PlayerLevel`
                                          SET `Xp` = @Xp
                                          WHERE `SteamId` = @SteamId;", con);

            cmd.Parameters.AddWithValue("@SteamId", item.SteamId);
            cmd.Parameters.AddWithValue("@Xp", item.Xp);

            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    private async Task UpdatePlayerLevelData(ulong steamId, int xp)
    {
        var con = Connection();
        if (con == null)
        {
            return;
        }

        try
        {
            var cmd = new MySqlCommand(@$"UPDATE `PlayerLevel`
                                          SET `Xp` = @Xp
                                          WHERE `SteamId` = @SteamId;", con);

            cmd.Parameters.AddWithValue("@SteamId", steamId);
            cmd.Parameters.AddWithValue("@Xp", xp);

            await cmd.ExecuteNonQueryAsync();
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