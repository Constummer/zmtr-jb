using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventRoundStart()
    {
        RegisterEventHandler<EventRoundStart>((@event, _) =>
        {
            ClearAll();
            return HookResult.Continue;
        });
    }
}