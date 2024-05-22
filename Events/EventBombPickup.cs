using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventBombPickup()
    {
        RegisterEventHandler<EventBombPickup>((@event, _) =>
        {
            try
            {
                ActiveTeamGamesGameBase?.EventBombPickup(@event);
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