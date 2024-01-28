using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("haftalikg")]
    [ConsoleCommand("haftalikgardiyan")]
    [ConsoleCommand("haftalikct")]
    [ConsoleCommand("haftact")]
    [ConsoleCommand("haftagardiyan")]
    [ConsoleCommand("haftag")]
    public void HaftalikGardiyan(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        var ordered = AllPlayerWeeklyCTTimeTracking.OrderByDescending(x => x.Value);

        player.PrintToChat($"{Prefix} {CC.W} ------===------------===------");
        player.PrintToChat($"{Prefix} {CC.W} TOP 10 Haftalık Gardiyan Süreler");
        foreach (var item in ordered)
        {
            if (PlayerNamesDatas.TryGetValue(item.Key, out var name))
            {
                var tempName = name;
                if (tempName?.Length > 20)
                {
                    tempName = tempName.Substring(0, 17) + "...";
                }
                tempName = tempName?.PadRight(20, '_');
                player.PrintToChat($"{Prefix} {CC.G}{tempName} {CC.W}| {CC.B}{(item.Value / 60)} {CC.Ol}Saat");
            }
        }
        player.PrintToChat($"{Prefix} {CC.B}!surem {CC.W}yazarak kendi süreni görebilirsin");
        player.PrintToChat($"{Prefix} {CC.W} ------===------------===------");
    }
}