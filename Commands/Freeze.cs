using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
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
                FreezeOrUnfreezeSound();
                Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Green}mahkûmları {ChatColors.Blue}dondurdu{ChatColors.White}.");
            });
        }
    }

    [ConsoleCommand("freeze", "Freeze a player.")]
    [CommandHelper(1, "<playerismi-@all-@t-@ct-@me-@alive-@random-@randomt-@randomct>")]
    public void OnFreezeCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye4"))
        {
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR]{ChatColors.White} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);
        FreezeTarget(target, player!.PlayerName);
        FreezeOrUnfreezeSound();
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
        var targetArgument = GetTargetArgument(target);

        bool randomFreeze = false;
        GetPlayers()
            .Where(x => x.PawnIsAlive)
            .ToList()
            .ForEach(x =>
            {
                if (targetArgument == TargetForArgument.None)
                {
                    Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Green}{x.PlayerName} {ChatColors.White}adlı oyuncunun{ChatColors.Blue} donunu bozdu{ChatColors.White}.");
                }
                UnfreezeX(player, x, target, randomFreeze);
            });
        FreezeOrUnfreezeSound();
        if (targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Green}{target} {ChatColors.White}hedefinin {ChatColors.Blue}donunu {ChatColors.White}bozdu.");
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
                Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{self}{ChatColors.White} adlı admin, {ChatColors.Green}{x.PlayerName} {ChatColors.White}adlı oyuncuyu {ChatColors.Blue}dondurdu{ChatColors.White}.");
            }
            Freeze(target, x, self, ref randomFreeze);
        });
        if (displayMessage && targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{self}{ChatColors.White} adlı admin, {ChatColors.Green}{target} {ChatColors.White}hedefinin {ChatColors.Blue}donunu {ChatColors.White}dondurdu.");
        }
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