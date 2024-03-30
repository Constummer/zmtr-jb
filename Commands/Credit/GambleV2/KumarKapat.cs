using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("kumarkapat")]
    [ConsoleCommand("kumarkapa")]
    [ConsoleCommand("ruletkapat")]
    [ConsoleCommand("ruletkapa")]
    [ConsoleCommand("piyangokapat")]
    [ConsoleCommand("piyangokapa")]
    public void KumarKapat(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);

        KumarKapatDisable = true;
        Server.PrintToChatAll($"{Prefix} {CC.R} KUMAR KAPATILDI");
    }
}