using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void CustomRespawn(CCSPlayerController x)
    {
        if (Config.CustomRespawnEnabled)
        {
            if (ValidateCallerPlayer(x, false) == false)
            {
                return;
            }
            var xpawn = x.PlayerPawn.Value;
            MemoryFunctionVoid<CCSPlayerController, CCSPlayerPawn, bool, bool> CBasePlayerController_SetPawnFunc = new(GameData.GetSignature("CBasePlayerController_SetPawn"));
            CBasePlayerController_SetPawnFunc.Invoke(x, xpawn, true, false);
            VirtualFunction.CreateVoid<CCSPlayerController>(x.Handle, GameData.GetOffset("CCSPlayerController_Respawn"))(x);
        }
        else
        {
            x.Respawn();
        }
    }
}