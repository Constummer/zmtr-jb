using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventWeaponZoom()
    {
        RegisterEventHandler<EventWeaponZoom>((@event, _) =>
        {
            LrWeaponZoom(@event);
            return HookResult.Continue;
        });
    }
}