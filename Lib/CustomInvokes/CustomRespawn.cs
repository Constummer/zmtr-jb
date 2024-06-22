using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using System.Runtime.InteropServices;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static void CustomRespawn(CCSPlayerController x)
    {
        if (_Config.Additional.CustomRespawnEnabled)
        {
            if (ValidateCallerPlayer(x, false) == false)
            {
                return;
            }
            var xpawn = x.PlayerPawn.Value;
            var signature = GetRespawnSignature();
            MemoryFunctionVoid<CCSPlayerController, CCSPlayerPawn, bool, bool> CBasePlayerController_SetPawnFunc = new(signature);
            CBasePlayerController_SetPawnFunc.Invoke(x, xpawn, true, false);
            //VirtualFunction.CreateVoid<CCSPlayerController>(x.Handle, GameData.GetOffset("CCSPlayerController_Respawn"))(x);
            VirtualFunction.CreateVoid<CCSPlayerController>(x.Handle, 256)(x);
        }
        else
        {
            if (ValidateCallerPlayer(x, false) == false)
            {
                return;
            }
            x.Respawn();
        }
    }

    private void CustomRespawnIfActive(CCSPlayerController? player)
    {
        if (RespawnAcActive)
        {
            AddTimer(1, () =>
            {
                if (ValidateCallerPlayer(player, false) == true)
                {
                    CustomRespawn(player);
                }
            }, SOM);
        }
    }

    private static string GetRespawnSignature()
    {
        string signature;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            signature = _Config.Additional.CustomRespawnLinuxSignature;
        }
        else
        {
            signature = _Config.Additional.CustomRespawnWindowsSignature;
        }
        return signature;
    }
}