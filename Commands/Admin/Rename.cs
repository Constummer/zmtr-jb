using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region TagKaldir

    [ConsoleCommand("rename")]
    [CommandHelper(1, "<isim>")]
    public void Rename(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        var target = info.ArgString.GetArgSkipFromLast(1);
        var newName = info.ArgString.GetArgLast();
        if (FindSinglePlayer(player, target, out var x) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        var oldname = x.PlayerName;
        x.PlayerName = newName;
        Global?.AddTimer(0.2f, () =>
        {
            if (ValidateCallerPlayer(x, false) == false) return;
            Utilities.SetStateChanged(x, "CCSPlayerController", "m_iszPlayerName");
        }, SOM);
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}{CC.B}{oldname}{CC.W} Adlı oyuncunun ismini {CC.B}{newName} {CC.W}yaptı.");
    }

    #endregion TagKaldir
}