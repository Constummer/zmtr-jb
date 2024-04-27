using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventBombDropped()
    {
        RegisterEventHandler<EventBombDropped>((@event, _) =>
           {
               ActiveTeamGamesGameBase?.EventBombDropped(@event);
               return HookResult.Continue;
           });
    }
}