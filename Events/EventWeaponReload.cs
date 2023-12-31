using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventWeaponReload()
    {
        RegisterEventHandler<EventWeaponReload>((@event, info) =>
        {
            if (@event == null)
                return HookResult.Continue;
            if (LrActive == false)
            {
                UnlimitedReserverAmmo(@event, info);
            }
            return HookResult.Continue;
        });
    }

    private static HookResult UnlimitedReserverAmmo(EventWeaponReload @event, GameEventInfo info)
    {
        if (ValidateCallerPlayer(@event?.Userid, false) == false)
        {
            return HookResult.Continue;
        }
        var player = @event!.Userid;

        var weaponServices = player.PlayerPawn.Value!.WeaponServices;
        if (weaponServices == null || weaponServices.MyWeapons == null)
        {
            return HookResult.Continue;
        }
        foreach (var weapon in weaponServices.MyWeapons)
        {
            if (weapon != null && weapon.IsValid)
            {
                var clip1 = weapon.Value.Clip1;
                var reservedAmmo = weapon.Value.ReserveAmmo[0];

                Server.NextFrame(() =>
                {
                    if (weapon.Value == null) return;
                    try
                    {
                        //weapon.Value.Clip1 = clip1;
                        weapon.Value.ReserveAmmo[0] = reservedAmmo + clip1;
                    }
                    catch (Exception)
                    { }
                });
                break;
            }
        }
        return HookResult.Continue;
    }
}