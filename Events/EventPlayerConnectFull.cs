using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

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
            //if (@event == null) return HookResult.Continue;
            //if (@event.Userid == null) return HookResult.Continue;
            //if (@event.Userid.IsValid == false) return HookResult.Continue;
            //if (@event.Userid.SteamID == 0) return HookResult.Continue;
            //if (ValidateCallerPlayer(@event.Userid, false) == false) return HookResult.Continue;

            var tempSteamId = @event?.Userid?.SteamID;
            var tempPlayerName = @event?.Userid?.PlayerName;
            var tempUserId = @event?.Userid?.UserId;
            _ClientQueue.Enqueue(new(tempSteamId ?? 0, tempUserId, tempPlayerName, QueueItemType.OnClientConnect));

            return HookResult.Continue;
        });
    }
}