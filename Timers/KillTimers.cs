namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void KillTimers()
    {
        FooGiveAndSaveTPToAllTimer?.Kill();
        FooUpdateAllTimeTrackingDatasTimer?.Kill();
        FooSaveTimeTrackingDatasTimer?.Kill();
        FooQueueProcessTimer?.Kill();
        FooSteamGroupsTimer?.Kill();
        FooCoinGoWantedTimer?.Kill();
        FooCoinRespawnTimer?.Kill();
        FooGiveCreditTimer?.Kill();
        FooGiveCreditToGroupTimer?.Kill();
        FooGiveCreditToCssAdmin1Timer?.Kill();
        FooGiveCreditToCssLiderTimer?.Kill();
        FooSaveCreditTimer?.Kill();
        FooDailyRestartTimer?.Kill();
        FooGiveAndSaveTPToAllTimer = null;
        FooUpdateAllTimeTrackingDatasTimer = null;
        FooSaveTimeTrackingDatasTimer = null;
        FooQueueProcessTimer = null;
        FooSteamGroupsTimer = null;
        FooCoinGoWantedTimer = null;
        FooCoinRespawnTimer = null;
        FooGiveCreditTimer = null;
        FooGiveCreditToGroupTimer = null;
        FooGiveCreditToCssAdmin1Timer = null;
        FooGiveCreditToCssLiderTimer = null;
        FooSaveCreditTimer = null;
        FooDailyRestartTimer = null;
    }
}