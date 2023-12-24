namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void ClearAll()
    {
        CountdownText = "";
        CountdownTime = 0;
        Countdown_enable_text = false;
        timer_2?.Kill();
        timer_1?.Kill();
        ActiveGodMode?.Clear();
        DeathLocations?.Clear();
        KilledPlayers?.Clear();
        LatestHediyeCommandCalls?.Clear();

        ClearCits();
        ClearLasers();
    }
}