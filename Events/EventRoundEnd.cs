using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventRoundEnd()
    {
        RegisterEventHandler<EventRoundEnd>((@event, _) =>
        {
            PrepareRoundDefaults();
            CoinRemove();
            CheckAllLevelTags();
            RoundEndParticle(@event.Winner);
            if (KumarKapatDisable == false)
            {
                RuletActivate();
                PiyangoKazananSonuc();
            }
            return HookResult.Continue;
        });
    }

    private void PrepareRoundDefaults()
    {
        foreach (var item in _Config.Additional.RoundEndStartCommands)
        {
            Server.ExecuteCommand(item);
        }
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