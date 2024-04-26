using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Dictionary<ulong, string> PlayerNamesDatas = new();

    private static MySqlConnection? Connection()
    {
        try
        {
            MySqlConnection? database = new MySqlConnection(
                $"Server={_Config.Database.Server};User ID={_Config.Database.Username};Password={_Config.Database.Password};Database={_Config.Database.Database};Port={_Config.Database.Port}");

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

                    cmd = new MySqlCommand($"CREATE DATABASE IF NOT EXISTS {Config.Database.Database}", con);
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
                          `KaTime` mediumint(9) DEFAULT NULL,
                          `WeeklyWTime` mediumint(9) DEFAULT NULL,
                          `WeeklyCTTime` mediumint(9) DEFAULT NULL,
                          `WeeklyTTime` mediumint(9) DEFAULT NULL,
                          `WeeklyTotalTime` mediumint(9) DEFAULT NULL,
                          `WeeklyKaTime` mediumint(9) DEFAULT NULL
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
                    @"CREATE TABLE IF NOT EXISTS `ManagerCommands` (
                         `Id` bigint(20) PRIMARY KEY AUTO_INCREMENT,
                         `SteamId` bigint(20) DEFAULT NULL,
                         `Command` TEXT DEFAULT NULL,
                         `Time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                       ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);
                    await cmd.ExecuteNonQueryAsync();

                    cmd = new MySqlCommand(
                    @"CREATE TABLE IF NOT EXISTS `PlayerChat` (
                         `Id` bigint(20) PRIMARY KEY AUTO_INCREMENT,
                         `SteamId` bigint(20) DEFAULT NULL,
                         `Msg` TEXT DEFAULT NULL,
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

                    cmd = new MySqlCommand(
                    @"CREATE TABLE IF NOT EXISTS `PlayerWeeklyWTime` (
                          `SteamId` bigint(20) DEFAULT NULL,
                          `WTime` mediumint(9) DEFAULT NULL,
                          `WeekNo` mediumint(9) DEFAULT NULL
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);
                    await cmd.ExecuteNonQueryAsync();

                    cmd = new MySqlCommand(
                    @"CREATE TABLE IF NOT EXISTS `BayramCredit` (
                          `Id` bigint(20) PRIMARY KEY AUTO_INCREMENT,
                          `SteamId` bigint(20) DEFAULT NULL,
                          `RecieveTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);
                    await cmd.ExecuteNonQueryAsync();

                    cmd = new MySqlCommand(
                    @"CREATE TABLE IF NOT EXISTS `KomWeeklyWCredit` (
                          `SteamId` bigint(20) DEFAULT NULL,
                          `Recieved` bit DEFAULT 0,
                          `WeekNo` mediumint(9) DEFAULT NULL
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);
                    await cmd.ExecuteNonQueryAsync();

                    cmd = new MySqlCommand(
                    @"CREATE TABLE IF NOT EXISTS `PlayerIsyanTeam` (
                          `SteamId` bigint(20) DEFAULT NULL
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);
                    await cmd.ExecuteNonQueryAsync();

                    cmd = new MySqlCommand(
                    @"CREATE TABLE IF NOT EXISTS `PlayerSutTeam` (
                          `SteamId` bigint(20) DEFAULT NULL
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);
                    await cmd.ExecuteNonQueryAsync();

                    cmd = new MySqlCommand(
                    @"CREATE TABLE IF NOT EXISTS `PlayerCTKit` (
                          `SteamId` bigint(20) DEFAULT NULL,
                          `KitId` mediumint(9) DEFAULT 0
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);
                    await cmd.ExecuteNonQueryAsync();

                    cmd = new MySqlCommand(
                    @"CREATE TABLE IF NOT EXISTS `PlayerCustomTag` (
                          `SteamId` bigint(20) DEFAULT NULL,
                          `Tag` TEXT DEFAULT NULL,
                          `TagColor` TEXT DEFAULT NULL,
                          `SayColor` TEXT DEFAULT NULL,
                          `TColor` TEXT DEFAULT NULL,
                          `CTColor` TEXT DEFAULT NULL
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);
                    await cmd.ExecuteNonQueryAsync();

                    cmd = new MySqlCommand(
                    @"CREATE TABLE IF NOT EXISTS `PlayerParticleData` (
                          `SteamId` bigint(20) DEFAULT NULL,
                          `BoughtModelIds` TEXT DEFAULT NULL,
                          `SelectedModelId` TEXT DEFAULT NULL
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);
                    await cmd.ExecuteNonQueryAsync();

                    cmd = new MySqlCommand(
                    @"CREATE TABLE IF NOT EXISTS `DcNotifyData` (
                          `MapName` TEXT DEFAULT NULL,
                          `WardenName` TEXT DEFAULT NULL,
                          `PlayerCount` mediumint(9) DEFAULT 0
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);
                    await cmd.ExecuteNonQueryAsync();

                    cmd = new MySqlCommand(
                    @"CREATE TABLE IF NOT EXISTS `PlayerGambleData` (
                         `Id` bigint(20) PRIMARY KEY AUTO_INCREMENT,
                         `GambleDataId` bigint(20) DEFAULT 0,
                         `SteamId` bigint(20) DEFAULT NULL,
                         `Credit` mediumint(9) DEFAULT 0,
                         `Color` mediumint(9) DEFAULT 0,
                         `Expired` bit DEFAULT 0,
                         `PlayedTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                         `ExpiredTime` DATETIME NULL
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);
                    await cmd.ExecuteNonQueryAsync();

                    cmd = new MySqlCommand(
                    @"CREATE TABLE IF NOT EXISTS `GambleData` (
                         `Id` bigint(20) PRIMARY KEY AUTO_INCREMENT,
                         `StartTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                         `EndTime` DATETIME NULL,
                         `Red` mediumint(9) DEFAULT 0,
                         `Green` mediumint(9) DEFAULT 0,
                         `Black` mediumint(9) DEFAULT 0,
                         `Winner` mediumint(9) DEFAULT 0,
                         `ParticipationCount` mediumint(9) DEFAULT 0,
                         `TourFinalized` bit DEFAULT 0
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);
                    await cmd.ExecuteNonQueryAsync();

                    cmd = new MySqlCommand(
                    @"CREATE TABLE IF NOT EXISTS `PlayerIsTop` (
                          `SteamId` bigint(20) DEFAULT NULL,
                          `KillCount` mediumint(9) DEFAULT 0
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);
                    await cmd.ExecuteNonQueryAsync();

                    cmd = new MySqlCommand(
                    @"CREATE TABLE IF NOT EXISTS `KomKalanInterceptor` (
                            `SelectedModelId` TEXT DEFAULT NULL
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);
                    await cmd.ExecuteNonQueryAsync();

                    cmd = new MySqlCommand(
                    @"CREATE TABLE IF NOT EXISTS `PlayerWTime` (
                          `SteamId` bigint(20) DEFAULT NULL,
                          `WTime` mediumint(9) DEFAULT 0
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);
                    await cmd.ExecuteNonQueryAsync();

                    cmd = new MySqlCommand(
                    @"CREATE TABLE IF NOT EXISTS `PlayerKasa` (
                          `Id` bigint(20) PRIMARY KEY AUTO_INCREMENT,
                          `StartTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                          `SteamId` bigint(20) DEFAULT NULL,
                          `Opened` mediumint(9) DEFAULT 0,
                          `Won` mediumint(9) DEFAULT 0,
                          `GotTheReward` bit DEFAULT 0
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);

                    cmd = new MySqlCommand(
                   @"CREATE TABLE IF NOT EXISTS `PlayerBattlePass` (
					    `Id` bigint(20) PRIMARY KEY AUTO_INCREMENT,
                        `SteamId` bigint(20) DEFAULT NULL,
                        `Level` mediumint(9) DEFAULT NULL,
                        `Config` TEXT DEFAULT NULL,
                        `Completed` bit DEFAULT 0
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);

                    await cmd.ExecuteNonQueryAsync();
                    GetAllPlayerNameData(con);
                    GetAllCTKitData(con);
                    GetAllTimeTrackingData(con);
                    GetAllCTBanData(con);
                    GetAllBanData(con);
                    GetAllPlayerIsyanTeamData(con);
                    GetAllPlayerSutTeamData(con);
                    GetAllKomWeeklyCreditData(con);
                    GetAllCustomTagData(con);
                    GetAllIsTopData(con);
                    GetAllKomKalanInterceptorDatas(con);
                    GetAllWTimes(con);
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
    }
}