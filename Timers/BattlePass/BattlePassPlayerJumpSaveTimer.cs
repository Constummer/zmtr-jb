namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public CounterStrikeSharp.API.Modules.Timers.Timer BattlePassPlayerJumpSaveTimer()
    {
        return AddTimer(10f, () =>
        {
            foreach (var item in BattlePassJumps)
            {
                var addJump = GetAddJumpAmount(item.Value);
                if (BattlePassDatas.TryGetValue(item.Key, out var data))
                {
                    data.OnEventPlayerJump(addJump);
                }
            }
            BattlePassJumps.Clear();
            foreach (var item in BattlePassPremiumJumps)
            {
                var addJump = GetAddJumpAmount(item.Value);
                if (BattlePassPremiumDatas.TryGetValue(item.Key, out var battlePassPremiumData))
                {
                    battlePassPremiumData?.OnEventPlayerJump(addJump);
                }
            }
            BattlePassPremiumJumps.Clear();

            GetPlayers()
                   .ToList()
                   .ForEach(x =>
                   {
                       if (ValidateCallerPlayer(x, false) == false) return;
                       if (x?.SteamID != null && x!.SteamID != 0)
                       {
                           if (BattlePassDatas.TryGetValue(x.SteamID, out var data))
                           {
                               data.CurrentTime = data.CurrentTime + 5;
                               data.CheckIfLevelUp(false);
                               BattlePassDatas[x.SteamID] = data;
                           }
                           if (BattlePassPremiumDatas.TryGetValue(x.SteamID, out var data2))
                           {
                               data2.CurrentTime = data2.CurrentTime + 5;
                               data2.CheckIfLevelUp(false);
                               BattlePassPremiumDatas[x.SteamID] = data2;
                           }
                           if (TimeRewardDatas.TryGetValue(x.SteamID, out var data3))
                           {
                               data3.CurrentTime = data3.CurrentTime + 5;
                               data3.CheckIfLevelUp();
                               TimeRewardDatas[x.SteamID] = data3;
                           }
                       }
                   });
        }, Full);
    }
}