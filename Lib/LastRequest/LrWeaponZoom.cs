using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void LrWeaponZoom(EventWeaponZoom @event)
    {
        if (LrActive == false)
        {
            return;
        }
        if (ActivatedLr == null)
        {
            return;
        }
        if (ActivatedLr.ScopeDisable == false)
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
        CBasePlayerWeapon? weapon = null;
        switch (ActivatedLr.Choice)
        {
            case LrChoices.NoScopeScout:
                RemoveWeapon(@event.Userid, "weapon_ssg08");
                @event.Userid.GiveNamedItem("weapon_ssg08");
                weapon = GetWeapon(@event.Userid, "weapon_ssg08");
                if (LRWeaponIsValid(weapon) == false)
                {
                    return;
                }
                SetAmmo(weapon, 1, 0);
                break;

            case LrChoices.NoScopeAwp:
                RemoveWeapon(@event.Userid, "weapon_awp");
                @event.Userid.GiveNamedItem("weapon_awp");
                weapon = GetWeapon(@event.Userid, "weapon_awp");
                if (LRWeaponIsValid(weapon) == false)
                {
                    return;
                }
                SetAmmo(weapon, 1, 0);
                break;

            default:
                break;
        }
    }
}