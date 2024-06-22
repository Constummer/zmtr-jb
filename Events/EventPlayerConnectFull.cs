using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Timers;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventPlayerConnectFull()
    {
        RegisterEventHandler<EventPlayerChat>((@event, info) =>
        {
            //Server.PrintToChatAll($"{@event.Teamonly}");
            //Server.PrintToChatAll($"{@event.Text}");
            //Server.PrintToChatAll($"{@event.Userid}");
            //var p = Utilities.GetPlayerFromUserid(@event.Userid);
            //if (p != null)
            //{
            //    if (ValidateCallerPlayer(p, false) == false)
            //    {
            //        return HookResult.Continue;
            //    }
            //    Server.PrintToChatAll(p.PlayerName);
            //    return HookResult.Continue;
            //}
            /*
             False
            tamam
            3
            smartest indian
            False
            tamam
            3
            smartest indian
            [KOMUTÇU] smartest indian : tamam
            False
            !gag @all
            1
            Constummer
            False
            !gag @all
            1
            Constummer
            *ÖLÜ* [Dev] Constummer : !gag @all
            */
            return HookResult.Continue;
        });
        RegisterEventHandler<EventPlayerConnectFull>((@event, _) =>
        {
            try
            {
                var tempSteamId = @event?.Userid?.SteamID;
                var tempPlayerName = @event?.Userid?.PlayerName;
                var tempUserId = @event?.Userid?.UserId;
                _ClientQueue.Enqueue(new(tempSteamId ?? 0, tempUserId, tempPlayerName, QueueItemType.OnClientConnect));
                if (Config.Additional.WelcomeActive && @event?.Userid != null && is_valid(@event?.Userid))
                {
                    WelcomeMsgSpam(@event?.Userid);
                }

                return HookResult.Continue;
            }
            catch (Exception e)
            {
                ConsMsg(e.Message);
                return HookResult.Continue;
            }
        });
        AddTimer(1f, () =>
        {
            if (checkqueue.TryDequeue(out var player))
            {
                if (BanCheck(player.Item1) == false)
                {
                    Server.ExecuteCommand($"kickid {player.Item3}");
                }

                AddOrUpdatePlayerToPlayerNameTable(player.Item1, player.Item2);
                GetPlayerMarketData(player.Item1).Wait();
                InsertAndGetTimeTrackingData(player.Item1);
                GetPGagData(player.Item1);
                InsertAndGetPlayerLevelData(player.Item1, true, player.Item2);
                CheckPlayerGroups(player.Item1);
            }
        }, TimerFlags.REPEAT);
    }
}