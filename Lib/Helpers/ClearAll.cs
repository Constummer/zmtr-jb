namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void ClearAll()
    {
        SutolCommandCalls?.Clear();
        SinirsizXKapaAction("@all", null);
        CountdownText = "";
        CountdownTime = 0;
        Countdown_enable_text = false;
        MarketEnvDisable = false;
        RespawnAcActive = false;
        timer?.Kill();
        ActiveGodMode?.Clear();
        DeathLocations?.Clear();
        KilledPlayers?.Clear();
        LatestHediyeCommandCalls?.Clear();
        HookDisablePlayers?.Clear();
        HookDisabled = false;
        CitEnabledPlayers?.Clear();
        TeamSteamIds?.Clear();
        FFMenuCheck = false;
        SpeedActive = false;
        SpeedActiveDatas?.Clear();
        SkzV2FailedSteamIds?.Clear();
        DizPlayerId = 0;
        DizActive = false;
        Config.UnrestrictedFov.Enabled = true;
        DizStart = null;
        DizEnd = null;
        UberSlapTimer?.Kill();
        FFTimer?.Kill();
        fzTimer?.Kill();
        BurnTimer?.Kill();
        unfzTimer?.Kill();
        BurnTimer?.Kill();
        DrugTimer?.Kill();
        LRTimer?.Kill();
        LRTimer = null;
        FFTimer = null;
        fzTimer = null;
        BurnTimer = null;
        unfzTimer = null;
        BurnTimer = null;
        DrugTimer = null;
        TgActive = false;
        TgTimer?.Kill();
        TgTimer = null;
        ActiveTeamGamesGameBase?.Clear(false);
        ActiveTeamGamesGameBase = null;
        LrCancel();
        ClearParachutes();
        ClearCits();
        ClearLasers();
    }

    private static void ClearOnDisconnect(ulong steamId, int? userId)
    {
        if (userId != null && userId != -1)
        {
            RemoveGivenParachute(userId.Value);
        }
        UpdatePlayerMarketData(steamId);
        UpdatePlayerLevelDataOnDisonnect(steamId);
        UpdatePlayerParticleDataOnDisonnect(steamId);

        _ = ActiveGodMode?.Remove(steamId, out _);
        _ = DeathLocations?.Remove(steamId, out _);
        _ = KilledPlayers?.Remove(steamId, out _);
        _ = LatestKredimCommandCalls?.Remove(steamId, out _);
        _ = LatestSeviyemCommandCalls?.Remove(steamId, out _);
        _ = LatestHediyeCommandCalls?.Remove(steamId, out _);
        _ = PlayerMarketModels?.Remove(steamId, out _);
        _ = PlayerLevels?.Remove(steamId, out _);
        _ = Gags?.Remove(steamId, out _);
        _ = HideFoots?.Remove(steamId, out _);
        _ = HookPlayers?.Remove(steamId, out _);
        _ = PlayerTimeTracking?.Remove(steamId, out _);
        _ = SpeedActiveDatas?.Remove(steamId, out _);
        _ = FovActivePlayers?.Remove(steamId, out _);

        _ = SutolCommandCalls?.RemoveAll(x => x == steamId);
        _ = PlayerSteamGroup?.RemoveAll(x => x == steamId);
        _ = SpeedoMeterActive?.RemoveAll(x => x == steamId);
        _ = LevelTagDisabledPlayers?.RemoveAll(x => x == steamId);
        _ = HookDisablePlayers?.RemoveAll(x => x == steamId);
        _ = CitEnabledPlayers?.RemoveAll(x => x == steamId);
    }
}