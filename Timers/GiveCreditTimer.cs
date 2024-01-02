using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region GiveCreditTimer

    public void GiveCreditTimer()
    {
        AddTimer(Config.Credit.RetrieveCreditEveryXMin, () =>
        {
            GetPlayers()
                   .ToList()
                   .ForEach(x =>
                   {
                       if (x?.SteamID != null && x!.SteamID != 0)
                       {
                           if (PlayerMarketModels.TryGetValue(x.SteamID, out var item))
                           {
                               item.Credit += Config.Credit.RetrieveCreditEveryXMinReward;
                           }
                           else
                           {
                               item = new(x.SteamID);
                               item.Credit = Config.Credit.RetrieveCreditEveryXMinReward;
                           }
                           PlayerMarketModels[x.SteamID] = item;
                           x.PrintToChat($" {CC.LR}[ZMTR] {CC.W}Bu sunucuda {CC.G}{Config.Credit.RetrieveCreditEveryXMin / 60} {CC.W}dakika zaman geçirdiğin için {CC.LB}{Config.Credit.RetrieveCreditEveryXMinReward} {CC.W}kredi kazandın!");
                       }
                   });
        }, TimerFlags.REPEAT);
    }

    #endregion GiveCreditTimer
}