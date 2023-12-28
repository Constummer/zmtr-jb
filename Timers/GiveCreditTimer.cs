using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region GiveCreditTimer

    public void GiveCreditTimer()
    {
        AddTimer(Config.RetrieveCreditEveryXMin, () =>
        {
            GetPlayers()
                   .ToList()
                   .ForEach(x =>
                   {
                       if (x?.SteamID != null && x!.SteamID != 0)
                       {
                           if (PlayerMarketModels.TryGetValue(x.SteamID, out var item))
                           {
                               item.Credit += Config.RetrieveCreditEveryXMinReward;
                           }
                           else
                           {
                               item = new(x.SteamID);
                               item.Credit = Config.RetrieveCreditEveryXMinReward;
                           }
                           PlayerMarketModels[x.SteamID] = item;
                           x.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.White}Bu sunucuda {ChatColors.Green}{Config.RetrieveCreditEveryXMin / 60} {ChatColors.White}dakika zaman geçirdiğin için {ChatColors.LightBlue}{Config.RetrieveCreditEveryXMinReward} {ChatColors.White}kredi kazandın!");
                       }
                   });
        }, TimerFlags.REPEAT);
    }

    #endregion GiveCreditTimer
}