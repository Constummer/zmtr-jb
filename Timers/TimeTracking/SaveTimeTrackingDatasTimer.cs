namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region SaveTimeTrackingDatas

    public CounterStrikeSharp.API.Modules.Timers.Timer SaveTimeTrackingDatasTimer()
    {
        var multipler = 15;
        return AddTimer(60 * multipler, () =>
        {
            UpdatePlayerTimeDataBulk(multipler);
        }, Full);
    }

    #endregion SaveTimeTrackingDatas
}