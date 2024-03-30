namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region UpdateAllTimeTrackingDatas

    public CounterStrikeSharp.API.Modules.Timers.Timer UpdateAllTimeTrackingDatasTimer()
    {
        return AddTimer(60 * 15, () =>
        {
            UpdateAllTimeTrackingData();
        }, Full);
    }

    #endregion UpdateAllTimeTrackingDatas
}