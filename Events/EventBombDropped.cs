using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventBombDropped()
    {
        RegisterEventHandler<EventBombDropped>((@event, _) =>
           {
               try
               {
                   ActiveTeamGamesGameBase?.EventBombDropped(@event);
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