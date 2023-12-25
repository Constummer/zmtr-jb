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

            return HookResult.Continue;
        });
    }

    private static void PrepareRoundDefaults()
    {
        foreach (var item in _Config.RoundEndStartCommands)
        {
            Server.ExecuteCommand(item);
        }
        CitEnable = false;
        LrActive = false;
        _Config.ParachuteEnabled = true;
        TeamBlueSteamIds = new();
        TeamRedSteamIds = new();
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