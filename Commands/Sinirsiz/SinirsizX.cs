﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private CounterStrikeSharp.API.Modules.Timers.Timer SinirsizXTimer = null;

    #region SinirsizX

    [ConsoleCommand("sinirsizxac")]
    [ConsoleCommand("smxac")]
    [ConsoleCommand("smx")]
    [CommandHelper(1, "<playerismi-@all-@t-@ct-@me-@alive-@dead> [weapon]")]
    public void SinirsizX(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var target = info.ArgCount > 1 ? info.ArgString.GetArg(0) : null;
        LogManagerCommand(player.SteamID, info.GetCommandString);
        if (info.ArgCount > 2)
        {
            var weapon = info.ArgCount > 2 ? info.ArgString.GetArg(1) : null;
            if (string.IsNullOrWhiteSpace(weapon))
            {
                player.PrintToChat($"{Prefix} {CC.G} Silah ismini vermeniz gerekmektedir..");
                player.PrintToChat($"{Prefix} {CC.G} Örnek = !sinirsizx @all ssg08.");
                player.PrintToChat($"{Prefix} {CC.G} Örnek = !smx @all ssg08.");
                return;
            }
            else
            {
                SinirsizXAction(player, target, weapon);
                Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}{CC.Ol}{target}{CC.W} hedefine {CC.P}{weapon} {CC.W} silahıyla {CC.DB}SMX{CC.W} başlattı.");
            }
        }
        else
        {
            player.PrintToChat($"{Prefix} {CC.G} Silah ismini vermeniz gerekmektedir..");
            player.PrintToChat($"{Prefix} {CC.G} Örnek = !sinirsizx @all ssg08.");
            player.PrintToChat($"{Prefix} {CC.G} Örnek = !smx @all ssg08.");
            return;
        }
    }

    public void SinirsizXAction(CCSPlayerController? self, string? target, string? weapon)
    {
        SinirsizXTimer?.Kill();
        SinirsizXTimer = null;
        SinirsizXTimer = GiveSinirsizCustomNade(1, SinirsizXTimer, $"weapon_{weapon}", target, self);
    }

    #endregion SinirsizX
}