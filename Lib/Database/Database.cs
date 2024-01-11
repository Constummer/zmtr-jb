﻿using CounterStrikeSharp.API.Modules.Entities;
using JailbreakExtras.Lib.Database.Models;
using Microsoft.Extensions.Logging;
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
        _ = Task.Run(async () =>
        {
            try
            {
                using (var con = Connection())
                {
                    var cmd = (MySqlCommand)null;
                    if (con == null)
                    {
                        return;
                    }

                    cmd = new MySqlCommand("CREATE DATABASE IF NOT EXISTS cs2_extras", con);
                    await cmd.ExecuteNonQueryAsync();

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
                       `Xp` mediumint(9) DEFAULT NULL,
                       `TagDisable` bit DEFAULT 0,
                       `GivenRewards` TEXT DEFAULT NULL
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
                          `WTime` mediumint(9) DEFAULT NULL,
                          `WeeklyWTime` mediumint(9) DEFAULT NULL
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);
                    await cmd.ExecuteNonQueryAsync();

                    cmd = new MySqlCommand(
                    @"CREATE TABLE IF NOT EXISTS `PlayerCTBan` (
                         `SteamId` bigint(20) DEFAULT NULL,
                         `BannedBySteamId` bigint(20) DEFAULT NULL,
                         `Time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                       ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);
                    await cmd.ExecuteNonQueryAsync();

                    cmd = new MySqlCommand(
                    @"CREATE TABLE IF NOT EXISTS `PlayerBan` (
                          `SteamId` bigint(20) DEFAULT NULL,
                          `BannedBySteamId` bigint(20) DEFAULT NULL,
                          `Time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);
                    await cmd.ExecuteNonQueryAsync();

                    GetAllPlayerNameData(con);
                    GetAllTimeTrackingData(con);
                    GetAllCTBanData(con);
                    GetAllBanData(con);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "hata");
            }
        });
    }

    private static void GetAllPlayerNameData(MySqlConnection con)
    {
        if (con == null)
        {
            return;
        }
        MySqlCommand? cmd = new MySqlCommand(@$"SELECT `SteamId`, `Name` FROM `PlayerName`", con);
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                var steamId = reader.IsDBNull(0) ? 0 : reader.GetInt64(0);
                var pname = reader.IsDBNull(1) ? "" : reader.GetString(1);
                if (string.IsNullOrWhiteSpace(pname))
                {
                    pname = "-";
                }
                if (PlayerNamesDatas.ContainsKey((ulong)steamId) == false)
                {
                    PlayerNamesDatas.Add((ulong)steamId, pname);
                }
                else
                {
                    PlayerNamesDatas[(ulong)steamId] = pname;
                }
            }
        }

        return;
    }
}