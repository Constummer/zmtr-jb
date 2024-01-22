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
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooCoinGoWantedTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooCoinRespawnTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooGiveCreditTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooGiveCreditToGroupTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooGiveCreditToCssAdmin1Timer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooGiveCreditToCssLiderTimer { get; set; } = null;
    public CounterStrikeSharp.API.Modules.Timers.Timer? FooSaveCreditTimer { get; set; } = null;

    public static TimeOnly MaintenanceTimer { get => new TimeOnly(6, 54); }
    public static TimeOnly MaintenanceTimer2 { get => new TimeOnly(7, 01); }

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

        #region DailyRestart

        FooDailyRestartTimer = DailyRestartTimer();

        #endregion DailyRestart
    }

    private CounterStrikeSharp.API.Modules.Timers.Timer DailyRestartTimer()
    {
        return AddTimer(300f, () =>
        {
            var nowTime = TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(3));
            if (nowTime >= MaintenanceTimer && nowTime <= MaintenanceTimer2)
            {
                Server.PrintToChatAll($"{Prefix} BAKIM GEREÐÝ SERVERE 07.00 DA RES GELECEKTÝR !!!!");
                Server.PrintToChatAll($"{Prefix} BAKIM GEREÐÝ SERVERE 07.00 DA RES GELECEKTÝR !!!!");
                Server.PrintToChatAll($"{Prefix} BAKIM GEREÐÝ SERVERE 07.00 DA RES GELECEKTÝR !!!!");
                Server.PrintToChatAll($"{Prefix} BAKIM GEREÐÝ SERVERE 07.00 DA RES GELECEKTÝR !!!!");
                Server.PrintToChatAll($"{Prefix} BAKIM GEREÐÝ SERVERE 07.00 DA RES GELECEKTÝR !!!!");
            }
        }, Full);
    }
}