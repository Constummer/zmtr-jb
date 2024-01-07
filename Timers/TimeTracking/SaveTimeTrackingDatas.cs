using CounterStrikeSharp.API.Modules.Timers;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region SaveTimeTrackingDatas

    public void SaveTimeTrackingDatas()
    {
        AddTimer(60f, () =>
        {
            UpdatePlayerTimeDataBulk();
        }, TimerFlags.REPEAT);
    }

    #endregion SaveTimeTrackingDatas
}