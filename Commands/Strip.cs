using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Commands.Targeting;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Strip

    [ConsoleCommand("strip", "Bicak dahil silme")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me>")]
    public void Strip(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);
        var targetArgument = GetTargetArgument(target);

        GetPlayers()
        .Where(x => x.PawnIsAlive
                    && GetTargetAction(x, target, player!.PlayerName))
        .ToList()
        .ForEach(x =>
        {
            RemoveWeapons(x, false);
            if (targetArgument == TargetForArgument.None)
            {
                Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Green}{x.PlayerName} {ChatColors.White}adlı oyuncunun {ChatColors.Blue}silahlarını {ChatColors.White}sildi.");
            }
        });
        if (targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Green}{target} {ChatColors.White}hedefinin {ChatColors.Blue}silahlarını {ChatColors.White}sildi.");
        }
    }

    [ConsoleCommand("stript", "Bicak dahil silme")]
    public void StripT(CCSPlayerController? player, CommandInfo info)
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
            RemoveWeapons(x, false);
        });
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Green}mahkûmların {ChatColors.Blue}silahlarını {ChatColors.White}sildi.");
    }

    [ConsoleCommand("stripct", "Bicak dahil silme")]
    public void StripCT(CCSPlayerController? player, CommandInfo info)
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
            RemoveWeapons(x, false);
        });
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Green}gardiyanların {ChatColors.Blue}silahlarını {ChatColors.White}sildi.");
    }

    [ConsoleCommand("stripall", "Bicak dahil silme")]
    public void StripAll(CCSPlayerController? player, CommandInfo info)
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
            RemoveWeapons(x, false);
        });
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Green}herkesin {ChatColors.Blue}silahlarını {ChatColors.White}sildi.");
    }

    #endregion Strip
}