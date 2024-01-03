using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private CounterStrikeSharp.API.Modules.Timers.Timer fzTimer = null;

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
            if (value > 120)
            {
                player.PrintToChat("Max 120 sn girebilirsin");
            }
            else
            {
                BasicCountdown.CommandStartTextCountDown(this, $"Mahkûmların donmasına {value} saniye kaldı!");
                fzTimer?.Kill();
                fzTimer = AddTimer(value, () =>
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
                        SetColour(x, Config.Burry.BuryColor);

                        x.PlayerPawn.Value!.MoveType = MoveType_t.MOVETYPE_OBSOLETE;
                        Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
                        Vector currentSpeed = new Vector(0, 0, 0);
                        QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
                        x.PlayerPawn.Value.Teleport(currentPosition, currentRotation, currentSpeed);
                    });
                    FreezeOrUnfreezeSound();
                    Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}mahkûmları {CC.B}dondurdu{CC.W}.");
                });
            }
        }
    }

    [ConsoleCommand("freeze", "Freeze a player.")]
    [CommandHelper(1, "<playerismi-@all-@t-@ct-@me>")]
    public void OnFreezeCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye4"))
        {
            player.PrintToChat($" {CC.LR}[ZMTR]{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);
        var targetArgument = GetTargetArgument(target);

        GetPlayers()
                   .Where(x => x.PawnIsAlive && GetTargetAction(x, target, player.PlayerName))
                   .ToList()
                   .ForEach(x =>
                   {
                       if (targetArgument == TargetForArgument.None)
                       {
                           Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu{CC.B} dondurdu{CC.W}.");
                       }
                       SetColour(x, Config.Burry.BuryColor);

                       x.PlayerPawn.Value!.MoveType = MoveType_t.MOVETYPE_OBSOLETE;
                       Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
                       Vector currentSpeed = new Vector(0, 0, 0);
                       QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
                       x.PlayerPawn.Value.Teleport(currentPosition, currentRotation, currentSpeed);
                   });
        FreezeOrUnfreezeSound();
        if (targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{target} {CC.W}hedefini {CC.B}dondurdu");
        }
    }

    [ConsoleCommand("unfreeze", "Unfreeze a player.")]
    [CommandHelper(1, "<playerismi-@all-@t-@ct-@me>")]
    public void OnUnfreezeCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);
        var targetArgument = GetTargetArgument(target);

        bool randomFreeze = false;
        GetPlayers()
            .Where(x => x.PawnIsAlive)
            .ToList()
            .ForEach(x =>
            {
                if (targetArgument == TargetForArgument.None)
                {
                    Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{x.PlayerName} {CC.W}adlı oyuncunun{CC.B} donunu bozdu{CC.W}.");
                }
                SetColour(x, DefaultPlayerColor);
                RefreshPawn(x);

                x.PlayerPawn.Value.MoveType = MoveType_t.MOVETYPE_WALK;
            });
        FreezeOrUnfreezeSound();
        if (targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{target} {CC.W}hedefinin {CC.B}donunu {CC.W}bozdu.");
        }
    }

    private void FreezeTarget(string target, string self, bool displayMessage = true)
    {
        bool randomFreeze = false;
        var targetArgument = GetTargetArgument(target);

        GetPlayers()
              .Where(x => x != null
                   && x.PlayerPawn.IsValid
                   && x.PawnIsAlive
                   && x.IsValid
                   && GetTargetAction(x, target, self))
              .ToList()
        .ForEach(x =>
        {
            if (displayMessage && targetArgument == TargetForArgument.None)
            {
                Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{self}{CC.W} adlı admin, {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu {CC.B}dondurdu{CC.W}.");
            }
            Freeze(target, x, self, ref randomFreeze);
        });
        if (displayMessage && targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{self}{CC.W} adlı admin, {CC.G}{target} {CC.W}hedefinin {CC.B}donunu {CC.W}dondurdu.");
        }
    }

    private static void Freeze(string target, CCSPlayerController x, string self, ref bool randomFreeze)
    {
        if (randomFreeze == false
        && x?.PlayerPawn?.Value != null
            && ExecuteFreezeOrUnfreeze(x, target, self, out randomFreeze))
        {
            SetColour(x, _Config.Burry.BuryColor);
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