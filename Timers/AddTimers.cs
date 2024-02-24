using CounterStrikeSharp.API;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooGiveAndSaveTPToAllTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooUpdateAllTimeTrackingDatasTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooSaveTimeTrackingDatasTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooQueueProcessTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooSteamGroupsTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooDailyRestartTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooDcWardenNotifyTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooSaveAllParticleDataTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooCoinGoWantedTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooCoinRespawnTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooGiveCreditTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooGiveCreditToGroupTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooGiveCreditToCssAdmin1Timer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooGiveCreditToCssLiderTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooSaveCreditTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooGiveCreditToCssPremiumTimer { get; set; } = null;

    public static TimeOnly MaintenanceTimer { get => new TimeOnly(6, 54); }
    public static TimeOnly MaintenanceGameTimer { get => new TimeOnly(6, 29); }
    public static TimeOnly MaintenanceTimer2 { get => new TimeOnly(7, 01); }

    private void AddTimers()
    {
        #region Credit

        FooGiveCreditTimer = GiveCreditTimer();
        FooGiveCreditToGroupTimer = GiveCreditToGroupTimer();
        FooGiveCreditToCssAdmin1Timer = GiveCreditToCssAdmin1Timer();
        FooGiveCreditToCssLiderTimer = GiveCreditToCssLiderTimer();
        FooSaveCreditTimer = SaveCreditTimer();
        FooGiveCreditToCssPremiumTimer = GiveCreditToCssPremiumTimer();

        #endregion Credit

        #region Coin

        FooCoinGoWantedTimer = CoinGoWantedTimer();
        FooCoinRespawnTimer = CoinRespawnTimer();

        #endregion Coin

        #region Level

        FooGiveAndSaveTPToAllTimer = GiveAndSaveTPToAllTimer();

        #endregion Level

        #region TimeTracking

        FooUpdateAllTimeTrackingDatasTimer = UpdateAllTimeTrackingDatasTimer();
        FooSaveTimeTrackingDatasTimer = SaveTimeTrackingDatasTimer();

        #endregion TimeTracking

        #region QueueProcess

        FooQueueProcessTimer = QueueProcessTimer();

        #endregion QueueProcess

        #region SteamGroups

        FooSteamGroupsTimer = SteamGroupsTimer();

        #endregion SteamGroups

        #region Extras

        FooDailyRestartTimer = DailyRestartTimer();
        FooDcWardenNotifyTimer = DcWardenNotifyTimer();

        #endregion Extras

        #region Particle

        FooSaveAllParticleDataTimer = SaveAllParticleDataTimer();

        #endregion Particle
    }
}