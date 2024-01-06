using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region TopKomutcu

    [ConsoleCommand("topwarden")]
    [ConsoleCommand("topkomutcu")]
    [ConsoleCommand("topkom")]
    [ConsoleCommand("topk")]
    [ConsoleCommand("topw")]
    public void TopKomutcu(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        var ordered = PlayerTimeTracking.Where(x => PlayerNamesDatas.ContainsKey(x.Key))
                                        .OrderByDescending(x => x.Value.WTime)
                                        .Take(10);

        player.PrintToChat($"{Prefix} {CC.W} ------===------------===------");
        player.PrintToChat($"{Prefix} {CC.W} TOP 10 Komutçu Süreler");
        foreach (var item in ordered)
        {
            if (PlayerNamesDatas.TryGetValue(item.Key, out var name))
            {
                player.PrintToChat($"{Prefix} {CC.G}{name} {CC.W}| {CC.B}{(item.Value.Total / 60)} {CC.Ol}Saat");
            }
        }
        player.PrintToChat($"{Prefix} {CC.B}!surem {CC.W}yazarak kendi süreni görebilirsin");
        player.PrintToChat($"{Prefix} {CC.W} ------===------------===------");
    }

    #endregion TopKomutcu
}