﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
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

        var target = info.ArgString.GetArgSkip(0);
        var targetArgument = GetTargetArgument(target);

        LogManagerCommand(player.SteamID, info.GetCommandString);
        GetPlayers()
             .Where(x => x.PawnIsAlive
                     && GetTargetAction(x, target, player))
             .ToList()
             .ForEach(x =>
          {
              if (x?.PlayerPawn?.Value != null)
              {
                  if (TeamActive == false)
                  {
                      SetColour(x, Config.Burry.BuryColor);
                  }
                  SetMoveType(x, MoveType_t.MOVETYPE_OBSOLETE);

                  if (x != null)
                  {
                      if (ValidateCallerPlayer(x, false) == false)
                          return;
                      Vector currentPosition = x.PlayerPawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
                      x.PlayerPawn.Value.Teleport(currentPosition.With(z: currentPosition.Z - 40), QAngle.Zero, Vector.Zero);
                  }
                  if ((targetArgument & TargetForArgument.SingleUser) == targetArgument)
                  {
                      Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu {CC.B}gömdü{CC.W}.");
                  }
              }
          });
        if ((targetArgument & TargetForArgument.SingleUser) != targetArgument)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefini {CC.B}gömdü{CC.W}.");
        }
    }

    [ConsoleCommand("gomsure", "yere gomer.")]
    [CommandHelper(1, "<sure>")]
    public void GomSure(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var target = info.ArgString.GetArg(0);
        if (int.TryParse(target, out int value))
        {
            if (value > 120)
            {
                player.PrintToChat("Max 120 sn girebilirsin");
            }
            else
            {
                LogManagerCommand(player.SteamID, info.GetCommandString);
                BasicCountdown.CommandStartTextCountDown(this, $"{T_PluralCamelPossesive} gömülmesine {value} saniye kaldı!");
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
                        if (ValidateCallerPlayer(x, false) == false) return;
                        if (TeamActive == false)
                        {
                            SetColour(x, Config.Burry.BuryColor);
                        }
                        SetMoveType(x, MoveType_t.MOVETYPE_OBSOLETE);

                        if (x != null)
                        {
                            if (ValidateCallerPlayer(x, false) == false)
                                return;
                            Vector currentPosition = x.PlayerPawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
                            x.PlayerPawn.Value.Teleport(currentPosition.With(z: currentPosition.Z - 40), QAngle.Zero, Vector.Zero);
                        }
                    });
                }, SOM);
            }
        }
    }

    [ConsoleCommand("gomkaldir", "yerden kaldirir.")]
    [ConsoleCommand("gomboz", "yerden kaldirir.")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me>")]
    public void GomKaldir(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        var target = info.ArgString.GetArgSkip(0);
        var targetArgument = GetTargetArgument(target);

        LogManagerCommand(player.SteamID, info.GetCommandString);
        GetPlayers()
           .Where(x => x.PawnIsAlive
                   && GetTargetAction(x, target, player))
           .ToList()
           .ForEach(x =>
           {
               if (x?.PlayerPawn?.Value != null)
               {
                   if (TeamActive == false)
                   {
                       SetColour(x, DefaultColor);
                   }

                   SetMoveType(x, MoveType_t.MOVETYPE_WALK);
                   Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
                   Vector currentSpeed = new Vector(0, 0, 0);
                   QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
                   x.PlayerPawn.Value.Teleport(new(currentPosition.X, currentPosition.Y, currentPosition.Z + 100), currentRotation, currentSpeed);
                   if ((targetArgument & TargetForArgument.SingleUser) == targetArgument)
                   {
                       Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu {CC.B}kaldırdı{CC.W}.");
                   }
               }
           });
        if ((targetArgument & TargetForArgument.SingleUser) != targetArgument)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefini {CC.B}kaldırdı{CC.W}.");
        }
    }

    #endregion Gom - gomsure
}