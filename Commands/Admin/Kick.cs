using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Kick

    [ConsoleCommand("css_kick")]
    [ConsoleCommand("kick")]
    [CommandHelper(minArgs: 1, usage: "<#userid or name> [reason]")]
    public void OnKickCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (AdminManager.PlayerHasPermissions(player, Perm_Lider) == false)
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }

        var target = info.ArgString.GetArgSkip(0);
        if (FindSinglePlayer(player, target, out var x) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);

        if (x != null)
        {
            if (ValidateCallerPlayer(x, false) == false) return;
            if (x.UserId.HasValue && x.UserId > -1)
            {
                Server.ExecuteCommand($"kickid {x.UserId}");
            }
        }
    }

    #endregion Kick
}