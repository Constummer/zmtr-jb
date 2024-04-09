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
        FooSaveCreditTimer?.Kill();
        FooDailyRestartTimer?.Kill();
        FooDcWardenNotifyTimer?.Kill();
        FooSaveAllParticleDataTimer?.Kill();
        FooWTimeSaveAndUpdateTimer?.Kill();
        FooGetGambleHistoryTimer?.Kill();
        FooGiveAndSaveTPToAllTimer = null;
        FooUpdateAllTimeTrackingDatasTimer = null;
        FooSaveTimeTrackingDatasTimer = null;
        FooQueueProcessTimer = null;
        FooSteamGroupsTimer = null;
        FooCoinGoWantedTimer = null;
        FooCoinRespawnTimer = null;
        FooGiveCreditTimer = null;
        FooSaveCreditTimer = null;
        FooDailyRestartTimer = null;
        FooDcWardenNotifyTimer = null;
        FooSaveAllParticleDataTimer = null;
        FooWTimeSaveAndUpdateTimer = null;
        FooGetGambleHistoryTimer = null;
    }
}