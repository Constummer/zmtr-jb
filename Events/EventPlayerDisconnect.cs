using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventPlayerDisconnect()
    {
        //disabled, was causing crash
        RegisterEventHandler<EventPlayerDisconnect>((@event, _) =>
        {
            if (@event == null)
                return HookResult.Continue;
            if (@event?.Userid?.AuthorizedSteamID?.SteamId64 != @event?.Userid?.SteamID)
            {
                return HookResult.Continue;
            }
            if (@event?.Userid?.AuthorizedSteamID?.SteamId64 == null)
            {
                return HookResult.Continue;
            }

            if (ValidateCallerPlayer(@event.Userid, false) == false)
            {
                return HookResult.Continue;
            }
            ClearOnDisconnect(@event.Userid.SteamID, @event.Userid.UserId);
            if (@event?.Userid?.SteamID == LatestWCommandUser)
            {
                CoinRemove();
            }
            return HookResult.Continue;
        });
    }
}