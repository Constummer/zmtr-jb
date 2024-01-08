using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
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

    [ConsoleCommand("ctban")]
    [CommandHelper(2, "<playerismi> <dakika/0 süresiz>")]
    public void CTBan(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        var target = info.ArgCount > 1 ? info.GetArg(1) : null;
        if (target == null)
        {
            return;
        }
        var godOneTwoStr = "0";
        if (int.TryParse(godOneTwoStr, out var value) == false)
        {
            return;
        }

        var targetArgument = GetTargetArgument(target);
        GetPlayers()
            .Where(x => targetArgument switch
            {
                TargetForArgument.None => x.PlayerName?.ToLower()?.Contains(target) ?? false,
                TargetForArgument.UserIdIndex => GetUserIdIndex(target) == x.UserId,
                _ => false
            })
            .ToList()
            .ForEach(gagPlayer =>
            {
                if (value <= 0)
                {
                    if (CTBans.TryGetValue(gagPlayer.SteamID, out var dateTime))
                    {
                        CTBans[gagPlayer.SteamID] = DateTime.UtcNow.AddYears(1);
                    }
                    else
                    {
                        CTBans.Add(gagPlayer.SteamID, DateTime.UtcNow.AddYears(1));
                    }
                    AddCTBanData(gagPlayer.SteamID, player.SteamID, DateTime.UtcNow.AddYears(1));
                    Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{gagPlayer.PlayerName} {CC.B}Sınırsız{CC.W} ct banladı.");
                }
                else
                {
                    if (CTBans.TryGetValue(gagPlayer.SteamID, out var dateTime))
                    {
                        CTBans[gagPlayer.SteamID] = DateTime.UtcNow.AddMinutes(value);
                    }
                    else
                    {
                        CTBans.Add(gagPlayer.SteamID, DateTime.UtcNow.AddMinutes(value));
                    }
                    AddCTBanData(gagPlayer.SteamID, player.SteamID, DateTime.UtcNow.AddMinutes(value));
                    Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{gagPlayer.PlayerName} {CC.B}{value}{CC.W} dakika boyunca ct banladı.");
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
        var con = Connection();
        if (con == null)
        {
            return;
        }

        try
        {
            var cmd = new MySqlCommand(@$"INSERT INTO `PlayerCTBan`
                                      (SteamId,BannedBySteamId,Time)
                                      VALUES (@SteamId,@BannedBySteamId,@Time);", con);

            cmd.Parameters.AddWithValue("@SteamId", steamId);
            cmd.Parameters.AddWithValue("@BannedBySteamId", bannerId);
            cmd.Parameters.AddWithValue("@Time", time);
            cmd.ExecuteNonQuery();

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return;
                }
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    #endregion RR
}