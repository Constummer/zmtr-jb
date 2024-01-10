using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventBulletImpact()
    {
        RegisterEventHandler<EventBulletImpact>((@event, info) =>
        {
            if (@event == null)
                return HookResult.Continue;
            CitEkle(@event);
            DizAction(@event);
            return HookResult.Continue;
        });
    }
}