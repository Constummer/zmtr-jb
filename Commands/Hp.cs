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
        if (info.ArgCount != 3) return;
        var target = info.GetArg(1);
        if (!int.TryParse(info.GetArg(2), out var health))
        {
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR] Can degeri duzgun deil!");
            return;
        }
        GetPlayers()
               .Where(x => x.PawnIsAlive
                        && x.Pawn.Value != null
                        && GetTargetAction(x, target, player.PlayerName))
               .ToList()
               .ForEach(x =>
               {
                   x.Pawn.Value!.Health = health;

                   RefreshPawn(x);
               });
    }

    [ConsoleCommand("hpt", "Change t player's HP.")]
    [CommandHelper(1, "<miktar>")]
    public void OnHealthTCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (!int.TryParse(info.GetArg(1), out var health))
        {
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR] Can degeri duzgun deil!");
            return;
        }
        GetPlayers(CsTeam.Terrorist)
               .Where(x => x.PawnIsAlive)
               .ToList()
               .ForEach(x =>
               {
                   x.Pawn.Value!.Health = health;

                   RefreshPawn(x);
               });
    }

    [ConsoleCommand("hpct", "Change ct player's HP.")]
    [CommandHelper(1, "<miktar>")]
    public void OnHealthCTCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (!int.TryParse(info.GetArg(1), out var health))
        {
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR] Can degeri duzgun deil!");
            return;
        }
        GetPlayers(CsTeam.CounterTerrorist)
               .Where(x => x.PawnIsAlive)
               .ToList()
               .ForEach(x =>
               {
                   x.Pawn.Value!.Health = health;

                   RefreshPawn(x);
               });
    }

    [ConsoleCommand("hpall", "Change all player's HP.")]
    [CommandHelper(1, "<miktar>")]
    public void OnHealthALLCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (!int.TryParse(info.GetArg(1), out var health))
        {
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR] Can degeri duzgun deil!");
            return;
        }
        GetPlayers()
               .Where(x => x.PawnIsAlive)
               .ToList()
               .ForEach(x =>
               {
                   x.Pawn.Value!.Health = health;

                   RefreshPawn(x);
               });
    }

    #endregion HP
}