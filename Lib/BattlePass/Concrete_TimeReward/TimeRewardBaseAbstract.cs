using CounterStrikeSharp.API.Core;
using MySqlConnector;
using Newtonsoft.Json;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public partial class TimeRewardBase
    {
        internal static void GiveReward(TimeRewardBase data, CCSPlayerController player)
        {
            if (data.Other != null)
            {
                player.PrintToChat($"{Prefix} {CC.B}{data.Other}");
            }
            if (data.Credit != 0)
            {
                try
                {
                    var marketData = GetPlayerMarketModel(player.SteamID);
                    if (marketData.Model != null)
                    {
                        marketData.Model.Credit += data.Credit;
                        PlayerMarketModels[player.SteamID] = marketData.Model;
                        player.PrintToChat($"{Prefix} {CC.B}{data.Credit} {CC.W}KREDI KAZANDIN!");
                        player.PrintToChat($"{Prefix} {CC.W}Mevcut Kredin = {CC.B}{marketData.Model.Credit}{CC.R}");
                    };
                }
                catch (Exception e)
                {
                    ConsMsg(e.Message);
                }
            }
            try
            {
                using (var con = Connection())
                {
                    if (con != null)
                    {
                        var cmd = new MySqlCommand(@$"UPDATE `PlayerTimeReward`
                                          SET Config = @Config,
                                             Completed = @Completed,
                                             EndTime = @EndTime
                                          where `SteamId` = @SteamId and Level = @Level;
                        ", con);

                        cmd.Parameters.AddWithValue("@SteamId", data.SteamId);
                        cmd.Parameters.AddWithValue("@Config", JsonConvert.SerializeObject(data));
                        cmd.Parameters.AddWithValue("@Completed", true);
                        cmd.Parameters.AddWithValue("@Level", data.Level);
                        cmd.Parameters.AddWithValue("@EndTime", DateTime.UtcNow);
                        cmd.ExecuteNonQuery();

                        var conf = GetTimeRewardLevelConfig(data.Level + 1);
                        cmd = new MySqlCommand(@$"INSERT INTO `PlayerTimeReward`
                                      (SteamId,Level,Config,Completed)
                                      VALUES (@SteamId,@Level,@Config,0);", con);

                        cmd.Parameters.AddWithValue("@SteamId", player.SteamID);
                        cmd.Parameters.AddWithValue("@Level", conf.Level);
                        cmd.Parameters.AddWithValue("@Config", JsonConvert.SerializeObject(conf));
                        cmd.ExecuteNonQuery();
                        conf.SteamId = player.SteamID;
                        TimeRewardDatas[conf.SteamId] = conf;
                    }
                }
            }
            catch (Exception e)
            {
                ConsMsg(e.Message);
            }
        }
    }
}