using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static bool WeaponIsValid(CBasePlayerWeapon? weapon) => weapon != null && weapon.IsValid != false;

    private static void RemoveWeapons(CCSPlayerController x, bool knifeStays)
    {
        if (x?.PlayerPawn?.Value?.WeaponServices?.MyWeapons != null)
        {
            foreach (var weapon in x.PlayerPawn.Value.WeaponServices!.MyWeapons)
            {
                if (weapon.Value != null
                    && string.IsNullOrWhiteSpace(weapon.Value.DesignerName) == false
                    && weapon.Value.DesignerName != "[null]")
                {
                    if (knifeStays == true)
                    {
                        if (weapon.Value.DesignerName.Contains("knife") == false)
                        {
                            weapon.Value.Remove();
                        }
                    }
                    else
                    {
                        weapon.Value.Remove();
                    }
                }
            }
        }
    }

    private static void RemoveWeapon(CCSPlayerController x, string weaponName)
    {
        if (x?.PlayerPawn?.Value?.WeaponServices?.MyWeapons != null)
        {
            foreach (var weapon in x.PlayerPawn.Value.WeaponServices!.MyWeapons)
            {
                if (weapon.Value != null
                    && string.IsNullOrWhiteSpace(weapon.Value.DesignerName) == false
                    && weapon.Value.DesignerName != "[null]")
                {
                    if (weapon.Value.DesignerName == weaponName)
                    {
                        weapon.Value.Remove();
                    }
                }
            }
        }
    }

    private static CBasePlayerWeapon GetWeapon(CCSPlayerController x, string weaponName)
    {
        if (x?.PlayerPawn?.Value?.WeaponServices?.MyWeapons != null)
        {
            foreach (var weapon in x.PlayerPawn.Value.WeaponServices!.MyWeapons)
            {
                if (weapon.Value != null
                    && string.IsNullOrWhiteSpace(weapon.Value.DesignerName) == false
                    && weapon.Value.DesignerName != "[null]")
                {
                    if (weapon.Value.DesignerName == weaponName)
                    {
                        return weapon.Value;
                    }
                }
            }
        }
        return null;
    }

    private void HideWeapons(CCSPlayerController x)
    {
        if (x?.PlayerPawn?.Value?.WeaponServices?.MyWeapons != null)
        {
            foreach (var weapon in x.PlayerPawn.Value.WeaponServices!.MyWeapons)
            {
                if (weapon.Value != null
                    && string.IsNullOrWhiteSpace(weapon.Value.DesignerName) == false
                    && weapon.Value.DesignerName != "[null]")
                {
                    if (WeaponIsValid(weapon.Value))
                    {
                        weapon.Value.RenderMode = RenderMode_t.kRenderTransColor;
                        weapon.Value.Render = Color.FromArgb(0, 0, 0, 0);
                        Utilities.SetStateChanged(weapon.Value, "CBaseModelEntity", "m_clrRender");
                        Utilities.SetStateChanged(weapon.Value, "CBaseModelEntity", "m_nRenderMode");
                    }
                }
            }
        }
    }

    private void ShowWeapons(CCSPlayerController x)
    {
        if (x?.PlayerPawn?.Value?.WeaponServices?.MyWeapons != null)
        {
            foreach (var weapon in x.PlayerPawn.Value.WeaponServices!.MyWeapons)
            {
                if (weapon.Value != null
                    && string.IsNullOrWhiteSpace(weapon.Value.DesignerName) == false
                    && weapon.Value.DesignerName != "[null]")
                {
                    if (WeaponIsValid(weapon.Value))
                    {
                        weapon.Value.RenderMode = RenderMode_t.kRenderTransColor;
                        weapon.Value.Render = DefaultColor;
                        Utilities.SetStateChanged(weapon.Value, "CBaseModelEntity", "m_clrRender");
                        Utilities.SetStateChanged(weapon.Value, "CBaseModelEntity", "m_nRenderMode");
                    }
                }
            }
        }
    }

    private static List<uint> PlayerWeaponIndexes(CCSPlayerController x)
    {
        var res = new List<uint>();
        if (x?.PlayerPawn?.Value?.WeaponServices?.MyWeapons != null)
        {
            foreach (var weapon in x.PlayerPawn.Value.WeaponServices!.MyWeapons)
            {
                if (weapon.Value != null
                    && string.IsNullOrWhiteSpace(weapon.Value.DesignerName) == false
                    && weapon.Value.DesignerName != "[null]")
                {
                    res.Add(weapon.Index);
                }
            }
        }
        return res;
    }
}