using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region OnTeamCommand

    [ConsoleCommand("swap")]
    [CommandHelper(0, "<nick-#userid-@me>")]
    public void OnSwapCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye6") == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var swap1res = SwapTargetOne(info.ArgString.GetArg(0), player);
        if (swap1res.Res)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{swap1res.PName} {CC.W} swapladı.");
        }
    }

    #endregion OnTeamCommand
}