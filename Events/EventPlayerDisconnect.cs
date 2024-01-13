using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventPlayerDisconnect()
    {
        //disabled, was causing crash
        RegisterEventHandler<EventPlayerDisconnect>((@event, _) =>
        {
            //if (@event == null) return HookResult.Continue;
            //if (@event.Userid == null) return HookResult.Continue;
            //if (@event.Userid.IsValid == false) return HookResult.Continue;
            //if (@event.Userid.UserId < 0) return HookResult.Continue;
            //if (@event.Userid.SteamID < 0) return HookResult.Continue;
            var tempSteamId = @event?.Userid?.SteamID;
            var tempUserId = @event?.Userid?.UserId;
            _ClientQueue.Enqueue(new(tempSteamId ?? 0, tempUserId, "", false));
            return HookResult.Continue;
        }, HookMode.Pre);
    }
}