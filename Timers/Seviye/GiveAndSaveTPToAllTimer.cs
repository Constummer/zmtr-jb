namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region GiveCreditToLiderTimer

    public CounterStrikeSharp.API.Modules.Timers.Timer GiveAndSaveTPToAllTimer()
    {
        return AddTimer(360f, () =>
        {
            BulkUpdatePlayerLevels();
        }, Full);
    }

    #endregion GiveCreditToLiderTimer
}