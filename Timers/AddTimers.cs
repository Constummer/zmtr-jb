namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void AddTimers()
    {
        #region Credit

        GiveCreditTimer();
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
    }
}