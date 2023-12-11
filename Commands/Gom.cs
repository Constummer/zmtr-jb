using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;
using Microsoft.Extensions.Logging;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static readonly Vector ZeroSpeed = new Vector(0, 0, 0);
    private static readonly QAngle ZeroRotation = new QAngle(0, 0, 0);

    #region Gom - gomsure

    [ConsoleCommand("gom", "yere gomer.")]
    public void Gom(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;

        var target = info.GetArg(1);

        GetPlayers()
           .Where(x => x.PawnIsAlive
                   && GetTargetAction(x, target, player.PlayerName))
           .ToList()
           .ForEach(x =>
           {
               if (x?.PlayerPawn?.Value != null)
               {
                   SetColour(x, Color.FromArgb(255, 0, 0, 255));
                   RefreshPawn(x);

                   Vector currentPosition = x?.Pawn?.Value?.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
                   Vector currentSpeed = new Vector(0, 0, 0);
                   QAngle currentRotation = new QAngle(0, 0, 0);
                   x.Teleport(new(currentPosition.X, currentPosition.Y, currentPosition.Z - 30), currentRotation, currentSpeed);

                   x.PlayerPawn.Value.MoveType = MoveType_t.MOVETYPE_NONE;
               }
           });
    }

    [ConsoleCommand("gomsure", "yere gomer.")]
    [CommandHelper(1, "<sure>")]
    public void GomSure(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;

        var target = info.GetArg(1);
        var targetFreeze = GetTargetArgument(target);
        if (int.TryParse(target, out int value))
        {
            BasicCountdown.CommandStartTextCountDown(this, $"{value} saniye kaldı");
            AddTimer(value, () =>
            {
                GetPlayers(CsTeam.Terrorist)
                   .Where(x => x.PawnIsAlive)
                   .ToList()
                   .ForEach(x =>
                   {
                       if (x?.PlayerPawn?.Value != null)
                       {
                           //SetColour(x, Color.FromArgb(255, 0, 0, 255));
                           //RefreshPawn(x);
                           if (x.AbsOrigin != null)
                           {
                               x.Teleport(new Vector(x.AbsOrigin.X, x.AbsOrigin.Y, x.AbsOrigin.Z - 50), ZeroRotation, ZeroSpeed);
                           }
                           x.PlayerPawn.Value.MoveType = MoveType_t.MOVETYPE_NONE;
                       }
                   });
            });
        }
    }

    [ConsoleCommand("gomkaldir", "yerden kaldirir.")]
    public void GomKaldir(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;

        var target = info.GetArg(1);

        GetPlayers()
           .Where(x => x.PawnIsAlive
                   && GetTargetAction(x, target, player.PlayerName))
           .ToList()
           .ForEach(x =>
           {
               if (x?.PlayerPawn?.Value != null)
               {
                   SetColour(x, Color.FromArgb(255, 255, 255, 255));
                   RefreshPawn(x);

                   x.PlayerPawn.Value.MoveType = MoveType_t.MOVETYPE_WALK;
                   Vector currentPosition = x.Pawn.Value.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
                   Vector currentSpeed = new Vector(0, 0, 0);
                   QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
                   x.PlayerPawn.Value.Teleport(new(currentPosition.X, currentPosition.Y, currentPosition.Z + 100), currentRotation, currentSpeed);
               }
           });
    }

    #endregion Gom - gomsure
}