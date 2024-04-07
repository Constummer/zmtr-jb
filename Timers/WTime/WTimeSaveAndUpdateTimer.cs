namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public CounterStrikeSharp.API.Modules.Timers.Timer WTimeSaveAndUpdateTimer()
    {
        return AddTimer(300f, () =>
         {
             //if (GetPlayerCount() > 16)
             //{
             //SaveWTimes();
             //}
         }, Full);
    }
}