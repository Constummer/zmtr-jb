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
        var datas = LastGambleDatas.ToList().OrderByDescending(x => x.Key).Take(20);
        var first = datas.FirstOrDefault();
        var others = datas.Skip(1).ToList();
        player.PrintToChat($"{Prefix} {CC.W} =>[{first.Value.Winner.GetShort()}{CC.W}] {string.Join(", ", others.Select(x => x.Value.Winner.GetShort()))}");
    }
}