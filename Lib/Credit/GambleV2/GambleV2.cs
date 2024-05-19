using JailbreakExtras.Lib.Database;
using JailbreakExtras.Lib.Database.Models;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using static JailbreakExtras.JailbreakExtras;

namespace JailbreakExtras;

internal static class RuletChatShort
{
    internal static string GetShort(this RuletOptions opt) => opt switch
    {
        RuletOptions.Yesil => $"{CC.G}Y",
        RuletOptions.Siyah => $"{CC.Gr}S",
        RuletOptions.Kirmizi => $"{CC.R}K",
        _ => ""
    };
}

public partial class JailbreakExtras
{
    private string CtOfRulet(RuletOptions data) => data switch
    {
        RuletOptions.Yesil => $"{CC.G}Yeşil",
        RuletOptions.Kirmizi => $"{CC.R}Kırmızı",
        RuletOptions.Siyah => $"{CC.Gr}Siyah",
        _ => ""
    };

    /*

                  CREATE TABLE IF NOT EXISTS `PlayerGambleData` (
                         `Id` bigint(20) PRIMARY KEY AUTO_INCREMENT,
                         `GambleDataId` bigint(20) DEFAULT 0,
                         `SteamId` bigint(20) DEFAULT NULL,
                         `Credit` mediumint(9) DEFAULT 0,
                         `Color` mediumint(9) DEFAULT 0,
                         `Expired` bit DEFAULT 0,
                         `PlayedTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                         `ExpiredTime` DATETIME NULL
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

                    CREATE TABLE IF NOT EXISTS `GambleData` (
                         `Id` bigint(20) PRIMARY KEY AUTO_INCREMENT,
                         `StartTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                         `EndTime` DATETIME NULL,
                         `Red` mediumint(9) DEFAULT 0,
                         `Green` mediumint(9) DEFAULT 0,
                         `Black` mediumint(9) DEFAULT 0,
                         `Winner` mediumint(9) DEFAULT 0,
                         `ParticipationCount` mediumint(9) DEFAULT 0,
                         `TourFinalized` bit DEFAULT 0
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
    */

    private class GambleHistory
    {
        public long Id { get; set; }
        public int Red { get; set; } = 0;
        public int Green { get; set; } = 0;
        public int Black { get; set; } = 0;
        public RuletOptions Winner { get; set; }
        public int ParticipationCount { get; set; } = 0;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }

    private RuletOptions LatestRuletWinner { get; set; } = RuletOptions.None;
    private static Dictionary<long, GambleHistory> LastGambleDatas { get; set; } = new Dictionary<long, GambleHistory>();
    private static Dictionary<ulong, RuletData> RuletPlayers { get; set; } = new();
    private static List<RuletOptions> GambleLast70HistoryData { get; set; } = new();

    private class RuletData
    {
        public int Credit { get; set; }
        public RuletOptions Option { get; set; }
    }

    internal enum RuletOptions
    {
        None = 0,
        Yesil,
        Siyah,
        Kirmizi
    }

    public static long LastGambleDataId { get; set; } = 0;

    /// <summary>
    /// create new empty record to get id of the round
    /// </summary>
    private void Ruletv2RoundStart()
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

                cmd = new MySqlCommand(@$"INSERT INTO `GambleData` () VALUES (); SELECT LAST_INSERT_ID();", con);

