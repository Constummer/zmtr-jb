namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void SteamGroupsTimer()
    {
        AddTimer(5f, () =>
        {
            CheckSteamGroupsData();
        }, Full);
    }
}