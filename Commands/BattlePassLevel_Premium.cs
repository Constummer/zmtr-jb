using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("bpplevelatla")]
    [ConsoleCommand("battlepassplevelatla")]
    [ConsoleCommand("battlepplevelatla")]
    [ConsoleCommand("bpassplevelatla")]
    [ConsoleCommand("bppremiumlevelatla")]
    [ConsoleCommand("battlepasspremiumlevelatla")]
    [ConsoleCommand("battleppremiumlevelatla")]
    [ConsoleCommand("bpasspremiumlevelatla")]
    public void BattlePassPremiumLevelAtla(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false) return;
        //if (!Config.Additional.BattlePassPremiumActive) return;

        if (!AdminManager.PlayerHasPermissions(player, "@css/premium"))
        {
            player.PrintToChat($"{Prefix} {CC.W}Bu menu sadece {CC.M}PREMIUM{CC.W}'lara özeldir.");
            return;
        }
        if (BattlePassPremiumDatas.TryGetValue(player.SteamID, out var data))
        {
            if (!data.Completed)
            {
                player.PrintToChat($"{Prefix} {CC.W}Battle Pass Premium - {data.Level} levelini");
                player.PrintToChat($"{Prefix} {CC.R}Tamamlamamışsın");
                player.PrintToChat($"{Prefix} {CC.B}!bpp{CC.W} yazarak");
                player.PrintToChat($"{Prefix} {CC.W}yapman gereken {data.Level} level görevleri görebilirsin.");
            }
            else
            {
                player.PrintToChat($"{Prefix} {CC.W}Battle Pass Premium - {data.Level} levelini");
                player.PrintToChat($"{Prefix} {CC.G}Tamamlamışsın. Tebrikler.");
                player.PrintToChat($"{Prefix} {CC.B}!bpp{CC.W} yazarak");
                player.PrintToChat($"{Prefix} {CC.W}yapman gereken {data.Level + 1} level görevleri görebilirsin.");
                BattlePassPremiumBase.GiveReward(data, player);
            }
        }
    }
}