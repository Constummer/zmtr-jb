using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventPlayerConnectFull()
    {
        //disabled, was causing crash
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
            _ClientQueue.Enqueue(new(tempSteamId ?? 0, tempUserId, tempPlayerName, true));

            return HookResult.Continue;
        });
    }
}