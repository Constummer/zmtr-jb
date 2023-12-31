namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void ClearAll()
    {
        CountdownText = "";
        CountdownTime = 0;
        Countdown_enable_text = false;
        timer?.Kill();
        ActiveGodMode?.Clear();
        DeathLocations?.Clear();
        KilledPlayers?.Clear();
        LatestHediyeCommandCalls?.Clear();
        HookDisablePlayers?.Clear();
        ClearCits();
        ClearLasers();
    }
}