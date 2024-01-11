using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void LrWeaponFire(EventWeaponFire @event)
    {
        if (LrActive == false)
        {
            return;
        }
        if (ActivatedLr == null)
        {
            return;
        }
        if (@event?.Userid == null)
        {
            return;
        }
        if (@event?.Userid?.SteamID == null || @event?.Userid?.SteamID <= 0)
        {
            return;
        }
        if (ActivatedLr?.GardSteamId == null || ActivatedLr.GardSteamId <= 0)
        {
            return;
        }
        if (ActivatedLr?.MahkumSteamId == null || ActivatedLr.MahkumSteamId <= 0)
        {
            return;
        }
        if (@event?.Userid?.SteamID != ActivatedLr.GardSteamId && @event?.Userid?.SteamID != ActivatedLr.MahkumSteamId)
        {
            return;
        }
        CBasePlayerWeapon? weaponCurrent = GetWeapon(@event.Userid, @event.Weapon);

        if (LRWeaponIsValid(weaponCurrent) == false)
        {
            return;
        }
        SetAmmo(weaponCurrent, 0, 0);
        if (@event.Userid.SteamID == ActivatedLr.GardSteamId)
        {
            var mahk = GetPlayers().Where(x => x.PawnIsAlive && x.SteamID == ActivatedLr.MahkumSteamId).FirstOrDefault();
            if (mahk == null)
            {
                return;
            }
            var weapon = GetWeapon(mahk, @event.Weapon);
            if (LRWeaponIsValid(weapon) == false)
            {
                return;
            }
            SetAmmo(weapon, 1, 0);
        }
        else if (@event.Userid.SteamID == ActivatedLr.MahkumSteamId)
        {
            var gard = GetPlayers().Where(x => x.PawnIsAlive && x.SteamID == ActivatedLr.GardSteamId).FirstOrDefault();
            if (gard == null)
            {
                return;
            }
            var weapon = GetWeapon(gard, @event.Weapon);
            if (LRWeaponIsValid(weapon) == false)
            {
                return;
            }
            SetAmmo(weapon, 1, 0);
        }
    }

    public static void SetAmmo(CBasePlayerWeapon? weapon, int clip, int reserve)
    {
        if (LRWeaponIsValid(weapon) == false)
        {
            return;
        }

        weapon.Clip1 = clip;
        Utilities.SetStateChanged(weapon, "CBasePlayerWeapon", "m_iClip1");
        weapon.ReserveAmmo[0] = reserve;
        Utilities.SetStateChanged(weapon, "CBasePlayerWeapon", "m_pReserveAmmo");
    }
}