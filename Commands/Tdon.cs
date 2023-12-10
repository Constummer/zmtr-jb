using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("tdon", "Freeze a t.")]
    [CommandHelper(1, "<saniye>")]
    public void OnTDonCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        if (info.ArgCount != 2)
        {
            FreezeTarget("@t");
        }
        else
        {
            var target = info.GetArg(1);
            if (int.TryParse(target, out int value))
            {
                BasicCountdown.CommandStartTextCountDown(this, $"{value} saniye kaldı");
                AddTimer(value, () =>
                {
                    FreezeTarget("@t");
                });
            }
        }
    }

    [ConsoleCommand("tdonboz", "Unfreeze t.")]
    public void TDonbozCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        bool randomFreeze = false;
        GetPlayers()
           .Where(x => x.PawnIsAlive)
           .ToList()
           .ForEach(x =>
           {
               if (x?.PlayerPawn?.Value != null
                    && ExecuteFreezeOrUnfreeze(x, "@t", out randomFreeze))
               {
                   SetColour(x, Color.FromArgb(255, 255, 255, 255));
                   RefreshPawn(x);

                   x.PlayerPawn.Value.MoveType = MoveType_t.MOVETYPE_WALK;
                   Vector currentPosition = x.Pawn.Value.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
                   Vector currentSpeed = new Vector(0, 0, 0);
                   QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
                   x.PlayerPawn.Value.Teleport(new(currentPosition.X, currentPosition.Y, currentPosition.Z + 15), currentRotation, currentSpeed);
               }
           });
    }
}