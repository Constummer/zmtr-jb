using CounterStrikeSharp.API.Modules.Admin;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region GiveCreditToAdmin1Timer

    public CounterStrikeSharp.API.Modules.Timers.Timer GiveCreditToCssAdmin1Timer()
    {
        return AddTimer(300f, () =>
        {
            GetPlayers()
            .ToList()
            .ForEach(x =>
              {
                  if (ValidateCallerPlayer(x, false) == false) return;
                  if (AdminManager.PlayerHasPermissions(x, "@css/adminkredi"))
                  {
                      if (x?.SteamID != null && x!.SteamID != 0)
                      {
                          if (PlayerMarketModels.TryGetValue(x.SteamID, out var item))
                          {
                              item.Credit += Config.Credit.RetrieveCreditEvery5MinRewardCssAdmin1 * CreditModifier;
                          }
                          else
                          {
                              item = new(x.SteamID);
                              item.Credit = Config.Credit.RetrieveCreditEvery5MinRewardCssAdmin1 * CreditModifier;
                          }
                          PlayerMarketModels[x.SteamID] = item;
                          if (ValidateCallerPlayer(x, false) == false) return;
                          x.PrintToChat($"{Prefix} {CC.R}Admin{CC.W} olduğun için {CC.LB}{Config.Credit.RetrieveCreditEvery5MinRewardCssAdmin1 * CreditModifier} {CC.W}kredi kazandın!");
                      }
                  }
              });
        }, Full);
    }

    #endregion GiveCreditToAdmin1Timer
}