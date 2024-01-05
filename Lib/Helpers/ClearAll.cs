﻿using CounterStrikeSharp.API.Modules.Entities;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void ClearAll()
    {
        CountdownText = "";
        CountdownTime = 0;
        Countdown_enable_text = false;
        RespawnAcActive = false;
        timer?.Kill();
        ActiveGodMode?.Clear();
        DeathLocations?.Clear();
        KilledPlayers?.Clear();
        LatestHediyeCommandCalls?.Clear();
        HookDisablePlayers?.Clear();
        HookDisabled = false;
        CitEnabledPlayers?.Clear();
        FFMenuCheck = false;
        ClearParachutes();
        ClearCits();
        ClearLasers();
    }

    private void ClearOnDisconnect(ulong steamId, int? userId)
    {
        _ = ActiveGodMode?.Remove(steamId, out _);
        _ = DeathLocations?.Remove(steamId, out _);
        _ = KilledPlayers?.Remove(steamId, out _);
        _ = LatestHediyeCommandCalls?.Remove(steamId, out _);
        _ = PlayerMarketModels?.Remove(steamId, out _);
        _ = PlayerLevels?.Remove(steamId, out _);
        if (userId != null && userId != -1)
        {
            RemoveGivenParachute(userId.Value);
        }
        _ = SpeedoMeterActive?.RemoveAll(x => x == steamId);
        _ = LevelTagDisabledPlayers?.RemoveAll(x => x == steamId);
        _ = HookDisablePlayers?.RemoveAll(x => x == steamId);
        _ = CitEnabledPlayers?.RemoveAll(x => x == steamId);
    }
}