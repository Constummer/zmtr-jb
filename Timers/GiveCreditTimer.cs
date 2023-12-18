using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private const float EveryXMin = 5 * 60;//her 5 dk
    private const int EveryXMinReward = 5;//her 5dkda kazanacagi 5 kredi

    #region GiveCreditTimer

    public void GiveCreditTimer()
    {
        AddTimer(EveryXMin, () =>
        {
            GetPlayers()
                   .ToList()
                   .ForEach(x =>
                   {
                       if (x?.SteamID != null && x!.SteamID != 0)
                       {
                           if (PlayerMarketModels.TryGetValue(x.SteamID, out var item))
                           {
                               item.Credit += EveryXMinReward;
                           }
                           else
                           {
                               item = new(x.SteamID);
                               item.Credit = EveryXMinReward;
                           }
                           PlayerMarketModels[x.SteamID] = item;
                           x.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.LightBlue} {EveryXMin / 60} dk oynadigin icin {EveryXMinReward} kredi kazandin!");
                       }
                   });
        }, TimerFlags.REPEAT);
    }

    #endregion GiveCreditTimer
}