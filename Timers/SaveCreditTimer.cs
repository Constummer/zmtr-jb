using CounterStrikeSharp.API.Modules.Timers;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private const float SaveEveryXMin = 1 * 60;//her 1 dk

    #region GiveCreditTimer

    public void SaveCreditTimer()
    {
        AddTimer(SaveEveryXMin, () =>
        {
            UpdateAllModels();
        }, TimerFlags.REPEAT);
    }

    #endregion GiveCreditTimer
}