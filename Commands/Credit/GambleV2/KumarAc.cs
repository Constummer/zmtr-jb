using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private bool KumarKapatDisable = false;

    [ConsoleCommand("kumarac")]
    [ConsoleCommand("ruletac")]
    [ConsoleCommand("piyangoac")]
    public void KumarAc(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);

        KumarKapatDisable = false;
        Server.PrintToChatAll($"{Prefix} {CC.R} KUMAR AÇILDI");
    }
}