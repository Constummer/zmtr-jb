using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private CounterStrikeSharp.API.Modules.Timers.Timer SinirsizMolyTimer = null;

    #region SinirsizBomba

    [ConsoleCommand("sinirsizmolotof")]
    [ConsoleCommand("smmolotof")]
    [CommandHelper(minArgs: 1, usage: "<0/1>")]
    public void SinirsizMoly(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        if (info.ArgCount < 2) return;
        var oneTwoStr = info.ArgCount == 2 ? info.GetArg(1) : "0";
        int.TryParse(oneTwoStr, out var oneTwo);
        if (oneTwo < 0 || oneTwo > 1)
        {
            player.PrintToChat($"{Prefix}{CC.W} 0 = kapatmak icin, 1 = acmak icin.");
            return;
        }
        SinirsizMolyTimer = GiveSinirsizCustomNade(oneTwo, SinirsizMolyTimer, "weapon_incgrenade");
    }

    #endregion SinirsizBomba
}