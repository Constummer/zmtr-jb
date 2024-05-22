using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("taser")]
    [CommandHelper(1, "0.1 - 30 <default 15>")]
    public void OnTaserCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        if (!float.TryParse(info.ArgString.GetArgSkip(0), out var taser) || taser < 0 || taser > 30)
        {
            player.PrintToChat($"{Prefix}{CC.W} 15 default, 0.1 - 30 hizi ayarlamak için.");
            return;
        }
        Server.ExecuteCommand($"mp_taser_recharge_time {taser}");
        LogManagerCommand(player.SteamID, info.GetCommandString);
    }
}