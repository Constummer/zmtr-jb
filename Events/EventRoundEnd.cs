using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.Logging;

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
            roundendparticle(@event.Winner);
            //AllDonbozAction();
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
    public void roundendparticle(int winner)
    {
        GetPlayers()
            .ToList()
            .ForEach(p =>
            {
                var Aura = Utilities.CreateEntityByName<CParticleSystem>("info_particle_system");

                if (Aura != null && Aura.IsValid)
                {
                    switch (winner)
                    {
                        case 2:
                            Aura.EffectName = "particles/test/energy.vpcf";
                            break;
                        case 3:
                            Aura.EffectName = "particles/test/rengarenk.vpcf";
                            break;
                        default:
                            break; 
                    }                    
                    Aura.TintCP = 1;

                    Aura.Teleport(p.PlayerPawn.Value.AbsOrigin, ANGLE_ZERO, VEC_ZERO);
                    Aura.DispatchSpawn();
                    Aura.AcceptInput("Start");
                    CustomSetParent(Aura, p.PlayerPawn.Value);
                }
            });
    }
}