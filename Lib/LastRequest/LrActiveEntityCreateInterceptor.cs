using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void LrActiveEntityCreateInterceptor(CBasePlayerWeapon weapon, CCSWeaponBase _weapon)
    {
        if (ActivatedLr == null)
        {
            return;
        }
        return;
        if (weapon.Index == ActivatedLr.GardWeaponIndex)
        {
            if (_weapon.VData != null)
            {
                _weapon.VData.MaxClip1 = 0;
                _weapon.VData.DefaultClip1 = 0;
            }

            _weapon.Clip1 = 0;

            Utilities.SetStateChanged(weapon.As<CCSWeaponBase>(), "CBasePlayerWeapon", "m_iClip1");

            if (_weapon.VData != null)
            {
                _weapon.VData.PrimaryReserveAmmoMax = 0;
            }
            _weapon.ReserveAmmo[0] = 0;

            Utilities.SetStateChanged(weapon.As<CCSWeaponBase>(), "CBasePlayerWeapon", "m_pReserveAmmo");
        }
        else
        {
            if (_weapon.VData != null)
            {
                _weapon.VData.MaxClip1 = 1;
                _weapon.VData.DefaultClip1 = 1;
            }

            _weapon.Clip1 = 1;

            Utilities.SetStateChanged(weapon.As<CCSWeaponBase>(), "CBasePlayerWeapon", "m_iClip1");

            if (_weapon.VData != null)
            {
                _weapon.VData.PrimaryReserveAmmoMax = 0;
            }
            _weapon.ReserveAmmo[0] = 0;

            Utilities.SetStateChanged(weapon.As<CCSWeaponBase>(), "CBasePlayerWeapon", "m_pReserveAmmo");
        }
    }
}