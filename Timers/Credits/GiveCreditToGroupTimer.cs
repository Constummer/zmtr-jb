using CounterStrikeSharp.API.Modules.Timers;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region GiveCreditToAdmin1Timer

    public void GiveCreditToGroupTimer()
    {
        AddTimer(300f, () =>
        {
            GetPlayers()
            .ToList()
            .ForEach(x =>
              {
                  if (x?.SteamID != null && x!.SteamID != 0)
                  {
                      if (PlayerSteamGroup.Contains(x.SteamID))
                      {
                          if (PlayerMarketModels.TryGetValue(x.SteamID, out var item))
                          {
                              item.Credit += 2;
                          }
                          else
                          {
                              item = new(x.SteamID);
                              item.Credit = 2;
                          }
                          PlayerMarketModels[x.SteamID] = item;
                          x.PrintToChat($"{Prefix} {CC.R}Steam Grubunda{CC.W} olduğun için {CC.LB}2 {CC.W}kredi kazandın!");
                      }
                  }
              });
        }, TimerFlags.REPEAT);
    }

    #endregion GiveCreditToAdmin1Timer
}