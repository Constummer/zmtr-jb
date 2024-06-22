using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using System.Runtime.InteropServices;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static void CustomSetParent<T>(T x, CCSPlayerPawn pawn)
    {
        var signature = GetSetParentSignature();
        MemoryFunctionVoid<T, CCSPlayerPawn, bool, bool> func = new(signature);
        func.Invoke(x, pawn, true, false);
    }

    private static string GetSetParentSignature()
    {
        string signature;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            signature = _Config.Additional.CustomSetParentLinuxSignature;
        }
        else
        {
            signature = _Config.Additional.CustomSetParentWindowsSignature;
        }
        return signature;
    }
}