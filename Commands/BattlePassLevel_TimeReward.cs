using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("trlevelatla")]
    [ConsoleCommand("timerewardlevelatla")]
    [ConsoleCommand("timerlevelatla")]
    [ConsoleCommand("trewardlevelatla")]
    public void TimeRewardLevelAtla(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false) return;
        if (!Config.Additional.TimeRewardActive) return;

        if (TimeRewardDatas.TryGetValue(player.SteamID, out var data))
        {
            if (!data.Completed)
            {
                player.PrintToChat($"{Prefix} {CC.W}Time Reward - {data.Level} levelini");
                player.PrintToChat($"{Prefix} {CC.R}Tamamlamamışsın");
                player.PrintToChat($"{Prefix} {CC.B}!bp{CC.W} yazarak");
                player.PrintToChat($"{Prefix} {CC.W}yapman gereken {data.Level} level görevleri görebilirsin.");
            }
            else
            {
                player.PrintToChat($"{Prefix} {CC.W}Time Reward - {data.Level} levelini");
                player.PrintToChat($"{Prefix} {CC.G}Tamamlamışsın. Tebrikler.");
                if (data.Level == 31)
                {
                    return;
                }
                player.PrintToChat($"{Prefix} {CC.B}!bp{CC.W} yazarak");
                player.PrintToChat($"{Prefix} {CC.W}yapman gereken {data.Level + 1} level görevleri görebilirsin.");
                TimeRewardBase.GiveReward(data, player);
            }
        }
    }
}