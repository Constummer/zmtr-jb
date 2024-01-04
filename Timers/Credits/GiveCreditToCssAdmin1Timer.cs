using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Timers;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region GiveCreditToAdmin1Timer

    public void GiveCreditToCssAdmin1Timer()
    {
        AddTimer(300f, () =>
        {
            GetPlayers()
            .ToList()
            .ForEach(x =>
              {
                  if (AdminManager.PlayerHasPermissions(x, "@css/adminkredi"))
                  {
                      if (x?.SteamID != null && x!.SteamID != 0)
                      {
                          if (PlayerMarketModels.TryGetValue(x.SteamID, out var item))
                          {
                              item.Credit += Config.Credit.RetrieveCreditEvery5MinRewardCssAdmin1;
                          }
                          else
                          {
                              item = new(x.SteamID);
                              item.Credit = Config.Credit.RetrieveCreditEvery5MinRewardCssAdmin1;
                          }
                          PlayerMarketModels[x.SteamID] = item;
                          x.PrintToChat($" {CC.LR}[ZMTR] {CC.R}Admin{CC.W} olduğun için {CC.LB}{Config.Credit.RetrieveCreditEvery5MinRewardCssAdmin1} {CC.W}kredi kazandın!");
                      }
                  }
              });
        }, TimerFlags.REPEAT);
    }

    #endregion GiveCreditToAdmin1Timer
}