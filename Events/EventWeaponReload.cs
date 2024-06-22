using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventWeaponReload()
    {
        RegisterEventHandler<EventWeaponReload>((@event, info) =>
        {
            try
            {
                if (@event == null)
                    return HookResult.Continue;

                LrWeaponReload(@event, info);

                return HookResult.Continue;
            }
            catch (Exception e)
            {
                ConsMsg(e.Message);
                return HookResult.Continue;
            }
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
                if (weapon.Value == null ||
                  string.IsNullOrWhiteSpace(weapon.Value.DesignerName) != false ||
                  weapon.Value.DesignerName == "[null]")
                {
                    continue;
                }
                try
                {
                    if (WeaponMenuHelper.ValidWeaponChecker(weapon.Value.DesignerName) == false)
                    {
                        continue;
                    }
                    var clip1 = weapon.Value.Clip1;
                    var reservedAmmo = weapon.Value.ReserveAmmo[0];
                    Server.NextFrame(() =>
                    {
                        weapon.Value.ReserveAmmo[0] = reservedAmmo + clip1;
                    });
                }
                catch (Exception e)
                {
                    ConsMsg(e.Message);
                }
            }
        }
        return HookResult.Continue;
    }
}