using CounterStrikeSharp.API;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static void WardenRefreshPawn()
    {
        var warden = GetWarden();
        if (warden != null)
        {
            RefreshPawn(warden);
        }
    }

    private static void RemoveWarden()
    {
        Server.NextFrame(() =>
        {
            WardenRefreshPawn();
            LatestWCommandUser = null;
            ClearLasers();
            CoinRemove();
            WardenLeaveSound();
        });
    }
}