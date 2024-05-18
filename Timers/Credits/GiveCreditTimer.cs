using CounterStrikeSharp.API.Modules.Admin;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region GiveCreditTimer

    public CounterStrikeSharp.API.Modules.Timers.Timer GiveCreditTimer()
    {
        return AddTimer(300f, () =>
        {
            GetPlayers()
                   .ToList()
                   .ForEach(x =>
                   {
                       if (ValidateCallerPlayer(x, false) == false) return;
                       if (x?.SteamID != null && x!.SteamID != 0)
                       {
                           if (PlayerMarketModels.TryGetValue(x.SteamID, out var item))
                           {
                               item.Credit += Config.Credit.RetrieveCreditEveryXMinReward * CreditModifier;
                           }
                           else
                           {
                               item = new(x.SteamID);
                               item.Credit = Config.Credit.RetrieveCreditEveryXMinReward * CreditModifier;
                           }
                           if (ValidateCallerPlayer(x, false) == false) return;
                           x.PrintToChat($"{Prefix} {CC.W}Bu sunucuda {CC.G}5 {CC.W}dakika zaman geçirdiğin için {CC.LB}{Config.Credit.RetrieveCreditEveryXMinReward * CreditModifier} {CC.W}kredi kazandın!");
                           if (AdminManager.PlayerHasPermissions(x, "@css/adminkredi"))
                           {
                               item.Credit += Config.Credit.RetrieveCreditEvery5MinRewardCssAdmin1 * CreditModifier;
                               x.PrintToChat($"{Prefix} {CC.R}Admin{CC.W} olduğun için {CC.LB}{Config.Credit.RetrieveCreditEvery5MinRewardCssAdmin1 * CreditModifier} {CC.W}kredi kazandın!");
                           }
                           if (AdminManager.PlayerHasPermissions(x, "@css/liderkredi"))
                           {
                               item.Credit += Config.Credit.RetrieveCreditEvery5MinRewardCssLider * CreditModifier;
                               x.PrintToChat($"{Prefix} {CC.R}Lider{CC.W} olduğun için {CC.LB}{Config.Credit.RetrieveCreditEvery5MinRewardCssLider * CreditModifier} {CC.W}kredi kazandın!");
                           }
                           if (AdminManager.PlayerHasPermissions(x, Perm_Premium))
                           {
                               item.Credit += Config.Credit.RetrieveCreditEvery5MinRewardCssPremium * CreditModifier;
                               x.PrintToChat($"{Prefix} {CC.R}Premium{CC.W} olduğun için {CC.LB}{Config.Credit.RetrieveCreditEvery5MinRewardCssPremium * CreditModifier} {CC.W}kredi kazandın!");
                           }
                           if (PlayerSteamGroup.Contains(x.SteamID))
                           {
                               item.Credit += 2 * CreditModifier;
                               x.PrintToChat($"{Prefix} {CC.R}Steam Grubunda{CC.W} olduğun için {CC.LB}{2 * CreditModifier} {CC.W}kredi kazandın!");
                           }
                           PlayerMarketModels[x.SteamID] = item;
                       }
                   });
        }, Full);
    }

    #endregion GiveCreditTimer
}