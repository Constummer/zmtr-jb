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
    #region RR

    /*
         @"CREATE TABLE IF NOT EXISTS `PlayerCTBan` (
                  `SteamId` bigint(20) DEFAULT NULL,
                  `BannedBySteamId` bigint(20) DEFAULT NULL,
                  `Time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
    */
    private static Dictionary<ulong, DateTime> CTBans = new Dictionary<ulong, DateTime>();

    [ConsoleCommand("ctunban")]
    [ConsoleCommand("unctban")]
    [ConsoleCommand("ctbankaldir")]
    [CommandHelper(1, "<playerismi>")]
    public void CTUnBan(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/lider"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        var target = info.ArgCount > 1 ? info.ArgString.GetArg(0) : null;
        if (target == null)
        {
            return;
        }

        var targetArgument = GetTargetArgument(target);
        GetPlayers()
            .Where(x => targetArgument switch
            {
                TargetForArgument.None => x.PlayerName?.ToLowerInvariant()?.Contains(target?.ToLowerInvariant()) ?? false,
                TargetForArgument.UserIdIndex => GetUserIdIndex(target) == x.UserId,
                _ => false
            })
            .ToList()
            .ForEach(x =>
            {
                CTBans.Remove(x.SteamID, out var _);
                RemoveCTBanData(x.SteamID);
                Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W} CT banını kaldırdı.");
            });
    }

    [ConsoleCommand("ctban")]
    [CommandHelper(2, "<playerismi> <dakika/0 süresiz>")]
    public void CTBan(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/lider"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        var target = info.ArgCount > 1 ? info.ArgString.GetArg(0) : null;
        if (target == null)
        {
            return;
        }
        var godOneTwoStr = info.ArgCount > 2 ? info.ArgString.GetArg(1) : "0";
        if (int.TryParse(godOneTwoStr, out var value) == false)
        {
            return;
        }

        var targetArgument = GetTargetArgument(target);
        GetPlayers()
            .Where(x => targetArgument switch
            {
                TargetForArgument.None => x.PlayerName?.ToLowerInvariant()?.Contains(target?.ToLowerInvariant()) ?? false,
                TargetForArgument.UserIdIndex => GetUserIdIndex(target) == x.UserId,
                _ => false
            })
            .ToList()
            .ForEach(x =>
            {
                if (value <= 0)
                {
                    if (CTBans.TryGetValue(x.SteamID, out var dateTime))
                    {
                        CTBans[x.SteamID] = DateTime.UtcNow.AddYears(1);
                    }
                    else
                    {
                        CTBans.Add(x.SteamID, DateTime.UtcNow.AddYears(1));
                    }
                    AddCTBanData(x.SteamID, player.SteamID, DateTime.UtcNow.AddYears(1));
                    Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.B}Sınırsız{CC.W} ct banladı.");
                }
                else
                {
                    if (CTBans.TryGetValue(x.SteamID, out var dateTime))
                    {
                        CTBans[x.SteamID] = DateTime.UtcNow.AddMinutes(value);
                    }
                    else
                    {
                        CTBans.Add(x.SteamID, DateTime.UtcNow.AddMinutes(value));
                    }
                    AddCTBanData(x.SteamID, player.SteamID, DateTime.UtcNow.AddMinutes(value));
                    Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.B}{value}{CC.W} dakika boyunca ct banladı.");
                }
                if (GetTeam(x) == CsTeam.CounterTerrorist)
                {
                    x.ChangeTeam(CsTeam.Terrorist);
                }
            });
    }

    private void GetAllCTBanData(MySqlConnection con)
    {
        if (con == null)
        {
            return;
        }
        MySqlCommand? cmd = new MySqlCommand(@$"SELECT `SteamId`, `Time` FROM `PlayerCTBan`", con);
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                var steamId = reader.IsDBNull(0) ? 0 : reader.GetInt64(0);

                if (CTBans.ContainsKey((ulong)steamId) == false)
                {
                    var time = reader.IsDBNull(1) ? DateTime.UtcNow : reader.GetDateTime(1);

                    CTBans.Add((ulong)steamId, time);
                }
            }
        }

        return;
    }

    private bool CTBanCheck(CCSPlayerController player)
    {
        if (CTBans.TryGetValue(player.SteamID, out var call))
        {
            if (call > DateTime.UtcNow)
            {
                return false;
            }
            else
            {
                CTBans.Remove(player.SteamID);
            }
        }
        return true;
    }

    private void AddCTBanData(ulong steamId, ulong bannerId, DateTime time)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }

                var cmd = new MySqlCommand(@$"SELECT 1 FROM `PlayerCTBan` WHERE `SteamId` = @SteamId;", con);
                cmd.Parameters.AddWithValue("@SteamId", steamId);
                bool exist = false;
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        exist = true;
                    }
                }
                if (exist)
                {
                    cmd = new MySqlCommand(@$"UPDATE `PlayerCTBan`
                                          SET
                                              `BannedBySteamId` = @BannedBySteamId,
                                              `Time` = @Time
                                          WHERE `SteamId` = @SteamId;
                ", con);

                    cmd.Parameters.AddWithValue("@SteamId", steamId);
                    cmd.Parameters.AddWithValue("@BannedBySteamId", bannerId);
                    cmd.Parameters.AddWithValue("@Time", time);

                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd = new MySqlCommand(@$"INSERT INTO `PlayerCTBan`
                                      (SteamId,BannedBySteamId,Time)
                                      VALUES (@SteamId,@BannedBySteamId,@Time);", con);

                    cmd.Parameters.AddWithValue("@SteamId", steamId);
                    cmd.Parameters.AddWithValue("@BannedBySteamId", bannerId);
                    cmd.Parameters.AddWithValue("@Time", time);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    private void RemoveCTBanData(ulong steamId)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }

                var cmd = new MySqlCommand(@$"DELETE FROM `PlayerCTBan` WHERE `SteamId` = @SteamId;", con);
                cmd.Parameters.AddWithValue("@SteamId", steamId);
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    #endregion RR
}