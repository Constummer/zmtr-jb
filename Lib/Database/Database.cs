using CounterStrikeSharp.API.Modules.Entities;
using JailbreakExtras.Lib.Database.Models;
using MySqlConnector;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private MySqlConnection? Connection()
    {
        try
        {
            MySqlConnection? database = new MySqlConnection(
                $"Server={Config.Database.Server};User ID={Config.Database.Username};Password={Config.Database.Password};Database={Config.Database.Database};Port={Config.Database.Port}");

            database.Open();

            return database;
        }
        catch
        {
            //Console.WriteLine(ex.ToString());
            return null;
        }
    }

    private void Database()
    {
        var con = Connection();

        if (con == null)
        {
            return;
        }

        _ = Task.Run(async () =>
        {
            var cmd = (MySqlCommand)null;
            try
            {
                cmd = new MySqlCommand("CREATE DATABASE IF NOT EXISTS cs2_extras", con);
                await cmd.ExecuteNonQueryAsync();
            }
            catch
            {
            }
            cmd = new MySqlCommand(
                @"CREATE TABLE IF NOT EXISTS `PlayerMarketModel` (
                  `SteamId` bigint(20) DEFAULT NULL,
                  `ModelIdCT` varchar(0) DEFAULT NULL,
                  `ModelIdT` varchar(1) DEFAULT NULL,
                  `DefaultIdCT` varchar(0) DEFAULT NULL,
                  `DefaultIdT` varchar(1) DEFAULT NULL,
                  `Credit` mediumint(9) DEFAULT NULL
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);
            await cmd.ExecuteNonQueryAsync();

            cmd = new MySqlCommand(
             @"CREATE TABLE IF NOT EXISTS `PlayerLevel` (
               `SteamId` bigint(20) DEFAULT NULL,
               `Xp` mediumint(9) DEFAULT NULL
               ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);
            await cmd.ExecuteNonQueryAsync();

            cmd = new MySqlCommand(
             @"CREATE TABLE IF NOT EXISTS `PlayerName` (
               `SteamId` bigint(20) DEFAULT NULL,
               `Name` TEXT DEFAULT NULL
               ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);
            await cmd.ExecuteNonQueryAsync();

            cmd = new MySqlCommand(
             @"CREATE TABLE IF NOT EXISTS `PlayerGag` (
                  `SteamId` bigint(20) DEFAULT NULL
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);
            await cmd.ExecuteNonQueryAsync();

            cmd = new MySqlCommand(
            @"CREATE TABLE IF NOT EXISTS `PlayerTime` (
                  `SteamId` bigint(20) DEFAULT NULL,
                  `Total` mediumint(9) DEFAULT NULL,
                  `CTTime` mediumint(9) DEFAULT NULL,
                  `TTime` mediumint(9) DEFAULT NULL,
                  `WTime` mediumint(9) DEFAULT NULL
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);
            await cmd.ExecuteNonQueryAsync();

            cmd = new MySqlCommand(@$"SELECT `SteamId`, `Name` FROM `PlayerName`", con);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var steamId = reader.IsDBNull(0) ? 0 : reader.GetInt64(0);

                    if (PlayerNamesDatas.ContainsKey((ulong)steamId) == false)
                    {
                        var total = reader.IsDBNull(1) ? "" : reader.GetString(1);

                        PlayerNamesDatas.Add((ulong)steamId, total);
                    }

                    return;
                }
            }
        });
    }
}