using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using System.Reflection;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("callMethods")]
    public void callMethods(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var eventHandlers = Global.GetType()
             .GetMethods()
             .Where(method => method.GetCustomAttributes<ConsoleCommandAttribute>().Any())
             .ToArray();
        //Global.CommandManager.
        player.PrintToConsole("%%%%%%%%%%%%%%%%%");
        foreach (var eventHandler in eventHandlers)
        {
            var attributes = eventHandler.GetCustomAttributes<ConsoleCommandAttribute>();
            foreach (var commandInfo in attributes)
            {
                player.PrintToConsole("-----------");
                player.PrintToConsole("Name: " + commandInfo.Command);
                player.PrintToConsole("Description: " + commandInfo.Description);
            }
            player.PrintToConsole("#######################");
        }
        player.PrintToConsole("%%%%%%%%%%%%%%%%%");
    }

    [ConsoleCommand("callMethods2")]
    public void callMethods2(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var eventHandlers = Global.GetType()
             .GetMethods()
             .Where(method => method.GetCustomAttributes<ConsoleCommandAttribute>().Any())
             .ToArray();

        foreach (var eventHandler in eventHandlers)
        {
            var attributes = eventHandler.GetCustomAttributes<ConsoleCommandAttribute>();
            foreach (var commandInfo in attributes)
            {
                if (commandInfo.Command == "callMethods3")
                {
                    Server.PrintToChatAll("1");
                    // Ensure we are invoking the method on the correct instance
                    var targetInstance = Global; // Replace Global with the correct instance if needed
                    Server.PrintToChatAll("2");

                    // Invoke the method on the targetInstance, not on the player
                    eventHandler.Invoke(targetInstance, new object[]
                    {
                    player,  // First parameter: CCSPlayerController player
                    info     // Second parameter: CommandInfo info
                    });
                    Server.PrintToChatAll("3");
                }
            }
        }
    }

    [ConsoleCommand("callMethods3")]
    public void callMethods3(CCSPlayerController? player, CommandInfo info)
    {
        Server.PrintToChatAll("4");
        player.PrintToConsole(info.ToString());
        player.PrintToConsole($"{info.ArgCount}");
        player.PrintToConsole($"ArgString=> {info.ArgString}");
        player.PrintToConsole($"{info.CallingPlayer}");
        player.PrintToConsole($"{info.CallingContext}");
        player.PrintToConsole($"{info.CallingPlayer.PlayerName}");
        Server.PrintToChatAll("5");

        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        player.PrintToConsole(info.ArgString.GetArg(0));
    }
}