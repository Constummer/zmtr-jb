using CounterStrikeSharp.API;
using System.Diagnostics;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Queue<QueueItems> _ClientQueue = new Queue<QueueItems>();

    public enum QueueItemType
    {
        None = 0,
        OnClientConnect,
        OnClientDisconnect,
        OnWChange
    }

    public class QueueItems
    {
        public QueueItems(ulong steamId, int? userId, string playerName, QueueItemType type)
        {
            SteamId = steamId;
            UserId = userId;
            PlayerName = playerName;
            Type = type;
        }

        public QueueItemType Type { get; set; }
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
                    if (item.Type == QueueItemType.None)
                    {
                        return;
                    }
                    var tempSteamId = item.SteamId;
                    var teampUserId = item.UserId;
                    var tempPlayerName = item.PlayerName;
                    var watch = Stopwatch.StartNew();

                    switch (item.Type)
                    {
                        case QueueItemType.OnClientConnect:
                            Server.PrintToConsole($"1.1 {item.SteamId}");
                            Server.PrintToConsole($"1.2 {item.PlayerName}");
                            Server.PrintToConsole($"1.3 {item.PlayerName}");
                            if (BanCheck(tempSteamId) == false)
                            {
                                Server.ExecuteCommand($"kickid {teampUserId} Banlısın, detaylı bilgi için Discord: discord.gg/wnxt3py");
                            }
                            else
                            {
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
                            break;

                        case QueueItemType.OnClientDisconnect:

                            Server.PrintToConsole($"1.1 {item.SteamId}");
                            Server.PrintToConsole($"1.2 {item.PlayerName}");
                            Server.PrintToConsole($"1.3 {item.PlayerName}");

                            Server.PrintToConsole($"2 {item.PlayerName}");

                            ClearOnDisconnect(tempSteamId, teampUserId);
                            Server.PrintToConsole($"3 {item.PlayerName}");
                            if (tempSteamId == LatestWCommandUser)
                            {
                                CoinRemove();
                            }
                            Server.PrintToConsole($"4 {item.PlayerName}");

                            watch.Stop();
                            Server.PrintToConsole($"5 {watch.Elapsed.TotalMilliseconds}");
                            break;

                        case QueueItemType.OnWChange:
                            DiscordPost("https://discord.com/api/webhooks/1194758709344215090/-XRiPj35x-KTHRtAyWlB5i1I16lFylHl_17we6SOS5HbYY5JCFPQYiOjYot6trvQiUcR", "w deisti XD");
                            break;

                        default:
                            break;
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