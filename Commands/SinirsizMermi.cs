using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static bool UnlimitedReserverAmmoActive = false;

    #region SinirsizMermi

    [ConsoleCommand("sinirsizmermiac")]
    [ConsoleCommand("smac")]
    public void SinirsizMermiAc(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        UnlimitedReserverAmmoActive = true;
    }

    [ConsoleCommand("sinirsizmermikapa")]
    [ConsoleCommand("sinirsizmermikapat")]
    [ConsoleCommand("smkapa")]
    [ConsoleCommand("smkapat")]
    public void SinirsizMermiKapa(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        UnlimitedReserverAmmoActive = false;

        SinirsizMolyTimer = GiveSinirsizCustomNade(0, SinirsizMolyTimer, "weapon_incgrenade");
        SinirsizBombaTimer = GiveSinirsizCustomNade(0, SinirsizBombaTimer, "weapon_hegrenade");

        //foreach (var item in weaponDefaults.ToList())
        //{
        //    var weapon = new CBasePlayerWeapon(item.Key);
        //    Server.NextFrame(() =>
        //    {
        //        try
        //        {
        //            if (!weapon.IsValid) return;

        //            CCSWeaponBaseVData? _weapon = weapon.As<CCSWeaponBase>().VData;
        //            if (_weapon == null) return;
        //            if (_weapon.GearSlot != gear_slot_t.GEAR_SLOT_KNIFE &&
        //                _weapon.GearSlot != gear_slot_t.GEAR_SLOT_GRENADES &&
        //                _weapon.GearSlot != gear_slot_t.GEAR_SLOT_INVALID &&
        //                _weapon.GearSlot != gear_slot_t.GEAR_SLOT_BOOSTS &&
        //                _weapon.GearSlot != gear_slot_t.GEAR_SLOT_UTILITY &&
        //                _weapon.GearSlot != gear_slot_t.GEAR_SLOT_C4)
        //            {
        //                _weapon.MaxClip1 = item.Value.MaxClip1;
        //                _weapon.MaxClip2 = item.Value.MaxClip2;
        //                _weapon.DefaultClip1 = item.Value.DefaultClip1;
        //                _weapon.DefaultClip2 = item.Value.DefaultClip2;
        //            }
        //        }
        //        catch (Exception) { }
        //    });
        //}
    }

    #endregion SinirsizMermi
}