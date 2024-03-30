using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("ruletgecmis")]
    public void RuletGecmis(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        var first = GambleLast70HistoryData.FirstOrDefault();
        var others = GambleLast70HistoryData.Skip(1).ToList();
        player.PrintToChat($"{Prefix} {CC.W} =>[{first.GetShort()}{CC.W}] {string.Join(" ", others.Select(x => x.GetShort()))}");
    }
}