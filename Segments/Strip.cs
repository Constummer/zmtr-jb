using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Strip

    [ConsoleCommand("strip", "Bicak dahil silme")]
    [CommandHelper(1, "<@t,@ct,@all,oyuncu ismi>")]
    public void Strip(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);

        GetPlayers()
        .Where(x => x.PawnIsAlive
                   && GetTargetAction(x, target))
        .ToList()
        .ForEach(x =>
        {
            RemoveWeapons(x, false);
        });
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
    }

    #endregion Strip
}