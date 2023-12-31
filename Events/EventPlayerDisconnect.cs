using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventPlayerDisconnect()
    {
        return;
        //disabled, was causing crash
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