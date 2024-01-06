using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
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
            player.PrintToChat($"{Prefix} {CC.W}Bu ürünü alabilmek için seviyen yok.");
            player.PrintToChat($"{Prefix} {CC.W}Seviye alabilmek için  {CC.DR}!slotol {CC.W},{CC.DR} !seviyeol {CC.W}yazabilirsin!");
            return;
        }

        var marketMenu = new ChatMenu($"TP Market | Krediniz = [{data.Model.Credit}]");
        foreach (var item in Config.Credit.TPMarketDatas)
        {
            marketMenu.AddMenuOption(item.Key, (p, i) =>
            {
                if (ValidateCallerPlayer(player, false) == false) return;
                if (player?.SteamID == null || player!.SteamID == 0) return;

                if (data.Model == null
                    || data.Model.Credit < item.Value.CreditCost
                    || data.Model.Credit - item.Value.CreditCost < 0)
                {
                    player.PrintToChat($"{Prefix} {CC.W}Yetersiz Bakiye!");
                    return;
                }
                if (PlayerLevels.TryGetValue(player.SteamID, out var level))
                {
                    data.Model.Credit -= item.Value.CreditCost;
                    PlayerMarketModels[player.SteamID] = data.Model;

                    level.Xp += item.Value.TPReward;
                    PlayerLevels[player.SteamID] = level;
                    player.PrintToChat($"{Prefix} {CC.B}{item.Value.CreditCost} {CC.W}Kredi Karşılığında {CC.B}{item.Value.TPReward} {CC.W}TP Satın Aldın!");
                    player.PrintToChat($"{Prefix} {CC.W}Mevcut Kredin = {CC.B}{data.Model.Credit}{CC.R} |{CC.W} Mevcut TP = {level.Xp}");
                }
                else
                {
                    player.PrintToChat($"{Prefix} {CC.W}Seviyen yok, seviye alabilmek için  {CC.DR}!slotol {CC.W},{CC.DR} !seviyeol {CC.W}yazabilirsin!");
                    return;
                }
            });
        }
        ChatMenus.OpenMenu(player, marketMenu);
    }
}