using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventCsWinPanelRound()
    {
        RegisterEventHandler<EventCsWinPanelRound>((@event, info) =>
        {
            if (@event == null) return HookResult.Continue;
            info.DontBroadcast = true;

            return HookResult.Handled;
        }, HookMode.Pre);
    }
}