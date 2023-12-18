namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void ClearAll()
    {
        Text = "";
        Time = 0;
        Countdown_enable_text = false;
        Countdown_enable = false;
        timer_2?.Kill();
        timer_1?.Kill();
        ActiveGodMode?.Clear();
        DeathLocations?.Clear();
        KilledPlayers?.Clear();
        LatestHediyeCommandCalls?.Clear();

        if (Lasers != null)
        {
            foreach (var item in Lasers)
            {
                item.Remove();
            }
            Lasers.Clear();
        }
    }
}