using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventWeaponFire()
    {
        RegisterEventHandler<EventWeaponFire>(UnlimitedReserverAmmo);
    }

    private static HookResult UnlimitedReserverAmmo(EventWeaponFire @event, GameEventInfo info)
    {
        if (@event.Weapon?.Contains("knife") ?? true)
        {
            return HookResult.Continue;
        }
        if (ValidateCallerPlayer(@event?.Userid, false) == false)
        {
            return HookResult.Continue;
        }
        var player = @event!.Userid;
        if (GetTeam(player) != CsTeam.CounterTerrorist)
        {
            return HookResult.Continue;
        }
        var weaponServices = player.PlayerPawn.Value!.WeaponServices;
        if (weaponServices == null || weaponServices.MyWeapons == null)
        {
            return HookResult.Continue;
        }
        foreach (var weapon in weaponServices.MyWeapons)
        {
            if (weapon != null && weapon.IsValid && weapon.Value!.DesignerName == @event.Weapon)
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