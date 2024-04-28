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
                       }
                   });
        }, Full);
    }
}