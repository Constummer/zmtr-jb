using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

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
            BasicCountdown.CommandStartTextCountDown(this, $"Mahkûmların donmasına {value} saniye kaldı!");

            _ = AddTimer(value, () =>
            {
                GetPlayers()
                .Where(x => x != null
                     && x.PlayerPawn.IsValid
                     && x.PawnIsAlive
                     && x.IsValid
                     && x?.PlayerPawn?.Value != null
                     && GetTeam(x) == CsTeam.Terrorist)
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
            });
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
        FreezeTarget(target, player!.PlayerName);
        player.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.White}Admin, {ChatColors.Blue}{player!.PlayerName} {ChatColors.White}adlı oyuncuyu dondurdu.");
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
                UnfreezeX(player, x, target, randomFreeze);
            });
        player.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.White}{player.PlayerName}, {ChatColors.Blue}{target} {ChatColors.White}adlı oyuncunun donunu kaldırdı.");
    }

    private void FreezeTarget(string target, string self)
    {
        bool randomFreeze = false;

        GetPlayers()
              .Where(x => x != null
                   && x.PlayerPawn.IsValid
                   && x.PawnIsAlive
                   && x.IsValid
                   && GetTargetAction(x, target, self))
              .ToList()
        .ForEach(x =>
        {
            Freeze(target, x, self, ref randomFreeze);
        });
    }

    private static void Freeze(string target, CCSPlayerController x, string self, ref bool randomFreeze)
    {
        if (randomFreeze == false
        && x?.PlayerPawn?.Value != null
            && ExecuteFreezeOrUnfreeze(x, target, self, out randomFreeze))
        {
            SetColour(x, _Config.BuryColor);
            RefreshPawn(x);

            //Vector currentPosition = x.Pawn.Value.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
            //Vector currentSpeed = new Vector(0, 0, 0);
            //QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
            //x.PlayerPawn.Value.Teleport(currentPosition, currentRotation, currentSpeed);
            x.PlayerPawn.Value.MoveType = MoveType_t.MOVETYPE_OBSOLETE;
        }
    }

    private static bool UnfreezeX(CCSPlayerController? player, CCSPlayerController x, string target, bool randomFreeze)
    {
        if (randomFreeze == false
                     && x?.PlayerPawn?.Value != null
                     && ExecuteFreezeOrUnfreeze(x, target, player!.PlayerName, out randomFreeze))
        {
            //Server.NextFrame(() =>
            //{
            SetColour(x, DefaultPlayerColor);
            RefreshPawn(x);

            //Vector currentPosition = x.Pawn.Value.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
            //Vector currentSpeed = new Vector(0, 0, 0);
            //QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
            //x.PlayerPawn.Value.Teleport(new(currentPosition.X, currentPosition.Y, currentPosition.Z + 100), currentRotation, currentSpeed);
            x.PlayerPawn.Value.MoveType = MoveType_t.MOVETYPE_WALK;
            //});
        }
        return randomFreeze;
    }

    #endregion Freeze-Unfreeze
}