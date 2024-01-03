using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Gravity

    [ConsoleCommand("gravityac")]
    public void GravityAc(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye28"))
        {
            player.PrintToChat($" {CC.LR}[ZMTR]{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        Server.ExecuteCommand("sv_gravity 150");
        Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.W}Gravity açıldı.");
    }

    [ConsoleCommand("gravitykapa")]
    [ConsoleCommand("gravitykapat")]
    public void GravityKapat(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye28"))
        {
            player.PrintToChat($" {CC.LR}[ZMTR]{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        Server.ExecuteCommand("sv_gravity 800");
        Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.W}Gravity kapandı.");
    }

    #endregion Gravity
}