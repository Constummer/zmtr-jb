using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static bool WeaponIsValid(CBasePlayerWeapon? weapon) => weapon != null && weapon.IsValid != false;

    public static void RemoveWeapons(CCSPlayerController? x, bool knifeStays, string custom = null, int? setHp = null)
    {
        if (x is null || !x.IsValid || x.IsBot || x.IsHLTV)
            return;

        if (ValidateCallerPlayer(x, false) == false) return;
        if (knifeStays)
        {
            foreach (var weapon in x?.PlayerPawn?.Value?.WeaponServices?.MyWeapons!)
            {
                if (weapon is { IsValid: true, Value.IsValid: true })
                {
                    if (weapon.Value.DesignerName.Contains("bayonet") || weapon.Value.DesignerName.Contains("knife"))
                    {
                        continue;
                    }
                    Utilities.RemoveItemByDesignerName(x, weapon.Value.DesignerName);
                }
                x.ExecuteClientCommand("slot3");
            }
        }
        else
        {
            x.RemoveWeapons();
        }

        if (custom != null)
        {
            x.GiveNamedItem(custom);
        }
        if (setHp != null)
        {
            SetHp(x, setHp.Value);
            RefreshPawnTP(x);
        }
    }

    [Obsolete("2nd method on disarm")]
    private static void RemoveWeapons2ndMethod(CCSPlayerController x, bool knifeStays, string custom = null, int? setHp = null)
    {
        if (ValidateCallerPlayer(x, false) == false) return;
        x.RemoveWeapons();
        if (ValidateCallerPlayer(x, false) == false) return;
        if (knifeStays)
        {
            x.GiveNamedItem("weapon_knife");
        }
        if (custom != null)
        {
            x.GiveNamedItem(custom);
        }
        if (setHp != null)
        {
            SetHp(x, setHp.Value);
            RefreshPawnTP(x);
        }
    }

    private static List<ulong> RemoveAllWeapons(bool giveKnife, bool giveFists = false, string custom = null, int? setHp = null)
    {
        List<ulong> steamids = new List<ulong>();
        GetPlayers(CsTeam.Terrorist)
            .Where(x => x.PawnIsAlive)
            .ToList()
            .ForEach(x =>
            {
                if (ValidateCallerPlayer(x, false) == false) return;
                steamids.Add(x.SteamID);
                RemoveWeapons(x, giveKnife || giveFists, custom, setHp);
                //x.RemoveWeapons();
                //if (giveKnife)
                //{
                //    x.GiveNamedItem("weapon_knife");
                //}
                //if (giveFists)
                //{
                //    x.GiveNamedItem("weapon_knife");
                //}
                //if (custom != null)
                //{
                //    x.GiveNamedItem(custom);
                //}
                //if (setHp != null)
                //{
                //    SetHp(x, setHp.Value);
                //    RefreshPawnTP(x);
                //}
            });
        return steamids;
    }

    private static void RemoveAllWeaponsCT(bool giveKnife, bool giveFists = false, string custom = null, int? setHp = null)
    {
        GetPlayers(CsTeam.CounterTerrorist)
            .Where(x => x.PawnIsAlive)
            .ToList()
            .ForEach(x =>
            {
                if (ValidateCallerPlayer(x, false) == false) return;
                x.RemoveWeapons();
                if (giveKnife)
                {
                    x.GiveNamedItem("weapon_knife");
                }
                if (giveFists)
                {
                    x.GiveNamedItem("weapon_knife");
                }
                if (custom != null)
                {
                    x.GiveNamedItem(custom);
                }
                if (setHp != null)
                {
                    SetHp(x, setHp.Value);
                    RefreshPawnTP(x);
                }
            });
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
                        Utilities.RemoveItemByDesignerName(x, weapon.Value.DesignerName);
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

    private static void HideWeapons(CCSPlayerController x)
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

    private static void ShowWeapons(CCSPlayerController x)
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