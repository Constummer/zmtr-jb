using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public partial class BattlePassBase
    {
        internal static void RoundTWin()
        {
            GetPlayers(CsTeam.Terrorist)
                  .ToList()
                  .ForEach(x =>
                  {
                      if (BattlePassDatas.TryGetValue(x.SteamID, out var battlePassData))
                      {
                          battlePassData?.OnRoundTWinCommand();
                      }
                  });
        }

        internal static void RoundCTWin()
        {
            GetPlayers(CsTeam.Terrorist)
                  .ToList()
                  .ForEach(x =>
                  {
                      if (BattlePassDatas.TryGetValue(x.SteamID, out var battlePassData))
                      {
                          battlePassData?.OnRoundCTWinCommand();
                      }
                  });
        }
    }
}