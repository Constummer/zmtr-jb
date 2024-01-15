using CounterStrikeSharp.API.Modules.Admin;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region GiveCreditToLiderTimer

    public void GiveAndSaveTPToAll()
    {
        AddTimer(300f, () =>
        {
            var lider1Players = GetPlayers()
                                .Where(x => AdminManager.PlayerHasPermissions(x, "@css/lider"))
                                .ToList()
                                .Select(x => x.SteamID);
            foreach (var item in PlayerLevels.ToList())
            {
                item.Value.Xp = item.Value.Xp + 5;
                if (lider1Players.Contains(item.Key))
                {
                    item.Value.Xp = item.Value.Xp + 5;
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
        }, Full);
    }

    #endregion GiveCreditToLiderTimer
}