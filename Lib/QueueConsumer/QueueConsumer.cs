using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;
using System.Drawing;
using System.Numerics;

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
        OnPlayerSpawn,
        OnRefreshPawn
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

                            #region OnClientConnect

                            if (Utilities.GetPlayers().Count() >= 45)
                            {
                                Server.ExecuteCommand($"cs2f_fix_disconnects 1");
                            }

                            if (BanCheck(tempSteamId) == false)
                            {
                                Server.ExecuteCommand($"kickid {tempUserId}");
                            }
                            else
                            {
                                Server.PrintToChatAll($"{Prefix} {CC.Or}{tempPlayerName}{CC.W} Sunucuya giriş yaptı.");
                                AddOrUpdatePlayerToPlayerNameTable(tempSteamId, tempPlayerName);

                                GetPlayerMarketData(tempSteamId);

                                InsertAndGetTimeTrackingData(tempSteamId);

                                GetPGagData(tempSteamId);

                                InsertAndGetPlayerLevelData(tempSteamId, true, tempPlayerName);

                                GetPlayerParticleData(tempSteamId);

                                GetPlayerParachuteData(tempSteamId);

                                GetPlayerBattlePassData(tempSteamId);

                                GetPlayerBattlePassPremiumData(tempSteamId);

                                GetPlayerTimeRewardData(tempSteamId);

                                CheckPlayerGroups(tempSteamId);
                            }

                            #endregion OnClientConnect

                            break;

                        case QueueItemType.OnClientDisconnect:

                            #region OnClientDisconnect

                            if (Utilities.GetPlayers().Count() < 45)
                            {
                                Server.ExecuteCommand($"cs2f_fix_disconnects 0");
                            }
                            ActiveTeamGamesGameBase?.EventPlayerDisconnect(tempSteamId);

                            ClearOnDisconnect(tempSteamId, tempUserId);

                            if (tempSteamId == LatestWCommandUser)
                            {
                                CoinRemove();
                            }

                            #endregion OnClientDisconnect

                            break;

                        case QueueItemType.OnWChange:
                            SendDcNotifyOnWardenChange();
                            break;

                        case QueueItemType.OnRefreshPawn:
                            //var player = Utilities.GetPlayerFromSteamId(tempSteamId);
                            //if (ValidateCallerPlayer(player, false))
                            //{
                            //    var weaponServices = player.PlayerPawn.Value!.WeaponServices;
                            //    if (weaponServices == null) return;
                            //    if (weaponServices.MyWeapons != null)
                            //    {
                            //        foreach (var weapon in weaponServices.MyWeapons)
                            //        {
                            //            if (weapon != null && weapon.IsValid && weapon.Value != null && weapon.Value!.DesignerName == "weapon_healthshot")
                            //            {
                            //                weapon.Value.Remove();
                            //                break;
                            //            }
                            //        }
                            //    }
                            //}
                            break;

                        case QueueItemType.OnPlayerSpawn:

                            #region OnPlayerSpawn

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
                            if (CheckPlayerIsTeamTag(tempSteamId) == false)
                            {
                                CheckPlayerSutTeamTag(tempSteamId);
                            }

                            #endregion OnPlayerSpawn

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