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
            TdonbozAction();
            return HookResult.Continue;
        });
    }

    private static void PrepareRoundDefaults()
    {
        foreach (var item in _Config.Additional.RoundEndStartCommands)
        {
            Server.ExecuteCommand(item);
        }
        CurrentCtRespawns = 0;
        LrActive = false;
        UnlimitedReserverAmmoActive = false;
        CoinAngleYUpdaterActive = false;
        CoinSpawned = false;
        _Config.Additional.ParachuteEnabled = true;
        TeamSteamIds?.Clear();
        RuletActivate();
        TeamActive = false;
        GetPlayers()
         .Where(x => ValidateCallerPlayer(x, false) == true && x.SteamID != LatestWCommandUser)
         .ToList()
         .ForEach(x =>
         {
             SetColour(x, DefaultPlayerColor);
             RefreshPawn(x);
         });
    }
}