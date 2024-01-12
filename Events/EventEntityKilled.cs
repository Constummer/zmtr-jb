using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventEntityKilled()
    {
        RegisterEventHandler<EventEntityKilled>((@event, _) =>
        {
            return HookResult.Continue;
        });
    }
}