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
        catch (Exception e)
        {
            ConsMsg(e.Message);
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
                    if (con == null)
                    {
                        return;
                    }

                    var cmd = new MySqlCommand($"CREATE DATABASE IF NOT EXISTS {Config.Database.Database}", con);
                    await cmd.ExecuteNonQueryAsync();

                    cmd = new MySqlCommand(
                    @"

-- BayramCredit definition

CREATE TABLE IF NOT EXISTS `BayramCredit` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `SteamId` bigint(20) DEFAULT NULL,
  `RecieveTime` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=113 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- DcNotifyData definition

CREATE TABLE IF NOT EXISTS `DcNotifyData` (
  `MapName` text DEFAULT NULL,
  `WardenName` text DEFAULT NULL,
  `PlayerCount` mediumint(9) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- GambleData definition

CREATE TABLE IF NOT EXISTS `GambleData` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `StartTime` datetime NOT NULL DEFAULT current_timestamp(),
  `EndTime` datetime DEFAULT NULL,
  `Red` mediumint(9) DEFAULT 0,
  `Green` mediumint(9) DEFAULT 0,
  `Black` mediumint(9) DEFAULT 0,
  `Winner` mediumint(9) DEFAULT 0,
  `ParticipationCount` mediumint(9) DEFAULT 0,
  `TourFinalized` bit(1) DEFAULT b'0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=24310 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- KomKalanInterceptor definition

CREATE TABLE IF NOT EXISTS `KomKalanInterceptor` (
  `SelectedModelId` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- KomWeeklyWCredit definition

CREATE TABLE IF NOT EXISTS `KomWeeklyWCredit` (
  `SteamId` bigint(20) DEFAULT NULL,
  `Recieved` bit(1) DEFAULT b'0',
  `WeekNo` mediumint(9) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- ManagerCommands definition

CREATE TABLE IF NOT EXISTS `ManagerCommands` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `SteamId` bigint(20) DEFAULT NULL,
  `Command` text DEFAULT NULL,
  `Time` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=258636 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- PlayerBan definition

CREATE TABLE IF NOT EXISTS `PlayerBan` (
  `SteamId` bigint(20) DEFAULT NULL,
  `BannedBySteamId` bigint(20) DEFAULT NULL,
  `Time` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- PlayerBattlePass definition

CREATE TABLE IF NOT EXISTS `PlayerBattlePass` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `SteamId` bigint(20) DEFAULT NULL,
  `Level` mediumint(9) DEFAULT NULL,
  `Config` text DEFAULT NULL,
  `Completed` bit(1) DEFAULT b'0',
  `StartTime` datetime NOT NULL DEFAULT current_timestamp(),
  `EndTime` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1657 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- PlayerBattlePassPremium definition

CREATE TABLE IF NOT EXISTS `PlayerBattlePassPremium` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `SteamId` bigint(20) DEFAULT NULL,
  `Level` mediumint(9) DEFAULT NULL,
  `Config` text DEFAULT NULL,
  `Completed` bit(1) DEFAULT b'0',
  `StartTime` datetime NOT NULL DEFAULT current_timestamp(),
  `EndTime` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1403 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- PlayerCTBan definition

CREATE TABLE IF NOT EXISTS `PlayerCTBan` (
  `SteamId` bigint(20) DEFAULT NULL,
  `BannedBySteamId` bigint(20) DEFAULT NULL,
  `Time` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- PlayerCTKit definition

CREATE TABLE IF NOT EXISTS `PlayerCTKit` (
  `SteamId` bigint(20) DEFAULT NULL,
  `KitId` mediumint(9) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- PlayerChat definition

CREATE TABLE IF NOT EXISTS `PlayerChat` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `SteamId` bigint(20) DEFAULT NULL,
  `Msg` text DEFAULT NULL,
  `Time` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1318770 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- PlayerCustomTag definition

CREATE TABLE IF NOT EXISTS `PlayerCustomTag` (
  `SteamId` bigint(20) DEFAULT NULL,
  `Tag` text DEFAULT NULL,
  `TagColor` text DEFAULT NULL,
  `SayColor` text DEFAULT NULL,
  `TColor` text DEFAULT NULL,
  `CTColor` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- PlayerGag definition

CREATE TABLE IF NOT EXISTS `PlayerGag` (
  `SteamId` bigint(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- PlayerGambleData definition

CREATE TABLE IF NOT EXISTS `PlayerGambleData` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `GambleDataId` bigint(20) DEFAULT 0,
  `SteamId` bigint(20) DEFAULT NULL,
  `Credit` mediumint(9) DEFAULT 0,
  `Color` mediumint(9) DEFAULT 0,
  `Expired` bit(1) DEFAULT b'0',
  `PlayedTime` datetime NOT NULL DEFAULT current_timestamp(),
  `ExpiredTime` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=12927 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- PlayerIsTop definition

CREATE TABLE IF NOT EXISTS `PlayerIsTop` (
  `SteamId` bigint(20) DEFAULT NULL,
  `KillCount` mediumint(9) DEFAULT 0,
  `TotalKillCount` mediumint(9) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- PlayerIsyanTeam definition

CREATE TABLE IF NOT EXISTS `PlayerIsyanTeam` (
  `SteamId` bigint(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- PlayerKasa definition

CREATE TABLE IF NOT EXISTS `PlayerKasa` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `StartTime` datetime NOT NULL DEFAULT current_timestamp(),
  `SteamId` bigint(20) DEFAULT NULL,
  `Opened` mediumint(9) DEFAULT 0,
  `Won` mediumint(9) DEFAULT 0,
  `GotTheReward` bit(1) DEFAULT b'0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=57 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- PlayerLevel definition

CREATE TABLE IF NOT EXISTS `PlayerLevel` (
  `SteamId` bigint(20) DEFAULT NULL,
  `Xp` mediumint(9) DEFAULT NULL,
  `TagDisable` bit(1) DEFAULT b'0',
  `GivenRewards` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- PlayerMarketModel definition

CREATE TABLE IF NOT EXISTS `PlayerMarketModel` (
  `SteamId` bigint(20) DEFAULT NULL,
  `ModelIdCT` varchar(255) DEFAULT NULL,
  `ModelIdT` varchar(255) DEFAULT NULL,
  `DefaultIdCT` varchar(8) DEFAULT NULL,
  `DefaultIdT` varchar(8) DEFAULT NULL,
  `Credit` mediumint(9) DEFAULT NULL,
  UNIQUE KEY `SteamId` (`SteamId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- PlayerModel definition

CREATE TABLE IF NOT EXISTS `PlayerModel` (
  `Id` int(11) DEFAULT NULL,
  `Text` varchar(14) DEFAULT NULL,
  `PathToModel` varchar(86) DEFAULT NULL,
  `TeamNo` tinyint(4) DEFAULT NULL,
  `Cost` mediumint(9) DEFAULT NULL,
  `Enable` tinyint(4) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- PlayerName definition

CREATE TABLE IF NOT EXISTS `PlayerName` (
  `SteamId` bigint(20) DEFAULT NULL,
  `Name` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- PlayerParachuteData definition

CREATE TABLE IF NOT EXISTS `PlayerParachuteData` (
  `SteamId` bigint(20) DEFAULT NULL,
  `BoughtModelIds` text DEFAULT NULL,
  `SelectedModelId` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- PlayerParticleData definition

CREATE TABLE IF NOT EXISTS `PlayerParticleData` (
  `SteamId` bigint(20) DEFAULT NULL,
  `BoughtModelIds` text DEFAULT NULL,
  `SelectedModelId` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- PlayerSutTeam definition

CREATE TABLE IF NOT EXISTS `PlayerSutTeam` (
  `SteamId` bigint(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- PlayerTime definition

CREATE TABLE IF NOT EXISTS `PlayerTime` (
  `SteamId` bigint(20) DEFAULT NULL,
  `Total` mediumint(9) DEFAULT NULL,
  `CTTime` mediumint(9) DEFAULT NULL,
  `TTime` mediumint(9) DEFAULT NULL,
  `WTime` mediumint(9) DEFAULT NULL,
  `WeeklyWTime` mediumint(9) DEFAULT NULL,
  `WeeklyCTTime` mediumint(9) DEFAULT NULL,
  `WeeklyTTime` mediumint(9) DEFAULT NULL,
  `WeeklyTotalTime` mediumint(9) DEFAULT NULL,
  `WeeklyKaTime` mediumint(9) DEFAULT NULL,
  `KaTime` mediumint(9) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- PlayerTimeReward definition

CREATE TABLE IF NOT EXISTS `PlayerTimeReward` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `SteamId` bigint(20) DEFAULT NULL,
  `Level` mediumint(9) DEFAULT NULL,
  `Config` text DEFAULT NULL,
  `Completed` bit(1) DEFAULT b'0',
  `StartTime` datetime NOT NULL DEFAULT current_timestamp(),
  `EndTime` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1776 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- PlayerWTime definition

CREATE TABLE IF NOT EXISTS `PlayerWTime` (
  `SteamId` bigint(20) DEFAULT NULL,
  `WTime` mediumint(9) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- PlayerWeeklyCTTime definition

CREATE TABLE IF NOT EXISTS `PlayerWeeklyCTTime` (
  `SteamId` bigint(20) DEFAULT NULL,
  `WTime` mediumint(9) DEFAULT NULL,
  `WeekNo` mediumint(9) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- PlayerWeeklyTTime definition

CREATE TABLE IF NOT EXISTS `PlayerWeeklyTTime` (
  `SteamId` bigint(20) DEFAULT NULL,
  `WTime` mediumint(9) DEFAULT NULL,
  `WeekNo` mediumint(9) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- PlayerWeeklyTotalTime definition

CREATE TABLE IF NOT EXISTS `PlayerWeeklyTotalTime` (
  `SteamId` bigint(20) DEFAULT NULL,
  `WTime` mediumint(9) DEFAULT NULL,
  `WeekNo` mediumint(9) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- PlayerWeeklyWTime definition

CREATE TABLE IF NOT EXISTS `PlayerWeeklyWTime` (
  `SteamId` bigint(20) DEFAULT NULL,
  `WTime` mediumint(9) DEFAULT NULL,
  `WeekNo` mediumint(9) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

", con);
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
            catch (Exception e)
            {
                ConsMsg(e.Message);
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