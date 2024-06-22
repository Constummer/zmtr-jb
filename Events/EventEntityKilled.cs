using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventEntityKilled()
    {
        RegisterEventHandler<EventEntityKilled>((@event, _) =>
        {
            try
            {
                ActiveTeamGamesGameBase?.EventEntityKilled(@event);
                return HookResult.Continue;
            }
            catch (Exception e)
            {
                ConsMsg(e.Message);
                return HookResult.Continue;
            }
        });
    }
}