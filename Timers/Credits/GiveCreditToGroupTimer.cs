namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region GiveCreditToAdmin1Timer

    public CounterStrikeSharp.API.Modules.Timers.Timer GiveCreditToGroupTimer()
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
                      if (PlayerSteamGroup.Contains(x.SteamID))
                      {
                          if (PlayerMarketModels.TryGetValue(x.SteamID, out var item))
                          {
                              item.Credit += 2 * CreditModifier;
                          }
                          else
                          {
                              item = new(x.SteamID);
                              item.Credit = 2 * CreditModifier;
                          }
                          PlayerMarketModels[x.SteamID] = item;
                          if (ValidateCallerPlayer(x, false) == false) return;
                          x.PrintToChat($"{Prefix} {CC.R}Steam Grubunda{CC.W} olduğun için {CC.LB}{2 * CreditModifier} {CC.W}kredi kazandın!");
                      }
                  }
              });
        }, Full);
    }

    #endregion GiveCreditToAdmin1Timer
}