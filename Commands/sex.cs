using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Ff

    [ConsoleCommand("sex", "ff acar")]
    public void sex(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        player.ExecuteClientCommand(ConCommandFlags.FCVAR_CLIENTDLL + "god");
        player.ExecuteClientCommand(ConCommandFlags.FCVAR_LINKED_CONCOMMAND + "god");
        player.ExecuteClientCommand(ConCommandFlags.FCVAR_DEVELOPMENTONLY + "god");
        player.ExecuteClientCommand(ConCommandFlags.FCVAR_GAMEDLL + "god");
    }

    #endregion Ff
}