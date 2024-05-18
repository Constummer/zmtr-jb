using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static int TPModifier { get; set; } = 1;

    [ConsoleCommand("2xtpac")]
    public void Command2xTPAc(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        TPModifier = 2;
        Server.PrintToChatAll($"{Prefix}{CC.B} 2x{CC.W} tp kazanımı başlamıştır.");
    }

    [ConsoleCommand("3xtpac")]
    public void Command3xTPAc(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        TPModifier = 3;
        Server.PrintToChatAll($"{Prefix}{CC.B} 3x{CC.W} tp kazanımı başlamıştır.");
    }

    [ConsoleCommand("4xtpac")]
    public void Command4xTPAc(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        TPModifier = 4;
        Server.PrintToChatAll($"{Prefix}{CC.B} 4x{CC.W} tp kazanımı başlamıştır.");
    }

    [ConsoleCommand("xtpkapa")]
    [ConsoleCommand("2xtpkapa")]
    [ConsoleCommand("3xtpkapa")]
    [ConsoleCommand("4xtpkapa")]
    [ConsoleCommand("xtpkapat")]
    [ConsoleCommand("2xtpkapat")]
    [ConsoleCommand("3xtpkapat")]
    [ConsoleCommand("4xtpkapat")]
    public void CommandxTPAc(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        TPModifier = 1;
        Server.PrintToChatAll($"{Prefix}{CC.W} tp kazanımları normale dönmüştür.");
    }
}