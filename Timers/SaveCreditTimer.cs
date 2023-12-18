using CounterStrikeSharp.API.Modules.Timers;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region GiveCreditTimer

    public void SaveCreditTimer()
    {
        AddTimer(Config.SaveCreditTimerEveryXSecond, () =>
        {
            UpdateAllModels();
        }, TimerFlags.REPEAT);
    }

    #endregion GiveCreditTimer
}