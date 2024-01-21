namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static void TeamGamesCancel(bool printMsg)
    {
        TgActive = false;
        TgTimer?.Kill();
        TgTimer = null;
        ActiveTeamGamesGameBase?.Clear(printMsg);
        ActiveTeamGamesGameBase = null;
    }
}