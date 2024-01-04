using CounterStrikeSharp.API.Modules.Timers;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region GiveCreditToLiderTimer

    public void GiveAndSaveTPToAll()
    {
        AddTimer(60f, () =>
        {
            foreach (var item in PlayerLevels.ToList())
            {
                item.Value.Xp++;

                if (PlayerLevels.Contains(item))
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