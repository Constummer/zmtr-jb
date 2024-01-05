using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System.Reflection.PortableExecutable;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public Dictionary<ulong, PlayerTime> PlayerTimeTracking { get; set; } = new();

    public class PlayerTime
    {
        public PlayerTime(int total, int cTTime, int tTime, int wTime)
        {
            Total = total;
            CTTime = cTTime;
            TTime = tTime;
            WTime = wTime;
        }

        public int Total { get; set; } = 0;
        public int CTTime { get; set; } = 0;
        public int TTime { get; set; } = 0;
        public int WTime { get; set; } = 0;
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

    private void InsertAndGetTimeTrackingData(ulong steamID)
    {
        var con = Connection();
        if (con == null)
        {
            return;
        }

        try
        {
            PlayerTime data = null;
            var cmd = new MySqlCommand(@$"SELECT `Total`,`CTTime`,`TTime`,`WTime`
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
                       reader.IsDBNull(3) ? 0 : reader.GetInt32(3));

                    if (PlayerTimeTracking.ContainsKey(steamID) == false)
                    {
                        PlayerTimeTracking.Add(steamID, data);
                    }
                    return;
                }
            }

            cmd = new MySqlCommand(@$"INSERT INTO `PlayerLevel`
                                          (`SteamId`,`Total`,`CTTime`,`TTime`,`WTime`)
                                          VALUES
                                          (@SteamId, 0, 0, 0, 0);", con);

            cmd.Parameters.AddWithValue("@SteamId", steamID);
            cmd.ExecuteNonQuery();
            data = new PlayerTime(0, 0, 0, 0);

            if (PlayerTimeTracking.ContainsKey(steamID) == false)
            {
                PlayerTimeTracking.Add(steamID, data);
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }

    private void UpdatePlayerTimeDataBulk()
    {
        try
        {
            var con = Connection();
            if (con == null)
            {
                return;
            }

            var cmd = new MySqlCommand();
            cmd.Connection = con;
            var cmdText = "";
            var i = 0;
            GetPlayers()
             .ToList()
             .ForEach(x =>
             {
                 if (PlayerTimeTracking.TryGetValue(x.SteamID, out var value))
                 {
                     cmdText += @$"UPDATE `PlayerLevel`
                                         SET
                                            `Total` = @Total_{i},
                                            `CTTime` = @CTTime_{i},
                                            `TTime` = @TTime_{i},
                                            `WTime` = @Wtime_{i}
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
                     }

                     cmd.Parameters.AddWithValue($"@SteamId_{i}", x.SteamID);
                     cmd.Parameters.AddWithValue($"@Total_{i}", value.Total);
                     cmd.Parameters.AddWithValue($"@CTTime_{i}", value.CTTime);
                     cmd.Parameters.AddWithValue($"@TTime_{i}", value.TTime);
                     cmd.Parameters.AddWithValue($"@WTime_{i}", value.WTime);
                     PlayerTimeTracking[x.SteamID] = value;
                 }
                 else
                 {
                     value = new(0, 0, 0, 0);
                     PlayerTimeTracking.Add(x.SteamID, value);
                 }
                 i++;
             });
            cmd.CommandText = cmdText;
            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }
}