using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Queue<QueueItems> _ClientQueue = new Queue<QueueItems>();

    public enum QueueItemType
    {
        None = 0,
        OnClientConnect,
        OnClientDisconnect,
        OnWChange,
        OnPlayerSpawn
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
                    var tempUserId = item.UserId;
                    var tempPlayerName = item.PlayerName;

                    switch (item.Type)
                    {
                        case QueueItemType.OnClientConnect:

                            if (BanCheck(tempSteamId) == false)
                            {
                                Server.ExecuteCommand($"kickid {tempUserId}");
                            }
                            else
                            {
                                AddOrUpdatePlayerToPlayerNameTable(tempSteamId, tempPlayerName);

                                GetPlayerMarketData(tempSteamId);

                                InsertAndGetTimeTrackingData(tempSteamId);

                                GetPGagData(tempSteamId);

                                InsertAndGetPlayerLevelData(tempSteamId, true, tempPlayerName);

                                CheckPlayerGroups(tempSteamId);
                            }
                            break;

                        case QueueItemType.OnClientDisconnect:

                            ActiveTeamGamesGameBase?.EventPlayerDisconnect(tempSteamId);

                            ClearOnDisconnect(tempSteamId, tempUserId);

                            if (tempSteamId == LatestWCommandUser)
                            {
                                CoinRemove();
                            }

                            break;

                        case QueueItemType.OnWChange:
                            DiscordPost("https://discord.com/api/webhooks/1194758709344215090/-XRiPj35x-KTHRtAyWlB5i1I16lFylHl_17we6SOS5HbYY5JCFPQYiOjYot6trvQiUcR", "w deisti XD");
                            break;

                        case QueueItemType.OnPlayerSpawn:
                            CCSPlayerController? x = null;
                            if (tempUserId.HasValue)
                            {
                                x = Utilities.GetPlayerFromUserid(tempUserId.Value);
                            }
                            else if (tempSteamId > 0)
                            {
                                x = Utilities.GetPlayerFromSteamId(tempSteamId);
                            }
                            else
                            {
                                return;
                            }
                            if (ValidateCallerPlayer(x, false) == false) return;
                            if (x.PawnIsAlive && get_health(x) > 0)
                            {
                                if (LatestWCommandUser == x.SteamID)
                                {
                                    if (ValidateCallerPlayer(x, false) == false) return;
                                    SetColour(x, Color.FromArgb(255, 0, 0, 255));
                                }
                                else
                                {
                                    if (ValidateCallerPlayer(x, false) == false) return;
                                    SetColour(x, DefaultColor);
                                }
                            }

                            CheckPlayerIsTeamTag(tempSteamId);
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