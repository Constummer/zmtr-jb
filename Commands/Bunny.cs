using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Bunny

    [ConsoleCommand("ba", "bunny acar")]
    [ConsoleCommand("bunnyac", "bunny acar")]
    public void BunnyAc(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        Server.ExecuteCommand($"sv_enablebunnyhopping 1;sv_autobunnyhopping 1");
        Server.PrintToChatAll("Bunny Açık");
    }

    [ConsoleCommand("bk", "bunny kapar")]
    [ConsoleCommand("bunnykapa", "bunny kapar")]
    [ConsoleCommand("bunnykapat", "bunny kapar")]
    public void BunnyKapat(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        Server.ExecuteCommand($"sv_enablebunnyhopping 0;sv_autobunnyhopping 0");
        Server.PrintToChatAll("Bunny Kapalı");
    }

    #endregion Bunny
}