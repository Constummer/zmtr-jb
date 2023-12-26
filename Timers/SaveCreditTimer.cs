using CounterStrikeSharp.API.Modules.Timers;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region SaveCreditTimer

    public void SaveCreditTimer()
    {
        AddTimer(Config.SaveCreditTimerEveryXSecond, () =>
        {
            UpdateAllModels();
        }, TimerFlags.REPEAT);
    }

    #endregion SaveCreditTimer
}