                // Execute the insert query and retrieve the last inserted id
                LastGambleDataId = Convert.ToInt64(cmd.ExecuteScalar());
                LastGambleDatas = LastGambleDatas.OrderByDescending(x => x.Key).Take(20).ToDictionary(x => x.Key, y => y.Value);
                LastGambleDatas.TryAdd(LastGambleDataId,
                    new GambleHistory()
                    {
                        Id = LastGambleDataId,
                        StartTime = DateTime.UtcNow,
                    });
            }
        }
        catch (Exception e)
        {
            ConsMsg(e.Message);
        }
    }

    private void GetGambleHistoryData()
    {
        try
        {
            GambleLast70HistoryData = new();

            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
                MySqlCommand? cmd = new MySqlCommand(@$"
                    select `Winner` from `GambleData`
                    where `Winner` != 0 and `TourFinalized` = 1
                    order by `Id` desc
                    limit 70", con);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var data = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);

                        GambleLast70HistoryData.Add((RuletOptions)data);
                    }
                }
            }
        }
        catch (Exception e)
        {
            ConsMsg(e.Message);
        }
    }

    private void RuletV2RoundEnd(RuletOptions kazananRenk)
    {
        try
        {
            LatestRuletWinner = kazananRenk;
            if (LastGambleDatas.TryGetValue(LastGambleDataId, out var gambleHistory))
            {
                gambleHistory.Winner = kazananRenk;
                LastGambleDatas[LastGambleDataId] = gambleHistory;

                using (var con = Connection())
                {
                    var cmd = (MySqlCommand)null;
                    if (con == null)
                    {
                        return;
                    }

                    cmd = new MySqlCommand(@$"
                        UPDATE `GambleData`
                        SET `EndTime` = @EndTime,
                            `Winner` = @Winner,
                            `TourFinalized` = 1
                            WHERE `Id` = @Id;

                        UPDATE `PlayerGambleData`
                        SET `Expired` = 1,
                            `ExpiredTime` = @EndTime
                            WHERE `GambleDataId` = @Id and `ExpiredTime` is null;", con);

                    cmd.Parameters.AddWithValue("@Id", LastGambleDataId.GetDbValue());
                    cmd.Parameters.AddWithValue("@EndTime", DateTime.UtcNow.AddHours(3));
                    cmd.Parameters.AddWithValue("@Winner", kazananRenk.GetDbValue());

                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception e)
        {
            ConsMsg(e.Message);
        }
    }

    private void RuletV2OnPlayerBet(ulong steamId, int credit, RuletOptions opt)
    {
        try
        {
            if (LastGambleDatas.TryGetValue(LastGambleDataId, out var gambleHistory))
            {
                switch (opt)
                {
                    case RuletOptions.None:
                        break;

                    case RuletOptions.Yesil:
                        gambleHistory.Green += credit;
                        break;

                    case RuletOptions.Siyah:
                        gambleHistory.Black += credit;
                        break;

                    case RuletOptions.Kirmizi:
                        gambleHistory.Red += credit;
                        break;

                    default:
                        break;
                }
                gambleHistory.ParticipationCount += 1;
                LastGambleDatas[LastGambleDataId] = gambleHistory;

                using (var con = Connection())
                {
                    var cmd = (MySqlCommand)null;
                    if (con == null)
                    {
                        return;
                    }

                    cmd = new MySqlCommand(@$"INSERT INTO `PlayerGambleData`
                        (`GambleDataId`, `SteamId`, `Credit`, `Color`)
                        VALUES(@GambleDataId, @SteamId, @Credit, @Color);", con);
                    cmd.Parameters.AddWithValue("@GambleDataId", LastGambleDataId.GetDbValue());
                    cmd.Parameters.AddWithValue("@SteamId", steamId.GetDbValue());
                    cmd.Parameters.AddWithValue("@Credit", credit.GetDbValue());
                    cmd.Parameters.AddWithValue("@Color", opt.GetDbValue());

                    cmd.ExecuteNonQuery();

                    cmd = new MySqlCommand(@$"UPDATE `GambleData`
                        SET `Red` = @Red,
                            `Green` = @Green,
                            `Black` = @Black,
                            `ParticipationCount` = `ParticipationCount`+1
                            WHERE `Id` = @Id;", con);

                    cmd.Parameters.AddWithValue("@Id", LastGambleDataId.GetDbValue());
                    cmd.Parameters.AddWithValue("@Red", gambleHistory.Red.GetDbValue());
                    cmd.Parameters.AddWithValue("@Green", gambleHistory.Green.GetDbValue());
                    cmd.Parameters.AddWithValue("@Black", gambleHistory.Black.GetDbValue());

                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception e)
        {
            ConsMsg(e.Message);
        }
    }

    private void RuletV2OnPlayerCancelBet(ulong steamID, RuletData ruletPlay)
    {
        try
        {
            if (LastGambleDatas.TryGetValue(LastGambleDataId, out var gambleHistory))
            {
                switch (ruletPlay.Option)
                {
                    case RuletOptions.None:
                        break;

                    case RuletOptions.Yesil:
                        gambleHistory.Green -= ruletPlay.Credit;
                        break;

                    case RuletOptions.Siyah:
                        gambleHistory.Black -= ruletPlay.Credit;
                        break;

                    case RuletOptions.Kirmizi:
                        gambleHistory.Red -= ruletPlay.Credit;
                        break;

                    default:
                        break;
                }
                gambleHistory.ParticipationCount -= 1;
                LastGambleDatas[LastGambleDataId] = gambleHistory;

                using (var con = Connection())
                {
                    var cmd = (MySqlCommand)null;
                    if (con == null)
                    {
                        return;
                    }

                    cmd = new MySqlCommand(@$"DELETE FROM `PlayerGambleData` WHERE `GambleDataId` = @GambleDataId;", con);
                    cmd.Parameters.AddWithValue("@GambleDataId", LastGambleDataId.GetDbValue());

                    cmd.ExecuteNonQuery();

                    cmd = new MySqlCommand(@$"UPDATE `GambleData`
                        SET `Red` = @Red,
                            `Green` = @Green,
                            `Black` = @Black,
                            `ParticipationCount` = `ParticipationCount`-1
                            WHERE `Id` = @Id;", con);

                    cmd.Parameters.AddWithValue("@Id", LastGambleDataId.GetDbValue());
                    cmd.Parameters.AddWithValue("@Red", gambleHistory.Red.GetDbValue());
                    cmd.Parameters.AddWithValue("@Green", gambleHistory.Green.GetDbValue());
                    cmd.Parameters.AddWithValue("@Black", gambleHistory.Black.GetDbValue());

                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception e)
        {
            ConsMsg(e.Message);
        }
    }

    private static PlayerMarketModel RuletV2GetNotExpiredGambleData(ulong tempSteamId, PlayerMarketModel data)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return data;
                }
                List<long> removeIds = new List<long>();
                MySqlCommand? cmd = new MySqlCommand(@$"
                    select
                    	pgd.`Id`,
                    	`Credit`
                    from
                    	`PlayerGambleData` pgd
                    inner join `GambleData` gd on
                    	pgd.`GambleDataId` = gd.`Id`
                    where
                    	gd.`TourFinalized` = 0
                    	and gd.`EndTime` is null
                    	and pgd.`Expired` = 0
                    	and pgd.`ExpiredTime` is null
	                    and EXISTS (
                            SELECT 1
                            FROM `GambleData` gd2
                            WHERE gd2.`Id` > gd.`Id`
                        )
                    	and pgd.`SteamId` = @SteamId", con);
                cmd.Parameters.AddWithValue("@SteamId", tempSteamId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var pgdId = reader.IsDBNull(0) ? 0 : reader.GetInt64(0);
                        var credit = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                        removeIds.Add(pgdId);

                        data.Credit += credit;
                    }
                }

                if (removeIds.Count > 0)
                {
                    cmd = new MySqlCommand(@$"UPDATE `PlayerGambleData`
                        SET `Expired` = 1,
                            `ExpiredTime` = @EndTime
                            WHERE `Id` in ({string.Join(",", removeIds)});", con);

                    cmd.Parameters.AddWithValue("@EndTime", DateTime.UtcNow.AddHours(3));

                    cmd.ExecuteNonQuery();
                }
                else
                {
                    return data;
                }
            }
            return data;
        }
        catch (Exception e)
        {
            ConsMsg(e.Message);
            return data;
        }
    }

    private void RuletV2TopRulet()
    {
        try
        {
        }
        catch (Exception e)
        {
            ConsMsg(e.Message);
        }
    }
}