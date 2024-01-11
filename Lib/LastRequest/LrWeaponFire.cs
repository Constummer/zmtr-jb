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

        switch (ActivatedLr.Choice)
        {
            case LrChoices.None:
                break;

            case LrChoices.Deagle:
                //Server.NextFrame(async () =>
                //{
                //CBasePlayerWeapon? weaponCurrent = GetWeapon(@event.Userid, @event.Weapon);

                //if (WeaponIsValid(weaponCurrent) == false)
                //{
                //    return;
                //}
                //SetAmmo(weaponCurrent, 0, 0);
                //});
                //RemoveWeapon(@event.Userid, "weapon_deagle");
                //gard.GiveNamedItem(item.WeaponName);

                //CBasePlayerWeapon? mahkumWeapon = GetWeapon(mahkum, item.WeaponName);
                //SetAmmo(mahkumWeapon, 1, 0);

                if (@event.Userid.SteamID == ActivatedLr.GardSteamId)
                {
                    //if (ActiveGodMode.TryGetValue(ActivatedLr.MahkumSteamId, out _))
                    //{
                    //    ActiveGodMode[ActivatedLr.MahkumSteamId] = true;
                    //}
                    //else
                    //{
                    //    ActiveGodMode.TryAdd(ActivatedLr.MahkumSteamId, true);
                    //}
                    //Server.PrintToChatAll($"{ActivatedLr.MahkumSteamId} - god ver");

                    //if (ActiveGodMode.ContainsKey(ActivatedLr.GardSteamId))
                    //{
                    //    Server.PrintToChatAll($"{ActivatedLr.GardSteamId} - goddan kaldir");
                    //    ActiveGodMode.Remove(ActivatedLr.GardSteamId);
                    //}
                    var mahk = GetPlayers().Where(x => x.PawnIsAlive && x.SteamID == ActivatedLr.MahkumSteamId).FirstOrDefault();
                    if (mahk == null)
                    {
                        return;
                    }
                    var weapon = GetWeapon(mahk, @event.Weapon);
                    if (WeaponIsValid(weapon) == false)
                    {
                        return;
                    }
                    SetAmmo(weapon, 1, 0);
                }
                else if (@event.Userid.SteamID == ActivatedLr.MahkumSteamId)
                {
                    //if (ActiveGodMode.TryGetValue(ActivatedLr.GardSteamId, out _))
                    //{
                    //    ActiveGodMode[ActivatedLr.GardSteamId] = true;
                    //}
                    //else
                    //{
                    //    ActiveGodMode.TryAdd(ActivatedLr.GardSteamId, true);
                    //}
                    //Server.PrintToChatAll($"{ActivatedLr.GardSteamId} - god ver");
                    //if (ActiveGodMode.ContainsKey(ActivatedLr.MahkumSteamId))
                    //{
                    //    Server.PrintToChatAll($"{ActivatedLr.MahkumSteamId} - goddan kaldir");
                    //    ActiveGodMode.Remove(ActivatedLr.MahkumSteamId);
                    //}
                    var gard = GetPlayers().Where(x => x.PawnIsAlive && x.SteamID == ActivatedLr.GardSteamId).FirstOrDefault();
                    if (gard == null)
                    {
                        return;
                    }
                    var weapon = GetWeapon(gard, @event.Weapon);
                    if (WeaponIsValid(weapon) == false)
                    {
                        return;
                    }
                    SetAmmo(weapon, 1, 0);
                }
                break;

            case LrChoices.NoScopeScout:
                break;

            case LrChoices.NoScopeAwp:
                break;

            default:
                break;
        }
    }

    public static void SetAmmo(CBasePlayerWeapon? weapon, int clip, int reserve)
    {
        if (WeaponIsValid(weapon) == false)
        {
            return;
        }

        weapon.Clip1 = clip;
        Utilities.SetStateChanged(weapon, "CBasePlayerWeapon", "m_iClip1");
        weapon.ReserveAmmo[0] = reserve;
        Utilities.SetStateChanged(weapon, "CBasePlayerWeapon", "m_pReserveAmmo");
    }
}