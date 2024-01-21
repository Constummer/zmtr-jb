using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventPlayerDisconnect()
    {
        RegisterEventHandler<EventPlayerDisconnect>((@event, _) =>
        {
            var tempSteamId = @event?.Userid?.SteamID;
            var tempUserId = @event?.Userid?.UserId;
            _ClientQueue.Enqueue(new(tempSteamId ?? 0, tempUserId, "", QueueItemType.OnClientDisconnect));
            return HookResult.Continue;
        }, HookMode.Pre);
    }
}