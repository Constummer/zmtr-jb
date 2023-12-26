using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Commands.Targeting;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("tdon", "Freeze a t.")]
    public void OnTDonCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        FreezeTarget("@t", "");
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Green}mahkûmları {ChatColors.Blue}dondurdu{ChatColors.White}.");
    }

    [ConsoleCommand("tdonboz", "Unfreeze t.")]
    public void TDonbozCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        bool randomFreeze = false;
        GetPlayers(CsTeam.Terrorist)
           .Where(x => x.PawnIsAlive)
           .ToList()
           .ForEach(x =>
           {
               randomFreeze = UnfreezeX(player, x, "@t", randomFreeze);
           });
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Green}mahkûmların {ChatColors.Blue}donunu kaldırdı{ChatColors.White}.");
    }
}