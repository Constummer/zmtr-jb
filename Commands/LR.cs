using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;
using System.Collections.Generic;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region lr

    [ConsoleCommand("lr")]
    public void OnLrCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (player.PawnIsAlive == false)
        {
            player.PrintToChat($"{Prefix} {CC.W}Sadece yaşayan son mahkûm lr atabilir.");
            return;
        }
        if (GetTeam(player) != CsTeam.Terrorist)
        {
            player.PrintToChat($"{Prefix} {CC.W}Sadece yaşayan son mahkûm lr atabilir.");
            return;
        }
        if (GetPlayerCount(CsTeam.Terrorist, true) != 1)
        {
            player.PrintToChat($"{Prefix} {CC.W}Sadece yaşayan son mahkûm lr atabilir.");
            return;
        }
        player.PrintToChat($"{Prefix} {CC.W}lr devre disi - simdilik -.");
        return;
        var lrMenu = new ChatMenu("LR Menu");
        lrMenu.AddMenuOption("Deagle", (p, i) =>
        {
        });

        ChatMenus.OpenMenu(player, lrMenu);
    }

    #endregion lr
}