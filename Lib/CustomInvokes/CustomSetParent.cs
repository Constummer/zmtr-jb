using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using System.Runtime.InteropServices;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static void CustomSetParent(CParticleSystem x, CCSPlayerPawn pawn)
    {
        var signature = GetSetParentSignature();
        MemoryFunctionVoid<CParticleSystem, CCSPlayerPawn, bool, bool> func = new(signature);
        func.Invoke(x, pawn, true, false);
        //VirtualFunction.CreateVoid<CParticleSystem>(x.Handle, GameData.GetOffset("CBaseEntity_SetParent"))(x);
    }

    private static string GetSetParentSignature()
    {
        string signature;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            signature = "\\x48\\x85\\xF6\\x74\\x2A\\x48\\x8B\\x47\\x10\\xF6\\x40\\x31\\x02\\x75\\x2A\\x48\\x8B\\x46\\x10\\xF6\\x40\\x31\\x02\\x75\\x2A\\xB8\\x2A\\x2A\\x2A\\x2A";
        }
        else
        {
            signature = "\\x4D\\x8B\\xD9\\x48\\x85\\xD2\\x74\\x2A";
        }
        return signature;
    }
}