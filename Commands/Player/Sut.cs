using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Sut

    private static Dictionary<ulong, DateTime> LatestSutolCommandCalls = new Dictionary<ulong, DateTime>();

    [ConsoleCommand("sut")]
    [ConsoleCommand("sutol")]
    public void Sut(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (GetTeam(player) != CsTeam.Terrorist)
        {
            player.PrintToChat($" {CC.LR}[ZMTR]{CC.W} Sadece T ler bu komutu kullanabilir.");
            return;
        }
        if (LatestSutolCommandCalls.TryGetValue(player.SteamID, out var call))
        {
            if (DateTime.UtcNow < call.AddMinutes(1))
            {
                player.PrintToChat($" {CC.LR}[ZMTR] {CC.W}Tekrar sutol diyebilmek için {CC.DR}60 {CC.W}saniye beklemelisin!");
                return;
            }
        }
        GetPlayers()
        .Where(x => x.PawnIsAlive && x.SteamID == player!.SteamID)
        .ToList()
        .ForEach(x =>
        {
            SetColour(x, Color.FromArgb(128, 0, 128));
            RemoveWeapons(x, false);

            LatestSutolCommandCalls[player.SteamID] = DateTime.UtcNow;
            Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı oyuncu, {CC.LP}süt oldu.");
        });
    }

    #endregion Sut
}