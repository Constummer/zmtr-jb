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
            return HookResult.Continue;
        });
    }

    private static void PrepareRoundDefaults()
    {
        foreach (var item in _Config.RoundEndStartCommands)
        {
            Server.ExecuteCommand(item);
        }
        CurrentCtRespawns = 0;
        LrActive = false;
        UnlimitedReserverAmmoDisabled = false;
        CoinAngleYUpdaterActive = false;
        CoinSpawned = false;
        _Config.ParachuteEnabled = true;
        TeamSteamIds?.Clear();
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