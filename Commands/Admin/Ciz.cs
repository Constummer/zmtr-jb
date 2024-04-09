using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using System.Collections.Concurrent;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private ConcurrentDictionary<ulong, bool> GrabOrCizPlayers = new();

    [ConsoleCommand("ciz")]
    [ConsoleCommand("cizac")]
    public void Ciz(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false) return;

        LogManagerCommand(player.SteamID, info.GetCommandString);
        GrabOrCizPlayers.AddOrUpdate(player.SteamID, true, (k, i) => true);
    }

    [ConsoleCommand("cizkapa")]
    [ConsoleCommand("cizkapat")]
    [ConsoleCommand("cizsil")]
    public void CizKapa(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false) return;
        LogManagerCommand(player.SteamID, info.GetCommandString);
        GrabOrCizPlayers.AddOrUpdate(player.SteamID, false, (k, i) => false);
    }

    private void CizOnTick(CCSPlayerController player)
    {
        if (ValidateCallerPlayer(player, false) == false) { return; }
        if (GrabAllowedSteamIds.Contains(player.SteamID) == false) return;
        if (GrabOrCizPlayers.TryGetValue(player.SteamID, out var c) && c)
        {
            bool isFButtonPressed = (player.Pawn.Value.MovementServices!.Buttons.ButtonStates[0] & FButtonIndex) != 0;
            if (isFButtonPressed)
            {
                var end = GetEndXYZ(player, 40);
                end.Z = end.Z + 65;
                _ = DrawLaser(new Vector(end.X, end.Y, end.Z - 1), end, LaserType.CizRgb, true, 10f);
            }
        }
    }
}