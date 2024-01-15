using CounterStrikeSharp.API.Modules.Timers;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region SaveCreditTimer

    public void SaveCreditTimer()
    {
        AddTimer(Config.Credit.SaveCreditTimerEveryXSecond, () =>
        {
            UpdateAllModels();
        }, Full);
    }

    #endregion SaveCreditTimer
}