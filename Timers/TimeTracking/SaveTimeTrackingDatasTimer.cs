namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region SaveTimeTrackingDatas

    public CounterStrikeSharp.API.Modules.Timers.Timer SaveTimeTrackingDatasTimer()
    {
        return AddTimer(60 * 15f, () =>
        {
            UpdatePlayerTimeDataBulk();
        }, Full);
    }

    #endregion SaveTimeTrackingDatas
}