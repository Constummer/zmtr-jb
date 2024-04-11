using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public enum AcquireResult : int
    {
        Allowed = 0,
        InvalidItem,
        AlreadyOwned,
        AlreadyPurchased,
        ReachedGrenadeTypeLimit,
        ReachedGrenadeTotalLimit,
        NotAllowedByTeam,
        NotAllowedByMap,
        NotAllowedByMode,
        NotAllowedForPurchase,
        NotAllowedByProhibition,
    };

    // Possible results for CSPlayer::CanAcquire
    public enum AcquireMethod : int
    {
        PickUp = 0,
        Buy,
    };

    public MemoryFunctionWithReturn<CCSPlayer_ItemServices, CEconItemView, AcquireMethod, NativeObject, AcquireResult> CCSPlayer_CanAcquireFunc;

    public MemoryFunctionWithReturn<int, string, CCSWeaponBaseVData> GetCSWeaponDataFromKeyFunc;

    public HookResult OnWeaponCanAcquire(DynamicHook hook)
    {
        var vdata = GetCSWeaponDataFromKeyFunc.Invoke(-1, hook.GetParam<CEconItemView>(1).ItemDefinitionIndex.ToString());
        // Weapon is not restricted

        var client = hook.GetParam<CCSPlayer_ItemServices>(0).Pawn.Value!.Controller.Value!.As<CCSPlayerController>();

        if (client == null || !client.IsValid || !client.PawnIsAlive)
            return HookResult.Continue;

        // Print chat message if we attempted to buy this weapon
        if (hook.GetParam<AcquireMethod>(2) != AcquireMethod.PickUp)
        {
            hook.SetReturn(AcquireResult.AlreadyOwned);
        }
        else
        {
            hook.SetReturn(AcquireResult.InvalidItem);
        }

        if (ActiveTeamGamesGameBase != null)
        {
            return ActiveTeamGamesGameBase?.OnWeaponCanAcquire(client, vdata.Name) ?? HookResult.Continue;
        }
        else if (PatronuKoruActive)
        {
            if (ValidateCallerPlayer(client, false) == false) return HookResult.Continue;
            if (client?.SteamID == PatronuKoruCTLider
                || client?.SteamID == PatronuKoruTLider)
            {
                if (ValidateCallerPlayer(client, false) == false) return HookResult.Continue;
                if (vdata.Name != null
                    && vdata.Name != "")
                {
                    if (vdata.Name.Contains("deagle"))
                    {
                        return HookResult.Continue;
                    }
                    if (vdata.Name.Contains("knife"))
                    {
                        return HookResult.Continue;
                    }
                    if (vdata.Name.Contains("bayonet"))
                    {
                        return HookResult.Continue;
                    }
                    //RemoveWeapons(client, true, "weapon_deagle");
                    return HookResult.Stop;
                }
            }
        }
        return HookResult.Continue;
    }
}