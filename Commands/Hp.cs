﻿using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region HP

    [ConsoleCommand("hp", "Change a player's HP.")]
    [CommandHelper(2, "<oyuncu ismi,@t,@ct,@all,@me>  <health>")]
    public void OnHealthCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 3) return;
        var target = info.GetArg(1);
        if (!int.TryParse(info.GetArg(2), out var health))
        {
            player!.PrintToChat($" {ChatColors.LightRed}[ZMTR]{ChatColors.Green} Can degeri duzgun deil!");
            return;
        }
        GetPlayers()
               .Where(x => x.PawnIsAlive
                        && x.Pawn.Value != null
                        && GetTargetAction(x, target, player!.PlayerName))
               .ToList()
               .ForEach(x =>
               {
                   if (ValidateCallerPlayer(x, false) == true)
                   {
                       SetHp(x, health);
                   }

                   RefreshPawn(x);
               });
    }

    [ConsoleCommand("hpt", "Change t player's HP.")]
    public void OnHealthTCommand(CCSPlayerController? player, CommandInfo info)
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
                   SetHp(x, 100);

                   RefreshPawn(x);
               });
    }

    [ConsoleCommand("hpct", "Change ct player's HP.")]
    public void OnHealthCTCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GetPlayers(CsTeam.CounterTerrorist)
               .Where(x => x.PawnIsAlive)
               .ToList()
               .ForEach(x =>
               {
                   SetHp(x, 100);

                   RefreshPawn(x);
               });
    }

    [ConsoleCommand("hpall", "Change all player's HP.")]
    public void OnHealthALLCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GetPlayers()
               .Where(x => x.PawnIsAlive)
               .ToList()
               .ForEach(x =>
               {
                   SetHp(x, 100);

                   RefreshPawn(x);
               });
    }

    public static void SetHp(CCSPlayerController controller, int health = 100)
    {
        if (health <= 0 || !controller.PawnIsAlive || controller.PlayerPawn.Value == null) return;

        controller.Health = health;
        controller.PlayerPawn.Value.Health = health;

        if (health > 100)
        {
            controller.MaxHealth = health;
            controller.PlayerPawn.Value.MaxHealth = health;
        }
    }

    #endregion HP
}