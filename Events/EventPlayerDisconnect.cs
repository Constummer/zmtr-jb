using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventPlayerDisconnect()
    {
        RegisterEventHandler<EventPlayerDisconnect>((@event, _) =>
        {
            if (@event == null)
                return HookResult.Continue;
            if (@event?.Userid?.SteamID == LatestWCommandUser)
            {
                CoinRemove();
            }
            return HookResult.Continue;
        });
    }
}