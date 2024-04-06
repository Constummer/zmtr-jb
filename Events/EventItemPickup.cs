using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using CounterStrikeSharp.API.Modules.Memory;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventItemPickup()
    {
        //VirtualFunctions.CCSPlayer_WeaponServices_CanUseFunc.Hook((DynamicHook hook) =>
        //{
        //    CBasePlayerWeapon clientweapon = hook.GetParam<CBasePlayerWeapon>(1);
        //    Server.PrintToChatAll(clientweapon.DesignerName);
        //    //if (clientweapon.DesignerName == "weapon_awp")
        //    //{
        //    //    hook.SetReturn(false);
        //    //    return HookResult.Handled;
        //    //}

        //    return HookResult.Continue;
        //},
        //HookMode.Pre);
        RegisterEventHandler<EventItemPickup>((@event, info) =>
        {
            if (ActiveTeamGamesGameBase != null)
            {
                ActiveTeamGamesGameBase?.EventItemPickup(@event);
            }
            else
            {
                if (@event == null) return HookResult.Continue;
                if (ValidateCallerPlayer(@event.Userid, false) == false) return HookResult.Continue;
                if (SutolCommandCalls.Contains(@event?.Userid?.SteamID ?? 0))
                {
                    @event.Userid.RemoveWeapons();
                }
            }
            return HookResult.Continue;
        }, HookMode.Post);
    }
}