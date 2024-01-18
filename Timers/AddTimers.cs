namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooGiveAndSaveTPToAllTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooUpdateAllTimeTrackingDatasTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooSaveTimeTrackingDatasTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooQueueProcessTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooSteamGroupsTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooCoinGoWantedTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooCoinRespawnTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooGiveCreditTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooGiveCreditToGroupTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooGiveCreditToCssAdmin1Timer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooGiveCreditToCssLiderTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooSaveCreditTimer { get; set; } = null;

    private void AddTimers()
    {
        #region Credit

        FooGiveCreditTimer = GiveCreditTimer();
        FooGiveCreditToGroupTimer = GiveCreditToGroupTimer();
        FooGiveCreditToCssAdmin1Timer = GiveCreditToCssAdmin1Timer();
        FooGiveCreditToCssLiderTimer = GiveCreditToCssLiderTimer();
        FooSaveCreditTimer = SaveCreditTimer();

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
    }
}