using CounterStrikeSharp.API;
using System.Diagnostics;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Queue<QueueItems> _ClientQueue = new Queue<QueueItems>();

    public class QueueItems
    {
        public QueueItems(ulong steamId, int? userId, string playerName, bool onConnect)
        {
            SteamId = steamId;
            UserId = userId;
            PlayerName = playerName;
            OnConnect = onConnect;
        }

        public bool OnConnect { get; set; }
        public ulong SteamId { get; set; }
        public int? UserId { get; set; }
        public string PlayerName { get; set; }
    }

    public class QueueConsumer
    {
        public static void StartConsumeOnConnect()
        {
            QueueItems item = null;
            try
            {
                if (_ClientQueue.TryDequeue(out item))
                {
                    if (item == null)
                    {
                        return;
                    }
                    if (item.SteamId == 0)
                    {
                        return;
                    }
                    var tempSteamId = item.SteamId;
                    var teampUserId = item.UserId;
                    var tempPlayerName = item.PlayerName;
                    if (item.OnConnect == true)
                    {
                        Server.PrintToConsole($"1.1 {item.SteamId}");
                        Server.PrintToConsole($"1.2 {item.PlayerName}");
                        Server.PrintToConsole($"1.3 {item.PlayerName}");
                        if (BanCheck(tempSteamId) == false)
                        {
                            Server.ExecuteCommand($"kickid {teampUserId} Banlısın, detaylı bilgi için Discord: discord.gg/wnxt3py");
                        }
                        else
                        {
                            var watch = Stopwatch.StartNew();
                            Server.PrintToConsole($"2 {item.PlayerName}");
                            AddOrUpdatePlayerToPlayerNameTable(tempSteamId, tempPlayerName);
                            Server.PrintToConsole($"3 {item.PlayerName}");
                            GetPlayerMarketData(tempSteamId);
                            Server.PrintToConsole($"4 {item.PlayerName}");
                            InsertAndGetTimeTrackingData(tempSteamId);
                            Server.PrintToConsole($"5 {item.PlayerName}");
                            GetPGagData(tempSteamId);
                            Server.PrintToConsole($"6 {item.PlayerName}");
                            InsertAndGetPlayerLevelData(tempSteamId, true, tempPlayerName);
                            Server.PrintToConsole($"7 {item.PlayerName}");
                            CheckPlayerGroups(tempSteamId);
                            Server.PrintToConsole($"8 {item.PlayerName}");
                            watch.Stop();
                            Server.PrintToConsole($"9 {watch.Elapsed.TotalMilliseconds}");
                        }
                    }
                    else
                    {
                        if (item == null)
                        {
                            return;
                        }
                        if (item.SteamId == 0)
                        {
                            return;
                        }
                        Server.PrintToConsole($"1.1 {item.SteamId}");
                        Server.PrintToConsole($"1.2 {item.PlayerName}");
                        Server.PrintToConsole($"1.3 {item.PlayerName}");
                        var watch = Stopwatch.StartNew();

                        Server.PrintToConsole($"3 {item.PlayerName}");

                        ClearOnDisconnect(tempSteamId, teampUserId);
                        Server.PrintToConsole($"4 {item.PlayerName}");
                        if (tempSteamId == LatestWCommandUser)
                        {
                            CoinRemove();
                        }
                        Server.PrintToConsole($"2 {item.PlayerName}");

                        watch.Stop();
                        Server.PrintToConsole($"9 {watch.Elapsed.TotalMilliseconds}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}