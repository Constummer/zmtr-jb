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
        }, Full);
    }
}