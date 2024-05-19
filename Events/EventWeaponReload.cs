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

            LrWeaponReload(@event, info);

            return HookResult.Continue;

            //    if (LrActive == false)
            //    {
            //        UnlimitedReserverAmmo(@event, info);
            //    }
            //    return HookResult.Continue;
            //});

            //RegisterListener<Listeners.OnEntityCreated>(entity =>
            //{
            //    if (UnlimitedReserverAmmoActive == false)
            //    {
            //        return;
            //    }
            //    else
            //    {
            //        if (entity == null || entity.Entity == null || !entity.IsValid || !entity.DesignerName.Contains("weapon_")) return;
            //        var weapon = new CBasePlayerWeapon(entity.Handle);
            //        Server.NextFrame(() =>
            //        {
            //            try
            //            {
            //                if (UnlimitedReserverAmmoActive == false)
            //                {
            //                    return;
            //                }
            //                else
            //                {
            //                    if (!weapon.IsValid) return;

            //                    CCSWeaponBaseVData? _weapon = weapon.As<CCSWeaponBase>().VData;
            //                    if (_weapon == null) return;
            //                    if (_weapon.GearSlot != gear_slot_t.GEAR_SLOT_KNIFE &&
            //                        _weapon.GearSlot != gear_slot_t.GEAR_SLOT_GRENADES &&
            //                        _weapon.GearSlot != gear_slot_t.GEAR_SLOT_INVALID &&
            //                        _weapon.GearSlot != gear_slot_t.GEAR_SLOT_BOOSTS &&
            //                        _weapon.GearSlot != gear_slot_t.GEAR_SLOT_UTILITY &&
            //                        _weapon.GearSlot != gear_slot_t.GEAR_SLOT_C4)
            //                    {
            //                        weaponDefaults.Add(entity.Handle, new(
            //                            _weapon.MaxClip1,
            //                            _weapon.MaxClip2,
            //                            _weapon.DefaultClip1,
            //                            _weapon.DefaultClip2)
            //                        {
            //                        });
            //                        _weapon.MaxClip1 = 999;
            //                        _weapon.MaxClip2 = 999;
            //                        _weapon.DefaultClip1 = 999;
            //                        _weapon.DefaultClip2 = 999;
            //                    }
            //                }
            //            }
            //            catch (Exception) { }
            //        });
            //    }
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