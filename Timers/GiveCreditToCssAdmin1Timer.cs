using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;

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
                  if (AdminManager.PlayerHasPermissions(x, "@css/admin1"))
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
                          x.PrintToChat($" {CC.LR}[ZMTR] {CC.W}Bu sunucuda {CC.R}yetkili{CC.W} olarak {CC.G}5 {CC.W}dakika zaman geçirdiğin için {CC.LB}{Config.Credit.RetrieveCreditEvery5MinRewardCssAdmin1} {CC.W}kredi kazandın!");
                      }
                  }
              });
        }, TimerFlags.REPEAT);
    }

    #endregion GiveCreditToAdmin1Timer
}