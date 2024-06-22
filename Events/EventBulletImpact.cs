using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventBulletImpact()
    {
        RegisterEventHandler<EventBulletImpact>((@event, info) =>
        {
            try
            {
                if (@event == null)
                    return HookResult.Continue;

                DebugBulletImpact(@event, info);
                CitEkle(@event);
                DizAction(@event);
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