using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void LrWeaponReload(EventWeaponReload @event, GameEventInfo info)
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
            case LrChoices.Deagle:
                weapon = GetWeapon(@event.Userid, "weapon_deagle");
                if (WeaponIsValid(weapon) == false)
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