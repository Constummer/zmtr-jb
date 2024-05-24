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
            signature = "\\x55\\x48\\x89\\xE5\\x41\\x54\\x49\\x89\\xFC\\x53\\xE8\\x?\\x?\\x?\\x?\\x41\\x80\\xBC\\x24\\x?\\x?\\x?\\x?\\x?\\x41\\xC6\\x84\\x24"
                /*"\\x55\\x48\\x89\\xE5\\x41\\x57\\x41\\x56\\x41\\x55\\x41\\x54\\x49\\x89\\xFC\\x53\\x48\\x89\\xF3\\x48\\x81\\xEC\\xC8\\x00\\x00\\x00"*/;
        }
        else
        {
            signature = "\\x44\\x88\\x4C\\x24\\x2A\\x55\\x57";
        }
        return signature;
    }
}