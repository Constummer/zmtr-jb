using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using JailbreakExtras.Lib.Database;
using MySqlConnector;
using System.Linq;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    /// <summary>
    /// CREATE TABLE IF NOT EXISTS `PlayerIsTop` (
    ///                   `SteamId` bigint(20) DEFAULT NULL,
    ///                   `KillCount` mediumint(9) DEFAULT 0
    ///                 ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
    /// </summary>
    public static Dictionary<ulong, int> IsTopDatas { get; set; } = new();

    public static ulong? CurrentRoundWKillerId { get; set; } = null;

    [ConsoleCommand("istop")]
    public void IsTop(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false) return;

        var playerIsCount = 0;
        bool exist = false;
        if (player?.SteamID != null && player!.SteamID != 0)
        {
            if (IsTopDatas.TryGetValue(player.SteamID, out playerIsCount))
            {
                exist = true;
            }
        }
        if (exist)
        {
            player.PrintToChat($"{Prefix} {CC.W} ------===------------===------");
            player.PrintToChat($"{Prefix} {CC.W} Toplam {CC.G}{playerIsCount} {CC.W}kere isyan yaptın!");
        }
        player.PrintToChat($"{Prefix} {CC.W} ------===------------===------");
        player.PrintToChat($"{Prefix} {CC.W} TOP 10 İsyancılar");
        var temp = IsTopDatas.OrderByDescending(x => x.Value).Take(10).ToList();
        foreach (var item in temp)
        {
            if (PlayerNamesDatas.TryGetValue(item.Key, out var name))
            {
                var tempName = name;
                if (tempName?.Length > 20)
                {
                    tempName = tempName.Substring(0, 17) + "...";
                }
                tempName = tempName?.PadRight(20, '_');
                player.PrintToChat($"{Prefix} {CC.G}{tempName} {CC.W}| {CC.B}{item.Value} {CC.Ol}kere,");
            }
        }
        player.PrintToChat($"{Prefix} {CC.W} İsyan yapmış.");
        player.PrintToChat($"{Prefix} {CC.W} ------===------------===------");
    }

    private static void UpdatePlayerIsTopData(ulong steamId)
    {
        try
        {
            if (IsTopDatas.TryGetValue(steamId, out var data))
            {
                using (var con = Connection())
                {
                    if (con == null)
                    {
                        return;
                    }

                    var cmd = new MySqlCommand(@$"SELECT 1 FROM `PlayerIsTop` WHERE `SteamId` = @SteamId;", con);
                    cmd.Parameters.AddWithValue("@SteamId", steamId);
                    bool exist = false;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            exist = true;
                        }
                    }
                    if (exist)
                    {
                        cmd = new MySqlCommand(@$"UPDATE `PlayerIsTop`
                                                  SET `KillCount` = @KillCount
                                                  WHERE `SteamId` = @SteamId;", con);

                        cmd.Parameters.AddWithValue("@SteamId", steamId);
                        cmd.Parameters.AddWithValue("@KillCount", data.GetDbValue());

                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        cmd = new MySqlCommand(@$"INSERT INTO `PlayerIsTop`
                                                  (SteamId,KillCount)
                                                  VALUES (@SteamId,@KillCount);", con);

                        cmd.Parameters.AddWithValue("@SteamId", steamId);
                        cmd.Parameters.AddWithValue("@KillCount", data.GetDbValue());

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private void GetAllIsTopData(MySqlConnection con)
    {
        try
        {
            if (con == null)
            {
                return;
            }
            MySqlCommand? cmd = new MySqlCommand(@$"SELECT `SteamId`, `KillCount` FROM `PlayerIsTop`", con);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var steamId = reader.IsDBNull(0) ? 0 : reader.GetInt64(0);
                    var killCount = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);

                    if (IsTopDatas.ContainsKey((ulong)steamId) == false)
                    {
                        IsTopDatas.Add((ulong)steamId, killCount);
                    }
                    else
                    {
                        IsTopDatas[(ulong)steamId] = killCount;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private void IsTopPlayerDeath(CsTeam? team, ulong? steamId)
    {
        if (steamId == 0)
        {
            return;
        }
        if (CurrentRoundWKillerId.HasValue)
        {
            return;
        }
        if (IsyanTeamPlayers.Contains(steamId.Value) == false)
        {
            return;
        }
        if (team != CsTeam.Terrorist)
        {
            return;
        }

        CurrentRoundWKillerId = steamId;
        if (IsTopDatas.TryGetValue(steamId.Value, out var killCount))
        {
            IsTopDatas[steamId.Value] = killCount + 1;
            UpdatePlayerIsTopData(steamId.Value);
        }
        else
        {
            IsTopDatas.Add(steamId.Value, 1);
            UpdatePlayerIsTopData(steamId.Value);
        }
    }

    private void IsTopWeeklyNotifyDc()
    {
        string url = Total_IsTop_DcWebHook;
        var msg = string.Empty;
        var datas = IsTopDatas.ToList().OrderByDescending(x => x.Value).Take(5);
        foreach (var item in datas)
        {
            try
            {
                var tempName = PlayerNamesDatas.TryGetValue((ulong)item.Key, out var name) != false
                              ? name : "-----";
                if (msg == string.Empty)
                {
                    msg = $"P.Name = {tempName} | S.Id = {item.Key} | H. İs.= {item.Value}";
                }
                else
                {
                    msg += $"\nP.Name = {tempName} | S.Id = {item.Key} | H. İs.= {item.Value}";
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
}