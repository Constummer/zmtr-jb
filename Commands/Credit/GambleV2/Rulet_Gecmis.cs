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
        player.PrintToChat($"{Prefix} {CC.W} {(string.Join(" ", datas.Select(x => x.Value.Winner.GetShort())))}");
    }
}