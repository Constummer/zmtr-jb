﻿using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Gom - gomsure

    [ConsoleCommand("gom", "yere gomer.")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me>")]
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
                     && GetTargetAction(x, target, player!.PlayerName))
             .ToList()
             .ForEach(x =>
          {
              if (x?.PlayerPawn?.Value != null)
              {
                  SetColour(x, Config.BuryColor);

                  x.PlayerPawn.Value.MoveType = MoveType_t.MOVETYPE_OBSOLETE;
                  Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
                  Vector currentSpeed = new Vector(0, 0, 0);
                  QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
                  x.PlayerPawn.Value.Teleport(new(currentPosition.X, currentPosition.Y, currentPosition.Z - 40), currentRotation, currentSpeed);
              }
          });
        player.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.White}Admin, {ChatColors.Blue}{player!.PlayerName} {ChatColors.White}adlı oyuncuyu gömdü.");
    }

    [ConsoleCommand("gomsure", "yere gomer.")]
    [CommandHelper(1, "<sure>")]
    public void GomSure(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var target = info.GetArg(1);
        if (int.TryParse(target, out int value))
        {
            BasicCountdown.CommandStartTextCountDown(this, $"Mahkûmların gömülmesine {value} saniye kaldı!");
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
                    x.PlayerPawn.Value.Teleport(new(currentPosition.X, currentPosition.Y, currentPosition.Z - 40), currentRotation, currentSpeed);
                });
            });
        }
    }

    [ConsoleCommand("gomkaldir", "yerden kaldirir.")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me>")]
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
                   && GetTargetAction(x, target, player!.PlayerName))
           .ToList()
           .ForEach(x =>
           {
               if (x?.PlayerPawn?.Value != null)
               {
                   SetColour(x, DefaultPlayerColor);
                   RefreshPawn(x);

                   x.PlayerPawn.Value.MoveType = MoveType_t.MOVETYPE_WALK;
                   Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
                   Vector currentSpeed = new Vector(0, 0, 0);
                   QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
                   x.PlayerPawn.Value.Teleport(new(currentPosition.X, currentPosition.Y, currentPosition.Z + 100), currentRotation, currentSpeed);
               }
           });
        player.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.White}Admin, {ChatColors.Blue}{player!.PlayerName} {ChatColors.White}adlı oyuncuyu kaldırdı.");
    }

    #endregion Gom - gomsure
}