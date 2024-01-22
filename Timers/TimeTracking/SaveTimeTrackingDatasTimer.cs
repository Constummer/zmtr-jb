using CounterStrikeSharp.API;
using System.Diagnostics;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region SaveTimeTrackingDatas

    public CounterStrikeSharp.API.Modules.Timers.Timer SaveTimeTrackingDatasTimer()
    {
        return AddTimer(60f, () =>
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            UpdatePlayerTimeDataBulk();
            stopwatch.Stop();
            Server.PrintToConsole("SaveTimeTrackingDatasTimer = " + stopwatch.Elapsed.TotalMilliseconds);
        }, Full);
    }

    #endregion SaveTimeTrackingDatas
}