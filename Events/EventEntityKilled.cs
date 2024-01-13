using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventEntityKilled()
    {
        RegisterEventHandler<EventEntityKilled>((@event, _) =>
        {
            ActiveTeamGamesGameBase?.EventEntityKilled(@event);
            return HookResult.Continue;
        });
    }
}