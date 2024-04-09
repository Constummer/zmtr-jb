using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("csay")]
    public void csay(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/sorumlu"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);

        Server.PrintToChatAll($"{player.PlayerName}: {info.ArgString}");
    }

    [ConsoleCommand("hsay")]
    public void hsay(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/sorumlu"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);

        foreach (var item in GetPlayers().ToList())
        {
            item.PrintToCenter(info.ArgString);
        }
    }

    [ConsoleCommand("hsay2")]
    public void hsay2(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/sorumlu"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);

        foreach (var item in GetPlayers().ToList())
        {
            PrintToCenterHtml(item, info.ArgString);
        }
    }

    [ConsoleCommand("hsay3")]
    public void hsay3(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/sorumlu"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);

        foreach (var item in GetPlayers().ToList())
        {
            PrintToCenterHtml(item, info.ArgString);
            item.PrintToCenter(info.ArgString);
        }
    }

    [ConsoleCommand("hsay4")]
    public void hsay4(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/sorumlu"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);

        foreach (var item in GetPlayers().ToList())
        {
            PrintToCenterHtml(item, info.ArgString);
            item.PrintToCenter(info.ArgString);
            item.PrintToCenter(info.ArgString);
        }
    }
}