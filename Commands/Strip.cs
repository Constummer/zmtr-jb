using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
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
        var target = info.ArgString.GetArgSkip(0);
        var targetArgument = GetTargetArgument(target);

        LogManagerCommand(player.SteamID, info.GetCommandString);
        GetPlayers()
        .Where(x => x.PawnIsAlive
                    && GetTargetAction(x, target, player))
        .ToList()
        .ForEach(x =>
        {
            RemoveWeapons(x, false);
            if ((targetArgument & TargetForArgument.SingleUser) == targetArgument)
            {
                Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncunun {CC.B}silahlarını {CC.W}sildi.");
            }
        });
        if ((targetArgument & TargetForArgument.SingleUser) != targetArgument)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefinin {CC.B}silahlarını {CC.W}sildi.");
        }
    }

    [ConsoleCommand("stript", "Bicak dahil silme")]
    public void StripT(CCSPlayerController? player, CommandInfo info)
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
            RemoveWeapons(x, false);
        });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{T_PluralCamelPossesive} {CC.B}silahlarını {CC.W}sildi.");
    }

    [ConsoleCommand("stripct", "Bicak dahil silme")]
    public void StripCT(CCSPlayerController? player, CommandInfo info)
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
            RemoveWeapons(x, false);
        });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{CT_PluralCamelPossesive} {CC.B}silahlarını {CC.W}sildi.");
    }

    [ConsoleCommand("stripall", "Bicak dahil silme")]
    public void StripAll(CCSPlayerController? player, CommandInfo info)
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
            RemoveWeapons(x, false);
        });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}herkesin {CC.B}silahlarını {CC.W}sildi.");
    }

    #endregion Strip
}