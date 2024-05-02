using MySqlConnector;
using Newtonsoft.Json;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    /*
        -- oyntch_nKxayDq.PlayerBattlePass definition
        CREATE TABLE `PlayerBattlePass` (
          `Id` bigint(20) NOT NULL AUTO_INCREMENT,
          `SteamId` bigint(20) DEFAULT NULL,
          `Level` mediumint(9) DEFAULT NULL,
          `Config` text DEFAULT NULL,
          `Completed` bit(1) DEFAULT b'0',
          `StartTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
          `EndTime` DATETIME NULL,
          PRIMARY KEY (`Id`)
        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
    */

    #region BP

    public static Dictionary<ulong, BattlePassBase> BattlePassDatas { get; set; } = new();

    private static BattlePassBase GetPlayerBattlePassData(ulong steamId)
    {
        try
        {
            if (BattlePassDatas.TryGetValue(steamId, out var data) && data != null)
            {
                return data;
            }

            using (var con = Connection())
            {
                if (con == null)
                {
                    return null;
                }
                var res = (BattlePassBase)null;

                var cmd = new MySqlCommand(@$"SELECT 1 FROM `PlayerBattlePass` where `SteamId` = @SteamId;", con);
                bool exist = false;
                cmd.Parameters.AddWithValue("@SteamId", steamId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        exist = true;
                    }
                }
                if (exist)
                {
                    cmd = new MySqlCommand(@$"SELECT
                                `Level`,`Config`,`Completed`
                                FROM `PlayerBattlePass`
                                where `SteamId` = @SteamId
                                order by `Level` desc limit 1;", con);
                    cmd.Parameters.AddWithValue("@SteamId", steamId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var level = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            if (level == 0)
                            {
                                res = InsertFreshAndReturn(steamId, con, out cmd);
                            }
                            else
                            {
                                var config = reader.IsDBNull(1) ? null : reader.GetString(1);
                                var completed = reader.IsDBNull(2) ? false : reader.GetBoolean(2);
                                if (config == null)
                                {
                                    res = GetBattlePassLevelConfig(level);
                                    res.Completed = completed;
                                }
                                else
                                {
                                    res = GetBattlePassLevelConfigFromString(level, config);
                                }
                            }
                        }
                    }
                }
                else
                {
                    res = InsertFreshAndReturn(steamId, con, out cmd);
                }
                res.SteamId = steamId;
                BattlePassDatas.TryAdd(steamId, res);
                return res;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return null;

        static BattlePassBase InsertFreshAndReturn(ulong steamId, MySqlConnection? con, out MySqlCommand cmd)
        {
            cmd = new MySqlCommand(@$"INSERT INTO `PlayerBattlePass`
                                          (SteamId,Level,Config,Completed)
                                          VALUES (@SteamId,@Level,@Config,0);", con);

            cmd.Parameters.AddWithValue("@SteamId", steamId);
            cmd.Parameters.AddWithValue("@Level", 1);
            var config = JsonConvert.SerializeObject(new BattlePass_Level01());
            cmd.Parameters.AddWithValue("@Config", config);
            cmd.ExecuteNonQuery();
            return new BattlePass_Level01();
        }
    }

    private static void UpdateBattlePassData(BattlePassBase config, bool completed)
    {
        try
        {
            ulong steamId = config.SteamId;
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
                var cmd = new MySqlCommand(@$"UPDATE `PlayerBattlePass`
                                          SET Config = @Config,
                                             Completed = @Completed,
                                             EndTime = @EndTime
                                          where `SteamId` = @SteamId and Level = @Level;", con);

                cmd.Parameters.AddWithValue("@SteamId", steamId);
                cmd.Parameters.AddWithValue("@Config", JsonConvert.SerializeObject(config));
                cmd.Parameters.AddWithValue("@Completed", completed);
                cmd.Parameters.AddWithValue("@Level", config.Level);
                cmd.Parameters.AddWithValue("@EndTime", completed == true ? DateTime.UtcNow : DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private static void UpdatePlayerBattlePassDataOnDisconnect(ulong steamId)
    {
        try
        {
            if (BattlePassDatas.TryGetValue(steamId, out var data))
            {
                UpdateBattlePassData(data, data.Completed);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    #endregion BP

    #region BPPremium

    public static Dictionary<ulong, BattlePassPremiumBase> BattlePassPremiumDatas { get; set; } = new();

    private static BattlePassPremiumBase GetPlayerBattlePassPremiumData(ulong steamId)
    {
        try
        {
            if (BattlePassPremiumDatas.TryGetValue(steamId, out var data) && data != null)
            {
                return data;
            }

            using (var con = Connection())
            {
                if (con == null)
                {
                    return null;
                }
                var res = (BattlePassPremiumBase)null;

                var cmd = new MySqlCommand(@$"SELECT 1 FROM `PlayerBattlePassPremium` where `SteamId` = @SteamId;", con);
                bool exist = false;
                cmd.Parameters.AddWithValue("@SteamId", steamId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        exist = true;
                    }
                }
                if (exist)
                {
                    cmd = new MySqlCommand(@$"SELECT
                                `Level`,`Config`,`Completed`
                                FROM `PlayerBattlePassPremium`
                                where `SteamId` = @SteamId
                                order by `Level` desc limit 1;", con);
                    cmd.Parameters.AddWithValue("@SteamId", steamId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var level = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            if (level == 0)
                            {
                                res = InsertFreshAndReturn(steamId, con, out cmd);
                            }
                            else
                            {
                                var config = reader.IsDBNull(1) ? null : reader.GetString(1);
                                var completed = reader.IsDBNull(2) ? false : reader.GetBoolean(2);
                                if (config == null)
                                {
                                    res = GetBattlePassPremiumLevelConfig(level);
                                    res.Completed = completed;
                                }
                                else
                                {
                                    res = GetBattlePassPremiumLevelConfigFromString(level, config);
                                }
                            }
                        }
                    }
                }
                else
                {
                    res = InsertFreshAndReturn(steamId, con, out cmd);
                }
                res.SteamId = steamId;
                BattlePassPremiumDatas.TryAdd(steamId, res);
                return res;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return null;

        static BattlePassPremiumBase InsertFreshAndReturn(ulong steamId, MySqlConnection? con, out MySqlCommand cmd)
        {
            cmd = new MySqlCommand(@$"INSERT INTO `PlayerBattlePassPremium`
                                          (SteamId,Level,Config,Completed)
                                          VALUES (@SteamId,@Level,@Config,0);", con);

            cmd.Parameters.AddWithValue("@SteamId", steamId);
            cmd.Parameters.AddWithValue("@Level", 1);
            var config = JsonConvert.SerializeObject(new BattlePassPremium_Level01());
            cmd.Parameters.AddWithValue("@Config", config);
            cmd.ExecuteNonQuery();
            return new BattlePassPremium_Level01();
        }
    }

    private static void UpdateBattlePassPremiumData(BattlePassPremiumBase config, bool completed)
    {
        try
        {
            ulong steamId = config.SteamId;
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
                var cmd = new MySqlCommand(@$"UPDATE `PlayerBattlePassPremium`
                                          SET Config = @Config,
                                             Completed = @Completed,
                                             EndTime = @EndTime
                                          where `SteamId` = @SteamId and Level = @Level;", con);

                cmd.Parameters.AddWithValue("@SteamId", steamId);
                cmd.Parameters.AddWithValue("@Config", JsonConvert.SerializeObject(config));
                cmd.Parameters.AddWithValue("@Completed", completed);
                cmd.Parameters.AddWithValue("@Level", config.Level);
                cmd.Parameters.AddWithValue("@EndTime", completed == true ? DateTime.UtcNow : DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private static void UpdatePlayerBattlePassPremiumDataOnDisconnect(ulong steamId)
    {
        try
        {
            if (BattlePassPremiumDatas.TryGetValue(steamId, out var data))
            {
                UpdateBattlePassPremiumData(data, data.Completed);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    #endregion BPPremium
}