using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Bunny

    [ConsoleCommand("bunnyac", "bunny acar")]
    [ConsoleCommand("ba", "bunny acar")]
    public void BunnyAc(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye29"))
        {
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR]{ChatColors.White} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        Server.ExecuteCommand($"sv_enablebunnyhopping 1;sv_autobunnyhopping 1");
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.White}Bunny açıldı.");
    }

    [ConsoleCommand("bk", "bunny kapar")]
    [ConsoleCommand("bunnykapa", "bunny kapar")]
    [ConsoleCommand("bunnykapat", "bunny kapar")]
    public void BunnyKapat(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye29"))
        {
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR]{ChatColors.White} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        Server.ExecuteCommand($"sv_enablebunnyhopping 0;sv_autobunnyhopping 0");
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.White}Bunny kapandı.");
    }

    #endregion Bunny
}