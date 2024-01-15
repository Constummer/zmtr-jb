using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public static Dictionary<ulong, PlayerTime> PlayerTimeTracking { get; set; } = new();
    public static Dictionary<ulong, PlayerTime> AllPlayerTimeTracking { get; set; } = new();

    public class PlayerTime
    {
        public PlayerTime(int total, int cTTime, int tTime, int wTime, int weeklyWTime)
        {
            Total = total;
            CTTime = cTTime;
            TTime = tTime;
            WTime = wTime;
            WeeklyWTime = weeklyWTime;
        }

        public int Total { get; set; } = 0;
        public int CTTime { get; set; } = 0;
        public int TTime { get; set; } = 0;
        public int WTime { get; set; } = 0;
        public int WeeklyWTime { get; set; } = 0;
    }

    /*
       @"CREATE TABLE IF NOT EXISTS `PlayerTime` (
                  `SteamId` bigint(20) DEFAULT NULL,
                  `Total` mediumint(9) DEFAULT NULL,
                  `CTTime` mediumint(9) DEFAULT NULL,
                  `TTime` mediumint(9) DEFAULT NULL,
                  `WTime` mediumint(9) DEFAULT NULL
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
            PlayerTime data = null;
            var cmd = new MySqlCommand(@$"SELECT `Total`,`CTTime`,`TTime`,`WTime`,`WeeklyWTime`,`SteamId`
                                          FROM `PlayerTime`;", con);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    data = new PlayerTime(
                       reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                       reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                       reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                       reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                       reader.IsDBNull(4) ? 0 : reader.GetInt32(4));
                    var steamid = reader.IsDBNull(5) ? 0 : reader.GetInt64(5);
                    if (steamid != 0)
                    {
                        if (AllPlayerTimeTracking.ContainsKey((ulong)steamid) == false)
                        {
                            AllPlayerTimeTracking.Add((ulong)steamid, data);
                        }
                        else
                        {
                            AllPlayerTimeTracking[(ulong)steamid] = data;
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
                var cmd = new MySqlCommand(@$"SELECT `Total`,`CTTime`,`TTime`,`WTime`,`WeeklyWTime`
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
                           reader.IsDBNull(4) ? 0 : reader.GetInt32(4));

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
                                          (`SteamId`,`Total`,`CTTime`,`TTime`,`WTime`,`WeeklyWTime`)
                                          VALUES
                                          (@SteamId, 0, 0, 0, 0, 0);", con);

                cmd.Parameters.AddWithValue("@SteamId", steamID);
                cmd.ExecuteNonQuery();
                data = new PlayerTime(0, 0, 0, 0, 0);

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
        }
    }

    private void UpdatePlayerTimeDataBulk()
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
                                            `WeeklyWTime` = @WeeklyWTime_{i}
                                        WHERE `SteamId` = @SteamId_{i};";
                            value.Total++;
                            var team = GetTeam(x);
                            if (team == CsTeam.CounterTerrorist)
                            {
                                value.CTTime++;
                            }
                            else if (team == CsTeam.Terrorist)
                            {
                                value.TTime++;
                            }
                            if (LatestWCommandUser == x.SteamID)
                            {
                                value.WTime++;
                                value.WeeklyWTime++;
                            }

                            parameters.Add(new MySqlParameter($"@SteamId_{i}", x.SteamID));
                            parameters.Add(new MySqlParameter($"@Total_{i}", value.Total));
                            parameters.Add(new MySqlParameter($"@CTTime_{i}", value.CTTime));
                            parameters.Add(new MySqlParameter($"@TTime_{i}", value.TTime));
                            parameters.Add(new MySqlParameter($"@WTime_{i}", value.WTime));
                            parameters.Add(new MySqlParameter($"@WeeklyWTime_{i}", value.WeeklyWTime));
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
                    Server.PrintToChatAll($"{Prefix} {CC.R} BİRKAÇ SANİYE LAG GİRECEK PAZAR GECESİ 00.00 İŞLEMİ OLDUĞU İÇİN, SIKI TUTUNUN");
                    Server.PrintToChatAll($"{Prefix} {CC.R} BİRKAÇ SANİYE LAG GİRECEK PAZAR GECESİ 00.00 İŞLEMİ OLDUĞU İÇİN, SIKI TUTUNUN");
                    Server.PrintToChatAll($"{Prefix} {CC.R} BİRKAÇ SANİYE LAG GİRECEK PAZAR GECESİ 00.00 İŞLEMİ OLDUĞU İÇİN, SIKI TUTUNUN");
                    Server.PrintToChatAll($"{Prefix} {CC.R} BİRKAÇ SANİYE LAG GİRECEK PAZAR GECESİ 00.00 İŞLEMİ OLDUĞU İÇİN, SIKI TUTUNUN");

                    var cmd = new MySqlCommand(@$"SELECT `SteamId`,`WeeklyWTime` FROM `PlayerTime`;", con);
                    var dic = new Dictionary<long, int>();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var steamid = reader.IsDBNull(0) ? 0 : reader.GetInt64(0);
                            var time = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                            if (steamid > 0)
                            {
                                if (IsKomutcuPlayer((ulong)steamid))
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
                            }
                        }
                    }

                    if (dic.Count == 0)
                    {
                        return;
                    }
                    List<MySqlParameter> parameters = new List<MySqlParameter>();
                    var weekno = GetIso8601WeekOfYear(DateTime.UtcNow.AddHours(3));
                    var cmdText = "";
                    var i = 0;
                    parameters.Add(new MySqlParameter($"@WeekNo", weekno));

                    dic.ToList()
                     .ForEach(x =>
                     {
                         cmdText += @$"UPDATE `PlayerTime`
                                         SET `WeeklyWTime` = 0
                                        WHERE `SteamId` = @SteamId_{i};

                               INSERT INTO `PlayerWeeklyWTime`
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

                    SendWarningForLessThan7HrsAWeekKomutcu(dic);
                    foreach (var item in AllPlayerTimeTracking.ToList())
                    {
                        item.Value.WeeklyWTime = 0;
                        AllPlayerTimeTracking[item.Key] = item.Value;
                    }
                    foreach (var item in PlayerTimeTracking.ToList())
                    {
                        item.Value.WeeklyWTime = 0;
                        PlayerTimeTracking[item.Key] = item.Value;
                    }
                    Server.PrintToChatAll($"{Prefix} {CC.R} SIKI TUTUNDUĞUNUZ İÇİN TŞK, DEVAAAAAM");
                    Server.PrintToChatAll($"{Prefix} {CC.R} SIKI TUTUNDUĞUNUZ İÇİN TŞK, DEVAAAAAM");
                    Server.PrintToChatAll($"{Prefix} {CC.R} SIKI TUTUNDUĞUNUZ İÇİN TŞK, DEVAAAAAM");
                    Server.PrintToChatAll($"{Prefix} {CC.R} SIKI TUTUNDUĞUNUZ İÇİN TŞK, DEVAAAAAM");
                }
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    private void SendWarningForLessThan7HrsAWeekKomutcu(Dictionary<long, int> dic)
    {
        var url = "https://discord.com/api/webhooks/1194758709344215090/-XRiPj35x-KTHRtAyWlB5i1I16lFylHl_17we6SOS5HbYY5JCFPQYiOjYot6trvQiUcR";
        var msg = string.Empty;
        foreach (var item in dic.ToList())
        {
            try
            {
                var tempName = PlayerNamesDatas.TryGetValue((ulong)item.Key, out var name) != false
                              ? name : "-----(ismi net deil)";
                if (msg == string.Empty)
                {
                    msg = $"PlayerName = {tempName} | SteamId = {item.Key} | Haftalık = {(item.Value < 120 ? $"{item.Value} dk" : $"{(int)(item.Value / 60)} saat")}";
                }
                else
                {
                    msg += $"\nPlayerName = {tempName} | SteamId = {item.Key} | Haftalık = {(item.Value < 120 ? $"{item.Value} dk" : $"{(int)(item.Value / 60)} saat")}";
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

    private static void DiscordPost(string uri, string message)
    {
        try
        {
            var body = JsonSerializer.Serialize(new { content = message });
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage res = _httpClient.PostAsync($"{uri}", content).GetAwaiter().GetResult().EnsureSuccessStatusCode();
        }
        catch
        {
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