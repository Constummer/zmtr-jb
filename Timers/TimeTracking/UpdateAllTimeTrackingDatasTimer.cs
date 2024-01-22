using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Timers;
using System.Diagnostics;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region UpdateAllTimeTrackingDatas

    public CounterStrikeSharp.API.Modules.Timers.Timer UpdateAllTimeTrackingDatasTimer()
    {
        return AddTimer(60 * 15, () =>
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            UpdateAllTimeTrackingData();
            stopwatch.Stop();
            Server.PrintToConsole("UpdateAllTimeTrackingDatasTimer = " + stopwatch.Elapsed.TotalMilliseconds);
        }, Full);
    }

    #endregion UpdateAllTimeTrackingDatas
}