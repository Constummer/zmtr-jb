using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("tdon", "Freeze a t.")]
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
                             SetColour(x, Config.BuryColor);

                             x.PlayerPawn.Value!.MoveType = MoveType_t.MOVETYPE_OBSOLETE;
                             Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
                             Vector currentSpeed = new Vector(0, 0, 0);
                             QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
                             x.PlayerPawn.Value.Teleport(currentPosition, currentRotation, currentSpeed);
                         });
        FreezeOrUnfreezeSound();
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Green}mahkûmları {ChatColors.Blue}dondurdu{ChatColors.White}.");
    }

    [ConsoleCommand("tdonboz", "Unfreeze t.")]
    [ConsoleCommand("tdb", "Unfreeze t.")]
    public void TDonbozCommand(CCSPlayerController? player, CommandInfo info)
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
                            SetColour(x, DefaultPlayerColor);
                            RefreshPawn(x);

                            x.PlayerPawn.Value.MoveType = MoveType_t.MOVETYPE_WALK;
                        });
        FreezeOrUnfreezeSound();
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Green}mahkûmların {ChatColors.Blue}donunu kaldırdı{ChatColors.White}.");
    }
}