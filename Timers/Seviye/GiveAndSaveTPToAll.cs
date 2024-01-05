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
                var player = GetPlayers().Where(x => x.SteamID == item.Key).FirstOrDefault();
                if (player != null)
                {
                    player.PrintToChat($" {CC.LR}[ZMTR] {CC.W}Bu sunucuda {CC.G}1 {CC.W}dakika zaman geçirdiğin için {CC.LB}1{CC.W} TP kazandın!");
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