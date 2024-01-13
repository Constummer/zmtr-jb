using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventBombPickup()
    {
        RegisterEventHandler<EventBombPickup>((@event, _) =>
        {
            ActiveTeamGamesGameBase?.EventBombPickup(@event);
            return HookResult.Continue;
        });
    }
}