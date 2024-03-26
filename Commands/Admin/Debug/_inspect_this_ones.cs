//using CounterStrikeSharp.API;
//using CounterStrikeSharp.API.Core;
//using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
//using CounterStrikeSharp.API.Modules.Utils;
//using System.Runtime.InteropServices;

//namespace TurSonu;

//public class TurSonu : BasePlugin
//{
//    public override string ModuleName => "Tur Sonu Overlay";
//    public override string ModuleAuthor => "Ambrosian";
//    public override string ModuleVersion => "1.0";

//    public static readonly string Mahkum = "particles/ambrosian/roundend/ice/wint.vpcf";

//    public static readonly string Gardiyan = "particles/ambrosian/roundend/ice/winct.vpcf";

//    public override void Load(bool hotReload)
//    {
//        RegisterEventHandler<EventRoundEnd>((@event, _) => RoundSonu(@event.Winner));
//    }

//    public HookResult RoundSonu(int winner)
//    {
//        var effectName = (CsTeam)winner switch
//        {
//            CsTeam.Terrorist => Mahkum,
//            CsTeam.CounterTerrorist => Gardiyan,
//            _ => null
//        };
//        if (effectName == null)
//        {
//            Server.PrintToConsole("Round end effect is null");
//            return HookResult.Continue;
//        }

//        Utilities.GetPlayers()
//            .Where(IsValid)
//            .ToList()
//            .ForEach(x =>
//            {
//                if (IsValid(x) == false)
//                {
//                    return;
//                }
//                var particle = Utilities.CreateEntityByName<CParticleSystem>("info_particle_system");

//                if (particle != null && particle.IsValid)
//                {
//                    particle.EffectName = effectName;
//                    particle.TintCP = 1;
//                    var pawn = x.PlayerPawn.Value!.AbsOrigin!;
//                    particle.Teleport(new Vector(pawn.X, pawn.Y, pawn.Z), new(0, 0, 0), new(0, 0, 0));
//                    particle.DispatchSpawn();
//                    particle.AcceptInput("Start");
//                    if (IsValid(x) == false)
//                    {
//                        return;
//                    }
//                    CustomSetParent(particle, x.PlayerPawn.Value);
//                }
//            });
//        return HookResult.Continue;
//    }

//    public static bool IsValid(CCSPlayerController? x)
//    {
//        return x != null && x.IsValid && x.PlayerPawn.IsValid && x.Connected == PlayerConnectedState.PlayerConnected;
//    }

//    private static void CustomSetParent(CParticleSystem x, CCSPlayerPawn pawn)
//    {
//        var signature = GetSetParentSignature();
//        MemoryFunctionVoid<CParticleSystem, CCSPlayerPawn, bool, bool> func = new(signature);
//        func.Invoke(x, pawn, true, false);
//    }

//    private static string GetSetParentSignature()
//    {
//        string signature;
//        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
//        {
//            signature = "\\x48\\x85\\xF6\\x74\\x2A\\x48\\x8B\\x47\\x10\\xF6\\x40\\x31\\x02\\x75\\x2A\\x48\\x8B\\x46\\x10\\xF6\\x40\\x31\\x02\\x75\\x2A\\xB8\\x2A\\x2A\\x2A\\x2A";
//        }
//        else
//        {
//            signature = "\\x4D\\x8B\\xD9\\x48\\x85\\xD2\\x74\\x2A";
//        }
//        return signature;
//    }
//}