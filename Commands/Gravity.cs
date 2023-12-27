using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Gravity

    [ConsoleCommand("gravityac")]
    public void GravityAc(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye28"))
        {
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR]{ChatColors.White} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        Server.ExecuteCommand("sv_gravity 150");
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.White}Gravity açıldı.");
    }

    [ConsoleCommand("gravitykapa")]
    [ConsoleCommand("gravitykapat")]
    public void GravityKapat(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye28"))
        {
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR]{ChatColors.White} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        Server.ExecuteCommand("sv_gravity 800");
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.White}Gravity kapandı.");
    }

    #endregion Gravity
}