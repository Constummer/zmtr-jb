namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void TeamGamesCancel()
    {
        TgActive = false;
        TgTimer?.Kill();
        TgTimer = null;
        ActiveTeamGamesGameBase?.Clear();
        ActiveTeamGamesGameBase = null;
    }
}