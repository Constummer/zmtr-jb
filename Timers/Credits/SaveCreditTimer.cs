using CounterStrikeSharp.API.Modules.Timers;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region SaveCreditTimer

    public CounterStrikeSharp.API.Modules.Timers.Timer SaveCreditTimer()
    {
        return AddTimer(60f, () =>
        {
            UpdateAllModels();
        }, Full);
    }

    #endregion SaveCreditTimer
}