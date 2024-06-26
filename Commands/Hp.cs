﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
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
        var target = info.ArgString.GetArgSkipFromLast(1);
        if (!int.TryParse(info.ArgString.GetArgLast(), out var health) || health < 1)
        {
            player!.PrintToChat($"{Prefix}{CC.G} Can değeri yanlış!");
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        var targetArgument = GetTargetArgument(target);
        GetPlayers()
               .Where(x => x.PawnIsAlive
                        && x.Pawn.Value != null
                        && GetTargetAction(x, target, player))
               .ToList()
               .ForEach(x =>
               {
                   if (ValidateCallerPlayer(x, false) == true)
                   {
                       SetHp(x, health);
                   }

                   RefreshPawn(x);
                   if ((targetArgument & TargetForArgument.SingleUser) == targetArgument)
                   {
                       Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncunun canını {CC.B}{health} {CC.W}olarak ayarladı.");
                   }
               });
        if ((targetArgument & TargetForArgument.SingleUser) != targetArgument)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefinin canını {CC.B}{health} {CC.W}olarak ayarladı.");
        }
    }

    [ConsoleCommand("hpt", "Change t player's HP.")]
    public void OnHealthTCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        GetPlayers(CsTeam.Terrorist)
               .Where(x => x.PawnIsAlive)
               .ToList()
               .ForEach(x =>
               {
                   SetHp(x, 100);

                   RefreshPawn(x);
               });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{T_PluralCamelPossesive} canını {CC.B}100 {CC.W}olarak ayarladı.");
    }

    [ConsoleCommand("hpct", "Change ct player's HP.")]
    public void OnHealthCTCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        GetPlayers(CsTeam.CounterTerrorist)
               .Where(x => x.PawnIsAlive)
               .ToList()
               .ForEach(x =>
               {
                   SetHp(x, 100);

                   RefreshPawn(x);
               });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{CT_PluralCamelPossesive} canını {CC.B}100 {CC.W}olarak ayarladı.");
    }

    [ConsoleCommand("hpall", "Change all player's HP.")]
    public void OnHealthALLCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        GetPlayers()
               .Where(x => x.PawnIsAlive)
               .ToList()
               .ForEach(x =>
               {
                   SetHp(x, 100);

                   RefreshPawn(x);
               });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}herkesin canını {CC.B}100 {CC.W}olarak ayarladı.");
    }

    public static void SetHp(CCSPlayerController controller, int health = 100)
    {
        if (health <= 0 || !controller.PawnIsAlive || controller.PlayerPawn.IsValid == false || controller.PlayerPawn.Value == null) return;

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