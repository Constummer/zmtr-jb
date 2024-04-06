using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using System.Runtime.InteropServices;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    //private void CustomEmitSound(CCSPlayerController x)
    //{
    //if (_Config.Additional.CustomRespawnEnabled)
    //{
    //    if (ValidateCallerPlayer(x, false) == false)
    //    {
    //        return;
    //    }
    //    var xpawn = x.PlayerPawn.Value;
    //    var signature = GetCustomEmitSoundSignature();
    //    MemoryFunctionVoid<CCSPlayerController, CCSPlayerPawn, bool, bool> CBasePlayerController_SetPawnFunc = new(signature);
    //    CBasePlayerController_SetPawnFunc.Invoke(x, xpawn, true, false);
    //}
    //else
    //{
    //    //x.CustomEmitSound();
    //}
    //}

    //private static MemoryFunctionVoid<CBaseEntity, string, int, float, float> CBaseEntity_EmitSoundParamsFunc = new(GetCustomEmitSoundSignature());

    //public static void CBaseEntity_EmitSoundParams(CBaseEntity entity, string soundpath, int pitch = 100, float volume = 1.0f, float delay = 0.0f)
    //{
    //    if (!entity.IsValid)
    //        return;

    //    CBaseEntity_EmitSoundParamsFunc.Invoke(entity, soundpath, pitch, volume, delay);
    //}

    //private static string GetCustomEmitSoundSignature()
    //{
    //    string signature;
    //    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    //    {
    //        signature = "\\x48\\xB8\\x2A\\x2A\\x2A\\x2A\\x2A\\x2A\\x2A\\x2A\\x55\\x48\\x89\\xE5\\x41\\x55\\x41\\x54\\x49\\x89\\xFC\\x53\\x48\\x89\\xF3";
    //    }
    //    else
    //    {
    //        signature = "\\x44\\x88\\x4C\\x24\\x2A\\x55\\x57";
    //    }
    //    return signature;
    //}
}