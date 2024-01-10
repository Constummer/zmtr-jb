using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using static JailbreakExtras.JailbreakExtras;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region SinirsizX

    [ConsoleCommand("sinirsizxKapa")]
    [ConsoleCommand("smxkapa")]
    [CommandHelper(1, "<playerismi-@all-@t-@ct-@me-@alive-@dead>")]
    public void SinirsizXKapa(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var target = info.ArgCount > 1 ? info.GetArg(1) : null;

        SinirsizXTimer = GiveSinirsizCustomNade(0, SinirsizXTimer, null, target, player.PlayerName);
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}{CC.Ol}{target} {CC.W} hedefinin {CC.DB}SMX{CC.W}'ini kapattı.");
    }

    #endregion SinirsizX
}