using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventCsWinPanelRound()
    {
        RegisterEventHandler<EventCsWinPanelRound>((@event, info) =>
        {
            try
            {
                if (@event == null) return HookResult.Continue;
                info.DontBroadcast = true;

                return HookResult.Handled;
            }
            catch (Exception e)
            {
                ConsMsg(e.Message);
                return HookResult.Continue;
            }
        }, HookMode.Pre);
    }
}