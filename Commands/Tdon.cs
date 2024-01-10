using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("tdon", "Freeze a t.")]
    [ConsoleCommand("td", "Freeze a t.")]
    public void OnTDonCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        GetPlayers(CsTeam.Terrorist)
                         .Where(x => x.PawnIsAlive)
                         .ToList()
                         .ForEach(x =>
                         {
                             if (TeamActive == false)
                             {
                                 SetColour(x, Config.Burry.BuryColor);
                             }
                             x.PlayerPawn.Value!.MoveType = MoveType_t.MOVETYPE_OBSOLETE;
                             Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
                             Vector currentSpeed = new Vector(0, 0, 0);
                             QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
                             x.PlayerPawn.Value.Teleport(currentPosition, currentRotation, currentSpeed);
                         });
        FreezeOrUnfreezeSound();
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}, {CC.G}mahkûmları {CC.B}dondurdu{CC.W}.");
    }

    [ConsoleCommand("tdonboz", "Unfreeze t.")]
    [ConsoleCommand("tdb", "Unfreeze t.")]
    public void TDonbozCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        TdonbozAction();
        FreezeOrUnfreezeSound();
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}, {CC.G}mahkûmların {CC.B}donunu kaldırdı{CC.W}.");
    }

    private static void TdonbozAction()
    {
        GetPlayers(CsTeam.Terrorist)
                        .Where(x => x.PawnIsAlive)
                        .ToList()
                        .ForEach(x =>
                        {
                            if (TeamActive == false)
                            {
                                SetColour(x, DefaultPlayerColor);
                            }
                            RefreshPawn(x);

                            x.PlayerPawn.Value.MoveType = MoveType_t.MOVETYPE_WALK;
                        });
    }

    private static void AllDonbozAction()
    {
        GetPlayers()
           .Where(x => x.PawnIsAlive)
           .ToList()
           .ForEach(x =>
           {
               if (TeamActive == false)
               {
                   SetColour(x, DefaultPlayerColor);
               }
               RefreshPawn(x);

               x.PlayerPawn.Value.MoveType = MoveType_t.MOVETYPE_WALK;
           });
    }
}