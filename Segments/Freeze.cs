using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using System.Drawing;
using Microsoft.Extensions.Logging;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Freeze-Unfreeze

    [ConsoleCommand("fz", "Freeze a player.")]
    [CommandHelper(1, "<saniye>")]
    public void OnFzCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
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

    [ConsoleCommand("freeze", "Freeze a player.")]
    [CommandHelper(1, "<playerismi-@all-@t-@ct-@alive-@random-@randomt-@randomct>")]
    public void OnFreezeCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);
        FreezeTarget(target);
    }

    [ConsoleCommand("unfreeze", "Unfreeze a player.")]
    [CommandHelper(1, "<playerismi-@all-@t-@ct-@alive-@random-@randomt-@randomct>")]
    public void OnUnfreezeCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);
        bool randomFreeze = false;
        GetPlayers()
           .Where(x => x.PawnIsAlive)
           .ToList()
           .ForEach(x =>
           {
               if (randomFreeze == false
                    && x?.PlayerPawn?.Value != null
                    && ExecuteFreezeOrUnfreeze(x, target, out randomFreeze))
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

    #endregion Freeze-Unfreeze
}