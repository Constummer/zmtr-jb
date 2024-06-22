namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region CoinGoWantedTimer

    public CounterStrikeSharp.API.Modules.Timers.Timer CoinGoWantedTimer()
    {
        return AddTimer(0.2f, () =>
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