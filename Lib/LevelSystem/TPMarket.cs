using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private readonly Dictionary<string, TPMarketItem> TPMarketDatas = new()
    {
        {"1000 TP | 5000 Kredi",new (5000, 1000)},
        {"2000 TP | 10000 Kredi",new (10000, 2000)},
        {"5000 TP | 25000 Kredi",new (25000, 5000)},
    };

    public class TPMarketItem
    {
        public TPMarketItem(int creditCost, int tPReward)
        {
            CreditCost = creditCost;
            TPReward = tPReward;
        }

        public int CreditCost { get; set; }
        public int TPReward { get; set; }
    }

    private void TPMarketSelected(CCSPlayerController player, ChatMenuOption option)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (player?.SteamID == null || player!.SteamID == 0)
        {
            return;
        }
        var data = GetPlayerMarketModel(player.SteamID);
        if (data.Model == null) return;

        if (PlayerLevels.TryGetValue(player.SteamID, out _) == false)
        {
            player.PrintToChat($" {CC.LR}[ZMTR] {CC.W}Bu ürünü alabilmek için seviyen yok.");
            player.PrintToChat($" {CC.LR}[ZMTR] {CC.W}Seviye alabilmek için  {CC.DR}!slotol {CC.W},{CC.DR} !seviyeol {CC.W}yazabilirsin!");
            return;
        }

        var marketMenu = new ChatMenu($"TP Market | Krediniz = [{data.Model.Credit}]");
        foreach (var item in TPMarketDatas)
        {
            marketMenu.AddMenuOption(item.Key, (p, i) =>
            {
                if (ValidateCallerPlayer(player, false) == false) return;
                if (player?.SteamID == null || player!.SteamID == 0) return;

                if (data.Model == null
                    || data.Model.Credit < item.Value.CreditCost
                    || data.Model.Credit - item.Value.CreditCost < 0)
                {
                    player.PrintToChat($" {CC.LR}[ZMTR] {CC.W}Yetersiz Bakiye!");
                    return;
                }
                if (PlayerLevels.TryGetValue(player.SteamID, out var level))
                {
                    level.Xp += item.Value.TPReward;
                }
                else
                {
                    player.PrintToChat($" {CC.LR}[ZMTR] {CC.W}Seviyen yok, seviye alabilmek için  {CC.DR}!slotol {CC.W},{CC.DR} !seviyeol {CC.W}yazabilirsin!");
                    return;
                }

                PlayerLevels[player.SteamID] = level;
            });
        }
        ChatMenus.OpenMenu(player, marketMenu);
    }
}