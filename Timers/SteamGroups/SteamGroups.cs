namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private CounterStrikeSharp.API.Modules.Timers.Timer SteamGroupsTimer()
    {
        return AddTimer(5f, () =>
        {
            CheckSteamGroupsData();
        }, Full);
    }
}