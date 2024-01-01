﻿using CounterStrikeSharp.API.Modules.Entities;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void ClearAll()
    {
        CountdownText = "";
        CountdownTime = 0;
        Countdown_enable_text = false;
        timer?.Kill();
        ActiveGodMode?.Clear();
        DeathLocations?.Clear();
        KilledPlayers?.Clear();
        LatestHediyeCommandCalls?.Clear();
        HookDisablePlayers?.Clear();
        HookDisabled = false;
        CitEnabledPlayers?.Clear();
        ClearCits();
        ClearLasers();
    }

    private void ClearOnDisconnect(ulong steamId)
    {
        _ = ActiveGodMode?.Remove(steamId, out _);
        _ = DeathLocations?.Remove(steamId, out _);
        _ = KilledPlayers?.Remove(steamId, out _);
        _ = LatestHediyeCommandCalls?.Remove(steamId, out _);
        HookDisablePlayers = HookDisablePlayers?.Where(x => x != steamId).ToList() ?? new();
        CitEnabledPlayers = CitEnabledPlayers?.Where(x => x != steamId).ToList() ?? new();
    }
}