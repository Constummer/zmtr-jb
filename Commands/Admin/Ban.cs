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
        @"CREATE TABLE IF NOT EXISTS `PlayerBan` (
                  `SteamId` bigint(20) DEFAULT NULL,
                  `BannedBySteamId` bigint(20) DEFAULT NULL,
                  `Time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);
    */
    private static Dictionary<ulong, DateTime> Bans = new Dictionary<ulong, DateTime>();

    [ConsoleCommand("unban")]
    [ConsoleCommand("bankaldir")]
    [CommandHelper(1, "<playerismi | steamid | #userid>", CommandUsage.CLIENT_ONLY)]
    public void UnBan(CCSPlayerController? player, CommandInfo info)
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

        GetPlayers()
            .Where(x => x.PlayerName?.ToLower()?.Contains(target) ?? false
            || GetUserIdIndex(target) == x.UserId
            || x.SteamID.ToString() == target)
            .ToList()
            .ForEach(x =>
            {
                Bans.Remove(x.SteamID, out var _);
                RemoveBanData(x.SteamID);
                Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}, {CC.G}{x.PlayerName} {CC.W} Banını kaldırdı.");
            });
    }

    [ConsoleCommand("ban")]
    [CommandHelper(2, "<playerismi | steamid | #userid> <dakika/0 süresiz>", CommandUsage.CLIENT_ONLY)]
    public void Ban(CCSPlayerController? player, CommandInfo info)
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
        var godOneTwoStr = info.ArgCount > 2 ? info.GetArg(2) : "0";
        if (int.TryParse(godOneTwoStr, out var value) == false)
        {
            return;
        }

        var players = GetPlayers()
            .Where(x => x.PlayerName?.ToLower()?.Contains(target) ?? false
            || GetUserIdIndex(target) == x.UserId
            || x.SteamID.ToString() == target
            )
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
        var x = players.FirstOrDefault();

        if (value <= 0)
        {
            if (Bans.TryGetValue(x.SteamID, out var dateTime))
            {
                Bans[x.SteamID] = DateTime.UtcNow.AddYears(1);
            }
            else
            {
                Bans.Add(x.SteamID, DateTime.UtcNow.AddYears(1));
            }
            AddBanData(x.SteamID, player.SteamID, DateTime.UtcNow.AddYears(1));
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}, {CC.G}{x.PlayerName} {CC.B}Sınırsız{CC.W} banladı.");
        }
        else
        {
            if (Bans.TryGetValue(x.SteamID, out var dateTime))
            {
                Bans[x.SteamID] = DateTime.UtcNow.AddMinutes(value);
            }
            else
            {
                Bans.Add(x.SteamID, DateTime.UtcNow.AddMinutes(value));
            }
            AddBanData(x.SteamID, player.SteamID, DateTime.UtcNow.AddMinutes(value));
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}, {CC.G}{x.PlayerName} {CC.B}{value}{CC.W} dakika boyunca banladı.");
        }
        Server.ExecuteCommand($"kickid {x.UserId}");
    }

    private void GetAllBanData(MySqlConnection con)
    {
        if (con == null)
        {
            return;
        }
        MySqlCommand? cmd = new MySqlCommand(@$"SELECT `SteamId`, `Time` FROM `PlayerBan`", con);
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                var steamId = reader.IsDBNull(0) ? 0 : reader.GetInt64(0);

                if (Bans.ContainsKey((ulong)steamId) == false)
                {
                    var time = reader.IsDBNull(1) ? DateTime.UtcNow : reader.GetDateTime(1);

                    Bans.Add((ulong)steamId, time);
                }
            }
        }

        return;
    }

    private bool BanCheck(CCSPlayerController player)
    {
        if (Bans.TryGetValue(player.SteamID, out var call))
        {
            if (call > DateTime.UtcNow)
            {
                return false;
            }
            else
            {
                Bans.Remove(player.SteamID);
            }
        }
        return true;
    }

    private void AddBanData(ulong steamId, ulong bannerId, DateTime time)
    {
        var con = Connection();
        if (con == null)
        {
            return;
        }

        try
        {
            var cmd = new MySqlCommand(@$"SELECT 1 FROM `PlayerBan` WHERE `SteamId` = @SteamId;", con);
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
                cmd = new MySqlCommand(@$"UPDATE `PlayerBan`
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
                cmd = new MySqlCommand(@$"INSERT INTO `PlayerBan`
                                      (SteamId,BannedBySteamId,Time)
                                      VALUES (@SteamId,@BannedBySteamId,@Time);", con);

                cmd.Parameters.AddWithValue("@SteamId", steamId);
                cmd.Parameters.AddWithValue("@BannedBySteamId", bannerId);
                cmd.Parameters.AddWithValue("@Time", time);
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    private void RemoveBanData(ulong steamId)
    {
        var con = Connection();
        if (con == null)
        {
            return;
        }

        try
        {
            var cmd = new MySqlCommand(@$"DELETE FROM `PlayerBan` WHERE `SteamId` = @SteamId;", con);
            cmd.Parameters.AddWithValue("@SteamId", steamId);
            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    #endregion RR
}