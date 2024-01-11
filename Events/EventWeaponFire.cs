using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventWeaponFire()
    {
        RegisterEventHandler<EventWeaponFire>((@event, _) =>
        {
            LrWeaponFire(@event);
            return HookResult.Continue;
        });
    }
}