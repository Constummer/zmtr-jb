using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventRoundEnd()
    {
        RegisterEventHandler<EventRoundEnd>((@event, _) =>
        {
            PrepareRoundDefaults();
            RoundDefaultCommands();

            CoinRemove();
            CheckAllLevelTags();
            if (KumarKapatDisable == false)
            {
                RuletActivate();
                PiyangoKazananSonuc();
            }
            if (PatronuKoruActive)
            {
                ChooseRandomTwoGuardian();
            }
            RoundEndParticle(@event.Winner);

            switch ((CsTeam)@event.Winner)
            {
                case CsTeam.Terrorist:
                    BattlePassBase.RoundTWin();
                    break;

                case CsTeam.CounterTerrorist:
                    BattlePassBase.RoundCTWin();
                    break;
            };
            return HookResult.Continue;
        });
    }

    private void RoundDefaultCommands()
    {
        foreach (var item in _Config.Additional.RoundEndStartCommands)
        {
            Server.ExecuteCommand(item);
        }
    }

    private void PrepareRoundDefaults()
    {
        LastRSoundDisable = false;
        CurrentCtRespawns = 0;
        LrActive = false;
        UnlimitedReserverAmmoActive = false;
        CoinAngleYUpdaterActive = false;
        CoinSpawned = false;
        _Config.Additional.ParachuteEnabled = true;
        _Config.Additional.ParachuteModelEnabled = true;
        TeamSteamIds?.Clear();
        TeamActive = false;
        SinirsizMolyTimer?.Kill();
        SinirsizMolyTimer = null;
        SinirsizBombaTimer?.Kill();
        SinirsizBombaTimer = null;
        SinirsizXTimer?.Kill();
        SinirsizXTimer = null;
    }
}