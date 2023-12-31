using CounterStrikeSharp.API.Modules.Utils;
using Serilog.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static void LastAliveTSound()
    {
        if (GetPlayers(CsTeam.Terrorist).Where(x => x.PawnIsAlive).Count() == 2)
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
}