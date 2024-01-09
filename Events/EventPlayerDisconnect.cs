using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventPlayerDisconnect()
    {
        //disabled, was causing crash
        //RegisterEventHandler<EventPlayerDisconnect>((@event, _) =>
        //{
        //    //if (@event?.Userid?.SteamID == null)
        //    //{
        //    //    return HookResult.Continue;
        //    //}
        //    //ClearOnDisconnect(@event.Userid.SteamID, @event?.Userid.UserId);
        //    //if (@event?.Userid?.SteamID == LatestWCommandUser)
        //    //{
        //    //    CoinRemove();
        //    //}
        //    return HookResult.Continue;
        //}, HookMode.Pre);
    }
}