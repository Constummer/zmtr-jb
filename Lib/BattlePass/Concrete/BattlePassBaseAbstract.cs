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

        internal static void EventPlayerDeath(EventPlayerDeath @event)
        {
            if (BattlePassDatas.TryGetValue(@event.Attacker.SteamID, out var battlePassData))
            {
                var deadOne = @event.Userid;
                var attacker = @event.Attacker;
                if (GetTeam(deadOne) == CsTeam.CounterTerrorist &&
                    GetTeam(attacker) == CsTeam.Terrorist)
                {
                    //t->ct
                    battlePassData.EventCTKilled();
                }
                else if (GetTeam(deadOne) == CsTeam.Terrorist &&
                    GetTeam(attacker) == CsTeam.CounterTerrorist)
                {
                    //ct->t
                    battlePassData.EventTKilled();
                }
                else if (GetTeam(deadOne) == CsTeam.CounterTerrorist &&
                    GetTeam(attacker) == CsTeam.Terrorist &&
                    deadOne.SteamID == LatestWCommandUser)
                {
                    //w
                    battlePassData.EventWKilled();
                }
            }
        }
    }
}