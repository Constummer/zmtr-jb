using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private bool KumarKapatDisable = false;

    [ConsoleCommand("kumarkapat")]
    [ConsoleCommand("kumarkapa")]
    public void KumarKapat(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        KumarKapatDisable = true;
        Server.PrintToChatAll($"{Prefix} {CC.R} KUMAR KAPATILDI");
    }

    [ConsoleCommand("kumarac")]
    public void KumarAc(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        KumarKapatDisable = false;
        Server.PrintToChatAll($"{Prefix} {CC.R} KUMAR AÇILDI");
    }
}