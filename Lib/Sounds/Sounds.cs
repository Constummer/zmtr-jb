using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static void LastAliveTSound()
    {
        if (LastRSoundDisable)
        {
            return;
        }
        if (GetPlayerCount(CsTeam.Terrorist, true) == 2)//TODO: yeni method ile 1 mi olmali 2 mi emin deilim
        {
            var players = GetPlayers();
            foreach (var player in players)
            {
                player.ExecuteClientCommand($"play {_Config.Sounds.LastAliveTSound}");
            }
        }
    }

    private static void WardenEnterSound()
    {
        var players = GetPlayers();
        foreach (var player in players)
        {
            player.ExecuteClientCommand($"play {_Config.Sounds.WardenEnterSound}");
        }
    }

    private static void WardenLeaveSound()
    {
        var players = GetPlayers();
        foreach (var player in players)
        {
            player.ExecuteClientCommand($"play {_Config.Sounds.WardenLeaveSound}");
        }
    }

    private static void FreezeOrUnfreezeSound()
    {
        var players = GetPlayers();
        foreach (var player in players)
        {
            player.ExecuteClientCommand($"play {_Config.Sounds.FreezeOrUnfreezeSound}");
        }
    }

    private static void LrStartSound()
    {
        var players = GetPlayers();
        foreach (var player in players)
        {
            player.ExecuteClientCommand($"play {_Config.Sounds.LrStartSound}");
        }
    }

    private static void TGStartSound()
    {
        var players = GetPlayers();
        foreach (var player in players)
        {
            player.ExecuteClientCommand($"play {_Config.Sounds.TGStartSound}");
        }
    }
}