namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void AddTimers()
    {
        #region Credit

        GiveCreditTimer();
        GiveCreditToGroupTimer();
        GiveCreditToCssAdmin1Timer();
        GiveCreditToCssLiderTimer();
        SaveCreditTimer();

        #endregion Credit

        #region Coin

        CoinGoWantedTimer();
        CoinRespawnTimer();

        #endregion Coin

        #region Level

        GiveAndSaveTPToAll();

        #endregion Level

        #region TimeTracking

        UpdateAllTimeTrackingDatas();
        SaveTimeTrackingDatas();

        #endregion TimeTracking

        #region QueueProcess

        QueueProcess();

        #endregion QueueProcess
    }
}