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
                           PlayerMarketModels[x.SteamID] = item;
                           if (ValidateCallerPlayer(x, false) == false) return;
                           x.PrintToChat($"{Prefix} {CC.W}Bu sunucuda {CC.G}5 {CC.W}dakika zaman geçirdiğin için {CC.LB}{Config.Credit.RetrieveCreditEveryXMinReward * CreditModifier} {CC.W}kredi kazandın!");
                       }
                   });
        }, Full);
    }

    #endregion GiveCreditTimer
}