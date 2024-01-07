using CounterStrikeSharp.API.Modules.Timers;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region UpdateAllTimeTrackingDatas

    public void UpdateAllTimeTrackingDatas()
    {
        AddTimer(60 * 15, () =>
        {
            UpdateAllTimeTrackingData();
        }, TimerFlags.REPEAT);
    }

    #endregion UpdateAllTimeTrackingDatas
}