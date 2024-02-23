using CounterStrikeSharp.API.Modules.Admin;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public CounterStrikeSharp.API.Modules.Timers.Timer GiveCreditToCssPremiumTimer()
    {
        return AddTimer(300f, () =>
        {
            GetPlayers()
            .ToList()
            .ForEach(x =>
            {
                if (ValidateCallerPlayer(x, false) == false) return;
                if (AdminManager.PlayerHasPermissions(x, "@css/premium"))
                {
                    if (x?.SteamID != null && x!.SteamID != 0)
                    {
                        if (PlayerMarketModels.TryGetValue(x.SteamID, out var item))
                        {
                            item.Credit += Config.Credit.RetrieveCreditEvery5MinRewardCssPremium * CreditModifier;
                        }
                        else
                        {
                            item = new(x.SteamID);
                            item.Credit = Config.Credit.RetrieveCreditEvery5MinRewardCssPremium * CreditModifier;
                        }
                        PlayerMarketModels[x.SteamID] = item;
                        if (ValidateCallerPlayer(x, false) == false) return;
                        x.PrintToChat($"{Prefix} {CC.R}Premium{CC.W} olduğun için {CC.LB}{Config.Credit.RetrieveCreditEvery5MinRewardCssPremium * CreditModifier} {CC.W}kredi kazandın!");
                    }
                }
            });
        }, Full);
    }
}