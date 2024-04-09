using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static void WardenRefreshPawn()
    {
        var warden = GetWarden();
        if (warden != null && warden.PawnIsAlive)
        {
            return;
            RefreshPawn(warden);
        }
    }

    private static void WardenUnmute()
    {
        var warden = GetWarden();
        if (warden != null)
        {
            warden.VoiceFlags &= ~VoiceFlags.Muted;
        }
    }

    private static void LastWardenMute()
    {
        var warden = GetWarden();
        if (warden != null)
        {
            warden.VoiceFlags |= VoiceFlags.Muted;
        }
    }

    private void RemoveWarden()
    {
        Server.NextFrame(() =>
        {
            WardenRefreshPawn();
            LastWardenMute();
            ClearLasers();
            CoinRemove();
            WardenLeaveSound();
            LatestWCommandUser = null;
            CleanTagOnKomutcuAdmin();
        });
    }

    public Dictionary<ulong, int> PlayerWTimeDatas { get; set; } = new();

    /// <summary>
    /// CREATE TABLE IF NOT EXISTS `PlayerWTime` (
    ///                     `SteamId` bigint(20) DEFAULT NULL,
    ///                     `WTime` mediumint(9) DEFAULT 0
    /// </summary>
    /// <param name="con"></param>
    private void GetAllWTimes(MySqlConnection con)
    {
        try
        {
            if (con == null)
            {
                return;
            }
            MySqlCommand? cmd = new MySqlCommand(@$"SELECT `SteamId`,`WTime` FROM `PlayerWTime`;", con);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var steamId = reader.IsDBNull(0) ? 0 : reader.GetInt64(0);
                    var wTime = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);

                    if (PlayerWTimeDatas.ContainsKey((ulong)steamId) == false)
                    {
                        PlayerWTimeDatas.Add((ulong)steamId, wTime);
                    }
                    else
                    {
                        PlayerWTimeDatas[(ulong)steamId] = wTime;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private void SaveWTimes()
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
                List<MySqlParameter> parameters = new List<MySqlParameter>();
                var cmdText = "";
                var i = 0;
                GetPlayers()
                 .ToList()
                 .ForEach(x =>
                 {
                     if (AdminManager.PlayerHasPermissions(x, "@css/komutcu"))
                     {
                         if (PlayerWTimeDatas.TryGetValue(x.SteamID, out var value))
                         {
                             cmdText += @$"UPDATE `PlayerWTime`
                                         SET `WTime` = @Wtime_{i}
                                        WHERE `SteamId` = @SteamId_{i};";

                             if (LatestWCommandUser == x.SteamID)
                             {
                                 value = value + 5;
                             }

                             parameters.Add(new MySqlParameter($"@SteamId_{i}", x.SteamID));
                             parameters.Add(new MySqlParameter($"@WTime_{i}", value));
                             PlayerWTimeDatas[x.SteamID] = value;
                             i++;
                         }
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
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }
}