using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Disarm

    [ConsoleCommand("disarm", "Bicak dahil silme")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me>")]
    public void Disarm(CCSPlayerController? player, CommandInfo info)
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
            RemoveWeapons(x, true);
        });
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.White}{player!.PlayerName} adlı oyuncunun silahları silindi.");
    }

    [ConsoleCommand("disarmt", "Bicak dahil silme")]
    public void DisarmT(CCSPlayerController? player, CommandInfo info)
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
            RemoveWeapons(x, true);
        });
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.White}Mahkûmların silahları silindi.");
    }

    [ConsoleCommand("disarmct", "Bicak dahil silme")]
    public void DisarmCT(CCSPlayerController? player, CommandInfo info)
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
            RemoveWeapons(x, true);
        });
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.White}Gardiyanların silahları silindi.");
    }

    [ConsoleCommand("disarmall", "Bicak dahil silme")]
    public void DisarmAll(CCSPlayerController? player, CommandInfo info)
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
            RemoveWeapons(x, true);
        });
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.White}Herkesin silahları silindi.");
    }

    #endregion Disarm
}