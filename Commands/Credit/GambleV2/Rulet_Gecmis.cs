using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("ruletgecmis")]
    [ConsoleCommand("rg")]
    [ConsoleCommand("ruletg")]
    [ConsoleCommand("rgecmis")]
    public void RuletGecmis(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        var first = LatestRuletWinner;
        var others = GambleLast70HistoryData.ToList();
        if (first == RuletOptions.None)
        {
            first = GambleLast70HistoryData.FirstOrDefault();
            others = GambleLast70HistoryData.Skip(1).ToList();
        }
        player.PrintToChat($"{Prefix} {CC.W} =>[{first.GetShort()}{CC.W}] {string.Join(" ", others.Select(x => x.GetShort()))}");
    }
}