using CounterStrikeSharp.API.Modules.Timers;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region CoinRespawnTimer

    public void CoinRespawnTimer()
    {
        AddTimer(30f, () =>
         {
             CoinSpawn();
         }, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
    }

    #endregion CoinRespawnTimer
}