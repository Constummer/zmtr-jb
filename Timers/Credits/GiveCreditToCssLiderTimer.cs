﻿using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Timers;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region GiveCreditToLiderTimer

    public void GiveCreditToCssLiderTimer()
    {
        AddTimer(300f, () =>
        {
            GetPlayers()
            .ToList()
            .ForEach(x =>
            {
                if (AdminManager.PlayerHasPermissions(x, "@css/lider"))
                {
                    if (x?.SteamID != null && x!.SteamID != 0)
                    {
                        if (PlayerMarketModels.TryGetValue(x.SteamID, out var item))
                        {
                            item.Credit += Config.Credit.RetrieveCreditEvery5MinRewardCssLider;
                        }
                        else
                        {
                            item = new(x.SteamID);
                            item.Credit = Config.Credit.RetrieveCreditEvery5MinRewardCssLider;
                        }
                        PlayerMarketModels[x.SteamID] = item;
                        x.PrintToChat($"{Prefix} {CC.R}Lider{CC.W} olduğun için {CC.LB}{Config.Credit.RetrieveCreditEvery5MinRewardCssLider} {CC.W}kredi kazandın!");
                    }
                }
            });
        }, TimerFlags.REPEAT);
    }

    #endregion GiveCreditToLiderTimer
}