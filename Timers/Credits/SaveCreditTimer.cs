using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Timers;
using System.Diagnostics;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region SaveCreditTimer

    public CounterStrikeSharp.API.Modules.Timers.Timer SaveCreditTimer()
    {
        return AddTimer(60f, () =>
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            UpdateAllModels();
            stopwatch.Stop();
            Server.PrintToConsole("SaveCreditTimer = " + stopwatch.Elapsed.TotalMilliseconds);
        }, Full);
    }

    #endregion SaveCreditTimer
}