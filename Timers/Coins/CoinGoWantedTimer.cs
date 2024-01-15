using CounterStrikeSharp.API.Modules.Timers;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region CoinGoWantedTimer

    public void CoinGoWantedTimer()
    {
        AddTimer(0.2f, () =>
         {
             if (CoinGoWanted)
             {
                 CoinGoWanted = false;
                 AddTimer(3f, () =>
                 {
                     CoinGo = true;
                 }, SOM);
             }
         }, Full);
    }

    #endregion CoinGoWantedTimer
}