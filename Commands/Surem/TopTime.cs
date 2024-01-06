using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region TopTime

    [ConsoleCommand("toptime")]
    [ConsoleCommand("topsure")]
    public void TopTime(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        var ordered = PlayerTimeTracking.Where(x => PlayerNamesDatas.ContainsKey(x.Key))
                                        .OrderByDescending(x => x.Value.Total)
                                        .Take(10);

        player.PrintToChat($"{Prefix} {CC.W} TOP 10 Süreler");
        foreach (var item in ordered)
        {
            if (PlayerNamesDatas.TryGetValue(item.Key, out var name))
            {
                player.PrintToChat($"{Prefix} {CC.R}{name} {CC.W}| {CC.B}{(item.Value.Total / 60)} {CC.Ol}Saat");
            }
        }
    }

    #endregion TopTime
}