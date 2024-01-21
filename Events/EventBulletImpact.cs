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

            DebugBulletImpact(@event, info);
            CitEkle(@event);
            DizAction(@event);
            return HookResult.Continue;
        });
    }
}