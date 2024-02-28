using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public static Dictionary<ulong, string> CustomPlayerTags { get; set; } = new();

    [ConsoleCommand("customtag")]
    [ConsoleCommand("customtagekle")]
    [CommandHelper(minArgs: 2, "<custom tag verilcek kişi> <tag (boşluklu olabilir * OPSIYONEL)>")]
    public void CustomTag(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/yonetim"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        var targetPlayer = info.ArgString.GetArg(0);
        var tag = info.ArgString.GetArgSkip(1);
        if (string.IsNullOrWhiteSpace(targetPlayer))
        {
            return;
        }
        if (string.IsNullOrWhiteSpace(tag))
        {
            player.PrintToChat($"{Prefix}{CC.W} tag hatali");
            return;
        }

        var targetArgument = GetTargetArgument(targetPlayer);

        var players = GetPlayers()
               .Where(x =>
               (targetArgument == TargetForArgument.UserIdIndex
               ? GetUserIdIndex(targetPlayer) == x.UserId : targetArgument == TargetForArgument.Me
               ? x.SteamID == player.SteamID : false)
               || (x.PlayerName?.ToLower()?.Contains(targetPlayer?.ToLower()) ?? false)
               || x.SteamID.ToString() == targetPlayer)
               .ToList();
        if (players.Count == 0)
        {
            player.PrintToChat($"{Prefix} {CC.W}Eşleşen oyuncu bulunamadı!");
            return;
        }
        if (players.Count != 1)
        {
            player.PrintToChat($"{Prefix} {CC.W}Birden fazla oyuncu bulundu.");
            return;
        }
        var y = players.FirstOrDefault();
        if (ValidateCallerPlayer(y, false) == false) return;

        if (CustomPlayerTags.ContainsKey(y.SteamID))
        {
            CustomPlayerTags[y.SteamID] = tag;
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}{CC.B} {y.PlayerName}{CC.W} adlı oyuncuya {CC.R}[{tag}]{CC.W} tagı verdi");
            return;
        }
        else
        {
            CustomPlayerTags.Add(y.SteamID, tag);
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}{CC.B} {y.PlayerName}{CC.W} adlı oyuncuya {CC.R}[{tag}]{CC.W} tagı verdi");
        }
        AddOrUpdateCustomTagData(y.SteamID, tag);
    }

    [ConsoleCommand("customtagsil")]
    [ConsoleCommand("customtagkaldir")]
    [ConsoleCommand("customtagiptal")]
    [CommandHelper(minArgs: 1, "<custom tagi alinacak kişi>")]
    public void CustomTagSil(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/yonetim"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        var targetPlayer = info.ArgString.GetArg(0);
        if (string.IsNullOrWhiteSpace(targetPlayer))
        {
            return;
        }

        var targetArgument = GetTargetArgument(targetPlayer);

        var players = GetPlayers()
               .Where(x =>
               (targetArgument == TargetForArgument.UserIdIndex
               ? GetUserIdIndex(targetPlayer) == x.UserId : targetArgument == TargetForArgument.Me
               ? x.SteamID == player.SteamID : false)
               || (x.PlayerName?.ToLower()?.Contains(targetPlayer?.ToLower()) ?? false)
               || x.SteamID.ToString() == targetPlayer)
               .ToList();
        if (players.Count == 0)
        {
            player.PrintToChat($"{Prefix} {CC.W}Eşleşen oyuncu bulunamadı!");
            return;
        }
        if (players.Count != 1)
        {
            player.PrintToChat($"{Prefix} {CC.W}Birden fazla oyuncu bulundu.");
            return;
        }
        var y = players.FirstOrDefault();
        if (ValidateCallerPlayer(y, false) == false) return;

        if (CustomPlayerTags.ContainsKey(y.SteamID))
        {
            CustomPlayerTags.Remove(y.SteamID, out var tag);
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}{CC.B} {y.PlayerName}{CC.W} adlı oyuncuya {CC.R}[{tag}]{CC.W} tagını sildi");
            RemoveCustomTagData(y.SteamID);
            return;
        }
    }

    private void RemoveCustomTagData(ulong steamId)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }

                var cmd = new MySqlCommand(@$"Delete From `PlayerCustomTag` WHERE `SteamId` = @SteamId;", con);
                cmd.Parameters.AddWithValue("@SteamId", steamId);
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    private void AddOrUpdateCustomTagData(ulong steamId, string tag)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }

                var cmd = new MySqlCommand(@$"SELECT 1 FROM `PlayerCustomTag` WHERE `SteamId` = @SteamId;", con);
                cmd.Parameters.AddWithValue("@SteamId", steamId);
                bool exist = false;
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        exist = true;
                    }
                }
                if (!exist)
                {
                    cmd = new MySqlCommand(@$"INSERT INTO `PlayerCustomTag`
                                      (SteamId,Tag)
                                      VALUES (@SteamId,@Tag);", con);

                    cmd.Parameters.AddWithValue("@SteamId", steamId);
                    cmd.Parameters.AddWithValue("@Tag", tag);

                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd = new MySqlCommand(@$"UPDATE `PlayerCustomTag`
                                          SET
                                              `Tag` = @Tag
                                          WHERE `SteamId` = @SteamId;", con);

                    cmd.Parameters.AddWithValue("@SteamId", steamId);
                    cmd.Parameters.AddWithValue("@Tag", tag);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    public static bool CustomTagSay(CCSPlayerController? player, CommandInfo info, bool isSayTeam)
    {
        if (CustomPlayerTags.TryGetValue(player.SteamID, out var tag) == false)
        {
            return false;
        }
        var team = GetTeam(player);
        var teamColor = team switch
        {
            CsTeam.CounterTerrorist => CC.BG,
            CsTeam.Terrorist => CC.Y,
            CsTeam.Spectator => CC.LP,
            CsTeam.None => CC.Or,
        };
        var chatColor = team switch
        {
            CsTeam.CounterTerrorist => CC.BG,
            CsTeam.Terrorist => CC.Y,
            CsTeam.Spectator => CC.P,
            CsTeam.None => CC.Or,
        };
        var teamStr = team switch
        {
            CsTeam.CounterTerrorist => $"{CC.B}[GARDİYAN]",
            CsTeam.Terrorist => $"{CC.R}[MAHKÛM]",
            CsTeam.Spectator => $"{CC.P}[SPEC]",
            _ => ""
        };
        var deadStr = player.PawnIsAlive == false ? $"{CC.R}*ÖLÜ*" : "";
        if (isSayTeam)
        {
            GetPlayers(team).ToList()
                .ForEach(x => x.PrintToChat($" {deadStr} {CC.M}[{tag}]{teamStr} {teamColor}{player.PlayerName} {CC.W}: {chatColor}{info.GetArg(1)}"));
        }
        else
        {
            Server.PrintToChatAll($" {deadStr} {CC.M}[{tag}] {teamColor}{player.PlayerName} {CC.W}: {chatColor}{info.GetArg(1)}");
        }
        return true;
    }

    private void GetAllCustomTagData(MySqlConnection con)
    {
        if (con == null)
        {
            return;
        }

        MySqlCommand? cmd = new MySqlCommand(@$"SELECT `SteamId`,`Tag` FROM `PlayerCustomTag`;", con);
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                var steamId = reader.IsDBNull(0) ? 0 : reader.GetInt64(0);
                var tag = reader.IsDBNull(1) ? "" : reader.GetString(1);
                if (steamId == 0)
                {
                    continue;
                }
                if (string.IsNullOrWhiteSpace(tag))
                {
                    continue;
                }

                if (CustomPlayerTags.ContainsKey((ulong)steamId) == false)
                {
                    CustomPlayerTags.Add((ulong)steamId, tag);
                }
                else
                {
                    CustomPlayerTags[(ulong)steamId] = tag;
                }
            }
        }

        return;
    }
}