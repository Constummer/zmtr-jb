using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("bplevelatla")]
    [ConsoleCommand("battlepasslevelatla")]
    [ConsoleCommand("battleplevelatla")]
    [ConsoleCommand("bpasslevelatla")]
    public void BattlePassLevelAtla(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false) return;
        if (!Config.Additional.BattlePassActive) return;

        if (BattlePassDatas.TryGetValue(player.SteamID, out var data))
        {
            if (!data.Completed)
            {
                player.PrintToChat($"{Prefix} {CC.W}Battle Pass - {data.Level} levelini");
                player.PrintToChat($"{Prefix} {CC.R}Tamamlamamışsın");
                player.PrintToChat($"{Prefix} {CC.B}!bp{CC.W} yazarak");
                player.PrintToChat($"{Prefix} {CC.W}yapman gereken {data.Level} level görevleri görebilirsin.");
            }
            else
            {
                player.PrintToChat($"{Prefix} {CC.W}Battle Pass - {data.Level} levelini");
                player.PrintToChat($"{Prefix} {CC.G}Tamamlamışsın. Tebrikler.");
                player.PrintToChat($"{Prefix} {CC.B}!bp{CC.W} yazarak");
                player.PrintToChat($"{Prefix} {CC.W}yapman gereken {data.Level + 1} level görevleri görebilirsin.");
                BattlePassBase.GiveReward(data, player);
            }
        }
    }
}