using CounterStrikeSharp.API.Modules.Timers;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region CoinRespawnTimer

    public CounterStrikeSharp.API.Modules.Timers.Timer CoinRespawnTimer()
    {
        return AddTimer(30f, () =>
         {
             CoinSpawn();
         }, Full);
    }

    #endregion CoinRespawnTimer
}