using CounterStrikeSharp.API.Modules.Timers;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region GiveCreditToLiderTimer

    public void SaveTimeTrackingDatas()
    {
        AddTimer(60f, () =>
        {
            UpdatePlayerTimeDataBulk();
        }, TimerFlags.REPEAT);
    }

    #endregion GiveCreditToLiderTimer
}