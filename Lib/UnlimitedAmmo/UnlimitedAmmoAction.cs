using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void UnlimitedAmmoV2(CEntityInstance entity)
    {
        return;
        if (!entity.DesignerName.Contains("weapon_")) return;
        foreach (var item in Weapons.Split("|"))
        {
            string[] weaponValues = item.Split(",");

            if (weaponValues.Length < 2) continue;

            Server.NextFrame(() =>
            {
                var weapon = new CBasePlayerWeapon(entity.Handle);

                if (!weapon.IsValid) return;

                weaponValues[0] = weaponValues[0].Trim();

                if (!CheckIfWeapon(weaponValues[0], weapon.AttributeManager.Item.ItemDefinitionIndex)) return;

                CCSWeaponBase _weapon = weapon.As<CCSWeaponBase>();
                if (_weapon == null) return;
                if (WeaponDefaults.ContainsKey(entity.DesignerName) == false)
                {
                    var wepDef = new WeaponDefault();
                    if (_weapon.VData != null)
                    {
                        wepDef.VData1MaxClip1 = _weapon.VData.MaxClip1;
                        wepDef.VData1DefaultClip1 = _weapon.VData.DefaultClip1;
                        wepDef.VData2PrimaryReserveAmmoMax = _weapon.VData.PrimaryReserveAmmoMax;
                    }
                    wepDef._1Clip1 = _weapon.Clip1;
                    wepDef._2ReserveAmmo0 = _weapon.ReserveAmmo[0];
                    WeaponDefaults[entity.DesignerName] = wepDef;
                }
                if (UnlimitedReserverAmmoActive)
                {
                    if (_weapon.VData != null)
                    {
                        _weapon.VData.MaxClip1 = 999;
                        _weapon.VData.DefaultClip1 = 999;
                    }

                    _weapon.Clip1 = 999;

                    Utilities.SetStateChanged(weapon.As<CCSWeaponBase>(), "CBasePlayerWeapon", "m_iClip1");
                }
                else
                {
                    if (WeaponDefaults.TryGetValue(entity.DesignerName, out var weaponDefault))
                    {
                        if (_weapon.VData != null)
                        {
                            _weapon.VData.MaxClip1 = weaponDefault.VData1MaxClip1;
                            _weapon.VData.DefaultClip1 = weaponDefault.VData1DefaultClip1;
                        }

                        _weapon.Clip1 = weaponDefault._1Clip1;

                        Utilities.SetStateChanged(weapon.As<CCSWeaponBase>(), "CBasePlayerWeapon", "m_iClip1");
                    }
                }

                if (weaponValues[2].Length > 0 && weaponValues[2] != "-1")
                {
                    if (_weapon.VData != null)
                    {
                        _weapon.VData.PrimaryReserveAmmoMax = int.Parse(weaponValues[2]);
                    }
                    _weapon.ReserveAmmo[0] = int.Parse(weaponValues[2]);

                    Utilities.SetStateChanged(weapon.As<CCSWeaponBase>(), "CBasePlayerWeapon", "m_pReserveAmmo");
                }
            });
        }
    }

    private static bool CheckIfWeapon(string weaponName, int weaponDefIndex)
    {
        if (WeaponDefindex.ContainsKey(weaponDefIndex) && WeaponDefindex[weaponDefIndex] == weaponName) return true;

        return false;
    }
}