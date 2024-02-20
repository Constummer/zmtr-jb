﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using MySqlConnector;
using System.Diagnostics;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region GiveCreditToLiderTimer

    public CounterStrikeSharp.API.Modules.Timers.Timer GiveAndSaveTPToAllTimer()
    {
        return AddTimer(360f, () =>
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            var lider1Players = GetPlayers()
                                .Where(x => AdminManager.PlayerHasPermissions(x, "@css/liderkredi"))
                                .ToList()
                                .Select(x => x.SteamID);
            var premiumPlayers = GetPlayers()
                             .Where(x => AdminManager.PlayerHasPermissions(x, "@css/premium"))
                             .ToList()
                             .Select(x => x.SteamID);
            foreach (var item in PlayerLevels.ToList())
            {
                item.Value.Xp = item.Value.Xp + (1 * 6 * TPModifier);
                if (lider1Players.Contains(item.Key))
                {
                    item.Value.Xp = item.Value.Xp + (int)(0.5 * 6 * TPModifier);
                }
                if (premiumPlayers.Contains(item.Key))
                {
                    item.Value.Xp = item.Value.Xp + (1 * 6 * TPModifier);
                }
                if (PlayerLevels.ContainsKey(item.Key))
                {
                    PlayerLevels[item.Key] = item.Value;
                    Task.Run(async () =>
                    {
                        await UpdatePlayerLevelData(item.Key, item.Value.Xp);
                    });
                }
            }
            stopwatch.Stop();
            Server.PrintToConsole("GiveAndSaveTPToAllTimer = " + stopwatch.Elapsed.TotalMilliseconds);
        }, Full);
    }

    #endregion GiveCreditToLiderTimer
}