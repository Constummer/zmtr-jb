using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System.Globalization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public static Dictionary<ulong, PlayerTime> PlayerTimeTracking { get; set; } = new();

    public static Dictionary<ulong, int> AllPlayerTotalTimeTracking { get; set; } = new();

    public static Dictionary<ulong, int> AllPlayerCTTimeTracking { get; set; } = new();
    public static Dictionary<ulong, int> AllPlayerTTimeTracking { get; set; } = new();
    public static Dictionary<ulong, int> AllPlayerWTimeTracking { get; set; } = new();
    public static Dictionary<ulong, int> AllPlayerKaTimeTracking { get; set; } = new();
    public static Dictionary<ulong, int> AllPlayerWeeklyWTimeTracking { get; set; } = new();
    public static Dictionary<ulong, int> AllPlayerWeeklyCTTimeTracking { get; set; } = new();
    public static Dictionary<ulong, int> AllPlayerWeeklyTTimeTracking { get; set; } = new();
    public static Dictionary<ulong, int> AllPlayerWeeklyKaTimeTracking { get; set; } = new();
    public static Dictionary<ulong, int> AllPlayerWeeklyTotalTimeTracking { get; set; } = new();

    public class PlayerTime
    {
        public PlayerTime(int total, int cTTime, int tTime, int wTime, int kaTime, int weeklyWTime, int weeklyCTTime, int weeklyTTime, int weeklyTotalTime, int weeklyKaTime)
        {
            Total = total;
            CTTime = cTTime;
            TTime = tTime;
            WTime = wTime;
            KaTime = kaTime;
            WeeklyWTime = weeklyWTime;
            WeeklyCTTime = weeklyCTTime;
            WeeklyTTime = weeklyTTime;
            WeeklyTotalTime = weeklyTotalTime;
            WeeklyKaTime = weeklyKaTime;
        }

        public int Total { get; set; } = 0;
        public int CTTime { get; set; } = 0;
        public int TTime { get; set; } = 0;
        public int WTime { get; set; } = 0;
        public int KaTime { get; set; } = 0;
        public int WeeklyWTime { get; set; } = 0;
        public int WeeklyTTime { get; set; } = 0;
        public int WeeklyCTTime { get; set; } = 0;
        public int WeeklyTotalTime { get; set; } = 0;
        public int WeeklyKaTime { get; set; } = 0;
    }

    /*
       @"CREATE TABLE IF NOT EXISTS `PlayerTime` (
                  `SteamId` bigint(20) DEFAULT NULL,
                  `Total` mediumint(9) DEFAULT NULL,
                  `CTTime` mediumint(9) DEFAULT NULL,
                  `TTime` mediumint(9) DEFAULT NULL,
                  `WTime` mediumint(9) DEFAULT NULL,
                  `WeeklyWTime` mediumint(9) DEFAULT NULL,
                  `WeeklyCTTime` mediumint(9) DEFAULT NULL,
                  `WeeklyTTime` mediumint(9) DEFAULT NULL,
                  `WeeklyTotalTime` mediumint(9) DEFAULT NULL
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;", con);
    */

    private void UpdateAllTimeTrackingData()
    {
        using (var con = Connection())
        {
            if (con == null)
            {
                return;
            }
            GetAllTimeTrackingData(con);
        }
    }

    private void GetAllTimeTrackingData(MySqlConnection con)
    {
        if (con == null)
        {
            return;
        }

        try
        {
            AllPlayerTotalTimeTracking?.Clear();
            var cmd = new MySqlCommand(@$"SELECT `Total`,`SteamId`
                                          FROM `PlayerTime` order by `Total` desc limit 10;", con);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var data = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    var steamid = reader.IsDBNull(1) ? 0 : reader.GetInt64(1);
                    if (steamid != 0)
                    {
                        if (AllPlayerTotalTimeTracking.ContainsKey((ulong)steamid) == false)
                        {
                            AllPlayerTotalTimeTracking.Add((ulong)steamid, data);
                        }
                        else
                        {
                            AllPlayerTotalTimeTracking[(ulong)steamid] = data;
                        }
                    }
                }
            }

            AllPlayerWeeklyTotalTimeTracking?.Clear();
            cmd = new MySqlCommand(@$"SELECT `WeeklyTotalTime`,`SteamId`
                                          FROM `PlayerTime` order by `WeeklyTotalTime` desc limit 10;", con);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var data = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    var steamid = reader.IsDBNull(1) ? 0 : reader.GetInt64(1);
                    if (steamid != 0)
                    {
                        if (AllPlayerWeeklyTotalTimeTracking.ContainsKey((ulong)steamid) == false)
                        {
                            AllPlayerWeeklyTotalTimeTracking.Add((ulong)steamid, data);
                        }
                        else
                        {
                            AllPlayerWeeklyTotalTimeTracking[(ulong)steamid] = data;
                        }
                    }
                }
            }

            AllPlayerWTimeTracking?.Clear();
            cmd = new MySqlCommand(@$"SELECT `WTime`,`SteamId`
                                          FROM `PlayerTime` order by `WTime` desc limit 10;", con);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var data = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    var steamid = reader.IsDBNull(1) ? 0 : reader.GetInt64(1);
                    if (steamid != 0)
                    {
                        if (AllPlayerWTimeTracking.ContainsKey((ulong)steamid) == false)
                        {
                            AllPlayerWTimeTracking.Add((ulong)steamid, data);
                        }
                        else
                        {
                            AllPlayerWTimeTracking[(ulong)steamid] = data;
                        }
                    }
                }
            }

            AllPlayerWeeklyWTimeTracking?.Clear();
            cmd = new MySqlCommand(@$"SELECT `WeeklyWTime`,`SteamId`
                                          FROM `PlayerTime` order by `WeeklyWTime` desc limit 10;", con);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var data = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    var steamid = reader.IsDBNull(1) ? 0 : reader.GetInt64(1);
                    if (steamid != 0)
                    {
                        if (AllPlayerWeeklyWTimeTracking.ContainsKey((ulong)steamid) == false)
                        {
                            AllPlayerWeeklyWTimeTracking.Add((ulong)steamid, data);
                        }
                        else
                        {
                            AllPlayerWeeklyWTimeTracking[(ulong)steamid] = data;
                        }
                    }
                }
            }

            AllPlayerCTTimeTracking?.Clear();
            cmd = new MySqlCommand(@$"SELECT `CTTime`,`SteamId`
                                          FROM `PlayerTime` order by `CTTime` desc limit 10;", con);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var data = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    var steamid = reader.IsDBNull(1) ? 0 : reader.GetInt64(1);
                    if (steamid != 0)
                    {
                        if (AllPlayerCTTimeTracking.ContainsKey((ulong)steamid) == false)
                        {
                            AllPlayerCTTimeTracking.Add((ulong)steamid, data);
                        }
                        else
                        {
                            AllPlayerCTTimeTracking[(ulong)steamid] = data;
                        }
                    }
                }
            }

            AllPlayerWeeklyCTTimeTracking?.Clear();
            cmd = new MySqlCommand(@$"SELECT `WeeklyCTTime`,`SteamId`
                                          FROM `PlayerTime` order by `WeeklyCTTime` desc limit 10;", con);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var data = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    var steamid = reader.IsDBNull(1) ? 0 : reader.GetInt64(1);
                    if (steamid != 0)
                    {
                        if (AllPlayerWeeklyCTTimeTracking.ContainsKey((ulong)steamid) == false)
                        {
                            AllPlayerWeeklyCTTimeTracking.Add((ulong)steamid, data);
                        }
                        else
                        {
                            AllPlayerWeeklyCTTimeTracking[(ulong)steamid] = data;
                        }
                    }
                }
            }

            AllPlayerTTimeTracking?.Clear();
            cmd = new MySqlCommand(@$"SELECT `TTime`,`SteamId`
                                          FROM `PlayerTime` order by `TTime` desc limit 10;", con);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var data = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    var steamid = reader.IsDBNull(1) ? 0 : reader.GetInt64(1);
                    if (steamid != 0)
                    {
                        if (AllPlayerTTimeTracking.ContainsKey((ulong)steamid) == false)
                        {
                            AllPlayerTTimeTracking.Add((ulong)steamid, data);
                        }
                        else
                        {
                            AllPlayerTTimeTracking[(ulong)steamid] = data;
                        }
                    }
                }
            }

            AllPlayerWeeklyTTimeTracking?.Clear();
            cmd = new MySqlCommand(@$"SELECT `WeeklyTTime`,`SteamId`
                                          FROM `PlayerTime` order by `WeeklyTTime` desc limit 10;", con);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var data = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    var steamid = reader.IsDBNull(1) ? 0 : reader.GetInt64(1);
                    if (steamid != 0)
                    {
                        if (AllPlayerWeeklyTTimeTracking.ContainsKey((ulong)steamid) == false)
                        {
                            AllPlayerWeeklyTTimeTracking.Add((ulong)steamid, data);
                        }
                        else
                        {
                            AllPlayerWeeklyTTimeTracking[(ulong)steamid] = data;
                        }
                    }
                }
            }

            AllPlayerKaTimeTracking?.Clear();
            cmd = new MySqlCommand(@$"SELECT `KaTime`,`SteamId`
                                          FROM `PlayerTime` order by `KaTime` desc limit 10;", con);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var data = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    var steamid = reader.IsDBNull(1) ? 0 : reader.GetInt64(1);
                    if (steamid != 0)
                    {
                        if (AllPlayerKaTimeTracking.ContainsKey((ulong)steamid) == false)
                        {
                            AllPlayerKaTimeTracking.Add((ulong)steamid, data);
                        }
                        else
                        {
                            AllPlayerKaTimeTracking[(ulong)steamid] = data;
                        }
                    }
                }
            }

            AllPlayerWeeklyKaTimeTracking?.Clear();
            cmd = new MySqlCommand(@$"SELECT `WeeklyKaTime`,`SteamId`
                                          FROM `PlayerTime` order by `WeeklyKaTime` desc limit 10;", con);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var data = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    var steamid = reader.IsDBNull(1) ? 0 : reader.GetInt64(1);
                    if (steamid != 0)
                    {
                        if (AllPlayerWeeklyKaTimeTracking.ContainsKey((ulong)steamid) == false)
                        {
                            AllPlayerWeeklyKaTimeTracking.Add((ulong)steamid, data);
                        }
                        else
                        {
                            AllPlayerWeeklyKaTimeTracking[(ulong)steamid] = data;
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    private static void InsertAndGetTimeTrackingData(ulong steamID)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
                PlayerTime data = null;
                var cmd = new MySqlCommand(@$"SELECT
                                        `Total`,`CTTime`,`TTime`,`WTime`,`KaTime`,
                                        `WeeklyWTime`,`WeeklyCTTime`,`WeeklyTTime`,`WeeklyTotalTime`,`WeeklyKaTime`
                                          FROM `PlayerTime`
                                          WHERE `SteamId` = @SteamId;", con);
                cmd.Parameters.AddWithValue("@SteamId", steamID);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        data = new PlayerTime(
                           reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                           reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                           reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                           reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                           reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                           reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                           reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                           reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                           reader.IsDBNull(8) ? 0 : reader.GetInt32(8),
                           reader.IsDBNull(9) ? 0 : reader.GetInt32(9));

                        if (PlayerTimeTracking.ContainsKey(steamID) == false)
                        {
                            PlayerTimeTracking.Add(steamID, data);
                        }
                        else
                        {
                            PlayerTimeTracking[steamID] = data;
                        }
                        return;
                    }
                }

                cmd = new MySqlCommand(@$"INSERT INTO `PlayerTime`
                                          (`SteamId`,`Total`,`CTTime`,`TTime`,`WTime`,`KaTime`,
                                           `WeeklyWTime`,`WeeklyCTTime`,`WeeklyTTime`,`WeeklyTotalTime`,`WeeklyKaTime`)
                                          VALUES
                                          (@SteamId, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);", con);

                cmd.Parameters.AddWithValue("@SteamId", steamID);
                cmd.ExecuteNonQuery();
                data = new PlayerTime(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

                if (PlayerTimeTracking.ContainsKey(steamID) == false)
                {
                    PlayerTimeTracking.Add(steamID, data);
                }
                else
                {
                    PlayerTimeTracking[steamID] = data;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private void UpdatePlayerTimeDataBulk(int multipler)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
                var calcRemainTime = CalculateMinutesUntilSundayMidnight();

                if (calcRemainTime > 1)
                {
                    List<MySqlParameter> parameters = new List<MySqlParameter>();
                    var count = GetPlayerCount();
                    var cmdText = "";
                    var i = 0;

                    GetPlayers()
                     .ToList()
                     .ForEach(x =>
                    {
                        if (PlayerTimeTracking.TryGetValue(x.SteamID, out var value))
                        {
                            cmdText += @$"UPDATE `PlayerTime`
                                         SET
                                            `Total` = @Total_{i},
                                            `CTTime` = @CTTime_{i},
                                            `TTime` = @TTime_{i},
                                            `WTime` = @Wtime_{i},
                                            `KaTime` = @KaTime_{i},
                                            `WeeklyWTime` = @WeeklyWTime_{i},
                                            `WeeklyTotalTime` = @WeeklyTotalTime_{i},
                                            `WeeklyTTime` = @WeeklyTTime_{i},
                                            `WeeklyCTTime` = @WeeklyCTTime_{i},
                                            `WeeklyKaTime` = @WeeklyKaTime_{i}
                                        WHERE `SteamId` = @SteamId_{i};";
                            value.Total += 1 * multipler;
                            value.WeeklyTotalTime += 1 * multipler;
                            var team = GetTeam(x);
                            if (team == CsTeam.CounterTerrorist)
                            {
                                value.CTTime += 1 * multipler;
                                value.WeeklyCTTime += 1 * multipler;
                                if (LatestWCommandUser == x.SteamID)
                                {
                                    value.WTime += 1 * multipler;
                                    value.WeeklyWTime += 1 * multipler;
                                }
                            }
                            else if (team == CsTeam.Terrorist)
                            {
                                value.TTime += 1 * multipler;
                                value.WeeklyTTime += 1 * multipler;
                            }
                            if (KomutcuAdminId == x.SteamID)
                            {
                                value.KaTime += 1 * multipler;
                                value.WeeklyKaTime += 1 * multipler;
                            }

                            parameters.Add(new MySqlParameter($"@SteamId_{i}", x.SteamID));
                            parameters.Add(new MySqlParameter($"@Total_{i}", value.Total));
                            parameters.Add(new MySqlParameter($"@CTTime_{i}", value.CTTime));
                            parameters.Add(new MySqlParameter($"@TTime_{i}", value.TTime));
                            parameters.Add(new MySqlParameter($"@WTime_{i}", value.WTime));
                            parameters.Add(new MySqlParameter($"@KaTime_{i}", value.KaTime));
                            parameters.Add(new MySqlParameter($"@WeeklyWTime_{i}", value.WeeklyWTime));
                            parameters.Add(new MySqlParameter($"@WeeklyCTTime_{i}", value.WeeklyCTTime));
                            parameters.Add(new MySqlParameter($"@WeeklyTTime_{i}", value.WeeklyTTime));
                            parameters.Add(new MySqlParameter($"@WeeklyTotalTime_{i}", value.WeeklyTotalTime));
                            parameters.Add(new MySqlParameter($"@WeeklyKaTime_{i}", value.WeeklyKaTime));
                            PlayerTimeTracking[x.SteamID] = value;
                            i++;
                        }
                    });
                    if (string.IsNullOrWhiteSpace(cmdText))
                    {
                        return;
                    }
                    var cmd = new MySqlCommand(cmdText, con);
                    cmd.Parameters.AddRange(parameters.ToArray());
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    SendWeeklyAllData(con);
                }
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    private void SendWeeklyAllData(MySqlConnection? con)
    {
        Server.PrintToChatAll($"{Prefix} {CC.R} BİRKAÇ SANİYE LAG GİRECEK PAZAR GECESİ 00.00 İŞLEMİ OLDUĞU İÇİN, SIKI TUTUNUN");
        Server.PrintToChatAll($"{Prefix} {CC.R} BİRKAÇ SANİYE LAG GİRECEK PAZAR GECESİ 00.00 İŞLEMİ OLDUĞU İÇİN, SIKI TUTUNUN");
        Server.PrintToChatAll($"{Prefix} {CC.R} BİRKAÇ SANİYE LAG GİRECEK PAZAR GECESİ 00.00 İŞLEMİ OLDUĞU İÇİN, SIKI TUTUNUN");
        Server.PrintToChatAll($"{Prefix} {CC.R} BİRKAÇ SANİYE LAG GİRECEK PAZAR GECESİ 00.00 İŞLEMİ OLDUĞU İÇİN, SIKI TUTUNUN");

        UpdateWeeklyTime(con);
        foreach (var item in PlayerTimeTracking.ToList())
        {
            item.Value.WeeklyWTime = 0;
            item.Value.WeeklyTTime = 0;
            item.Value.WeeklyCTTime = 0;
            item.Value.WeeklyTotalTime = 0;
            item.Value.WeeklyKaTime = 0;
            PlayerTimeTracking[item.Key] = item.Value;
        }
        AllPlayerWeeklyWTimeTracking?.Clear();
        AllPlayerWeeklyCTTimeTracking?.Clear();
        AllPlayerWeeklyTTimeTracking?.Clear();
        AllPlayerWeeklyTotalTimeTracking?.Clear();
        AllPlayerWeeklyKaTimeTracking?.Clear();
        KomWeeklyWCredits?.Clear();

        var cmd = new MySqlCommand(@"UPDATE `PlayerTime`
                                            SET `WeeklyWTime` = 0,
                                                `WeeklyCTTime` = 0,
                                                `WeeklyTTime` = 0,
                                                `WeeklyTotalTime` = 0,
                                                `WeeklyKaTime` = 0;", con);
        cmd.ExecuteNonQuery();

        cmd = new MySqlCommand(@"UPDATE `PlayerIsTop`
                                             SET `KillCount` = 0,`TotalKillCount` = `TotalKillCount` + `KillCount`;", con);
        cmd.ExecuteNonQuery();

        IsTopWeeklyNotifyDc();
        foreach (var item in IsTopDatas)
        {
            IsTopDatas[item.Key] = 0;
        }

        Server.PrintToChatAll($"{Prefix} {CC.R} SIKI TUTUNDUĞUNUZ İÇİN TŞK, DEVAAAAAM");
        Server.PrintToChatAll($"{Prefix} {CC.R} SIKI TUTUNDUĞUNUZ İÇİN TŞK, DEVAAAAAM");
        Server.PrintToChatAll($"{Prefix} {CC.R} SIKI TUTUNDUĞUNUZ İÇİN TŞK, DEVAAAAAM");
        Server.PrintToChatAll($"{Prefix} {CC.R} SIKI TUTUNDUĞUNUZ İÇİN TŞK, DEVAAAAAM");
    }

    private void UpdateWeeklyTime(MySqlConnection con)
    {
        var dicw = new Dictionary<long, int>();
        var dicct = new Dictionary<long, int>();
        var dict = new Dictionary<long, int>();
        var dictotal = new Dictionary<long, int>();
        var dicka = new Dictionary<long, int>();
        var cmd = new MySqlCommand(@$"SELECT `SteamId`,`WeeklyWTime` FROM `PlayerTime` order by `WeeklyWTime` desc;", con);
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                var steamid = reader.IsDBNull(0) ? 0 : reader.GetInt64(0);
                var wtime = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                if (steamid > 0)
                {
                    if (IsKomutcuPlayer((ulong)steamid))
                    {
                        AddToDic(dicw, steamid, wtime);
                    }
                }
            }
        }
        cmd = new MySqlCommand(@$"SELECT `SteamId`,`WeeklyCTTime` FROM `PlayerTime` order by `WeeklyCTTime` desc limit 10;", con);
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                var steamid = reader.IsDBNull(0) ? 0 : reader.GetInt64(0);
                var weeklyTime = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                if (steamid > 0)
                {
                    AddToDic(dicct, steamid, weeklyTime);
                }
            }
        }
        cmd = new MySqlCommand(@$"SELECT `SteamId`,`WeeklyTTime` FROM `PlayerTime` order by `WeeklyTTime` desc limit 10;", con);
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                var steamid = reader.IsDBNull(0) ? 0 : reader.GetInt64(0);
                var weeklyTime = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                if (steamid > 0)
                {
                    AddToDic(dict, steamid, weeklyTime);
                }
            }
        }
        cmd = new MySqlCommand(@$"SELECT `SteamId`,`WeeklyTotalTime` FROM `PlayerTime` order by `WeeklyTotalTime` desc limit 10;", con);
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                var steamid = reader.IsDBNull(0) ? 0 : reader.GetInt64(0);
                var weeklyTime = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                if (steamid > 0)
                {
                    AddToDic(dictotal, steamid, weeklyTime);
                }
            }
        }
        cmd = new MySqlCommand(@$"SELECT `SteamId`,`WeeklyKaTime` FROM `PlayerTime` order by `WeeklyKaTime` desc limit 10;", con);
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                var steamid = reader.IsDBNull(0) ? 0 : reader.GetInt64(0);
                var weeklyTime = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                if (steamid > 0)
                {
                    AddToDic(dicka, steamid, weeklyTime);
                }
            }
        }

        if (dicw.Count > 0)
        {
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            var weekno = GetIso8601WeekOfYear(DateTime.UtcNow.AddHours(3));
            var cmdText = "";
            var i = 0;
            parameters.Add(new MySqlParameter($"@WeekNo", weekno));

            dicw.ToList()
             .ForEach(x =>
             {
                 cmdText += @$"INSERT INTO `PlayerWeeklyWTime`
                                          (`SteamId`,`WTime`,`WeekNo`)
                                          VALUES
                                          (@SteamId_{i},@WTime_{i}, @WeekNo);";
                 parameters.Add(new MySqlParameter($"@SteamId_{i}", x.Key));
                 parameters.Add(new MySqlParameter($"@WTime_{i}", x.Value));

                 i++;
             });
            if (string.IsNullOrWhiteSpace(cmdText))
            {
                return;
            }

            cmd = new MySqlCommand(cmdText, con);
            cmd.Parameters.AddRange(parameters.ToArray());
            cmd.ExecuteNonQuery();
        }

        SendWarningForLessThan7HrsAWeekKomutcu(dicw, true);
        SendWarningForLessThan7HrsAWeekKomutcu(dicct, false, "CT");
        SendWarningForLessThan7HrsAWeekKomutcu(dict, false, "T");
        SendWarningForLessThan7HrsAWeekKomutcu(dictotal, false, "Total");
        SendWarningForLessThan7HrsAWeekKomutcu(dicka, false, "KA");
    }

    private static void AddToDic(Dictionary<long, int> dic, long steamid, int time)
    {
        if (dic.TryGetValue(steamid, out var oldtime))
        {
            if (oldtime > time)
            {
                dic[steamid] = oldtime;
            }
            else
            {
                dic[steamid] = time;
            }
        }
        else
        {
            dic.Add(steamid, time);
        }
    }

    private void SendWarningForLessThan7HrsAWeekKomutcu(Dictionary<long, int> dic, bool uriFirst, string title = null)
    {
        string url;
        if (uriFirst)
        {
            url = WardenDcWebHook;
        }
        else
        {
            url = Total_T_CTDcWebHook;
        }
        var msg = string.Empty;
        foreach (var item in dic.ToList())
        {
            try
            {
                var tempName = PlayerNamesDatas.TryGetValue((ulong)item.Key, out var name) != false
                              ? name : "-----";
                if (msg == string.Empty)
                {
                    msg = $"{tempName} | {item.Key} | H. {(title ?? "W")}= {(item.Value < 120 ? $"{item.Value} dk" : $"{(int)(item.Value / 60)} s")}";
                }
                else
                {
                    msg += $"\n{tempName} | {item.Key} | H. {(title ?? "W")}= {(item.Value < 120 ? $"{item.Value} dk" : $"{(int)(item.Value / 60)} s")}";
                }
            }
            catch
            {
                continue;
            }
        }
        int maxLengthPerRequest = 1999;

        // Call the method with substrings of the long string
        for (int i = 0; i < msg.Length; i += maxLengthPerRequest)
        {
            int length = Math.Min(maxLengthPerRequest, msg.Length - i);
            string substring = msg.Substring(i, length);

            // Call your method with the substring
            DiscordPost(url, substring);
        }
    }

    public static int GetIso8601WeekOfYear(DateTime dt)
    {
        ///If its Monday, Tuesday or Wednesday, then it'll
        // be the same week# as whatever Thursday, Friday or Saturday are,
        // and we always get those right
        DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(dt);
        if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
        {
            dt = dt.AddDays(3);
        }

        // Return the week of our adjusted day
        return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(dt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }
}