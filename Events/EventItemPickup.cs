using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventItemPickup()
    {
        RegisterEventHandler<EventItemPickup>((@event, info) =>
        {
            ActiveTeamGamesGameBase?.EventItemPickup(@event);
            return HookResult.Continue;
        }, HookMode.Pre);
    }
}