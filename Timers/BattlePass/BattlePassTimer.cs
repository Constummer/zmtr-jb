namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public CounterStrikeSharp.API.Modules.Timers.Timer BattlePassTimer()
    {
        return AddTimer(300f, () =>
        {
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
                       }
                   });
        }, Full);
    }
}