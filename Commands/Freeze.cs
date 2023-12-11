using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static int freezeTimer = 0;
    private static bool freezeWanted = false;

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
            freezeWanted = true;
            freezeTimer = value;
            BasicCountdown.CommandStartTextCountDown(this, $"{value} saniye kaldı");

            _ = AddTimer(1f, () =>
            {
                if (freezeWanted)
                {
                    freezeTimer--;
                    if (freezeTimer == 0)
                    {
                        freezeWanted = false;
                        GetPlayers()
                        .Where(x => x != null
                             && x.PlayerPawn.IsValid
                             && x.PawnIsAlive
                             && x.IsValid
                             && x?.PlayerPawn?.Value != null
                             && (CsTeam)x.TeamNum == CsTeam.Terrorist)
                        .ToList()
                        .ForEach(x =>
                        {
                            if (x.AbsOrigin != null)
                            {
                                x.Teleport(new Vector(x.AbsOrigin.X, x.AbsOrigin.Y, x.AbsOrigin.Z - 50), ZeroRotation, ZeroSpeed);
                            }
                            x.PlayerPawn.Value.MoveType = MoveType_t.MOVETYPE_NONE;
                        });
                    }
                }
            }, TimerFlags.REPEAT);
        }
    }

    [ConsoleCommand("freeze", "Freeze a player.")]
    [CommandHelper(1, "<playerismi-@all-@t-@ct-@me-@alive-@random-@randomt-@randomct>")]
    public void OnFreezeCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);
        FreezeTarget(target, player.PlayerName);
    }

    [ConsoleCommand("unfreeze", "Unfreeze a player.")]
    [CommandHelper(1, "<playerismi-@all-@t-@ct-@me-@alive-@random-@randomt-@randomct>")]
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
                     && ExecuteFreezeOrUnfreeze(x, target, player.PlayerName, out randomFreeze))
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

    #endregion Freeze-Unfreeze
}