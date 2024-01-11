using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Timers;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region GiveCreditToLiderTimer

    public void GiveAndSaveTPToAll()
    {
        AddTimer(60f, () =>
        {
            var lider1Players = GetPlayers()
                                .Where(x => AdminManager.PlayerHasPermissions(x, "@css/lider"))
                                .ToList()
                                .Select(x => x.SteamID);
            foreach (var item in PlayerLevels.ToList())
            {
                item.Value.Xp++;
                if (lider1Players.Contains(item.Key))
                {
                    item.Value.Xp++;
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
        }, TimerFlags.REPEAT);
    }

    #endregion GiveCreditToLiderTimer
}