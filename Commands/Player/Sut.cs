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
    private static List<ulong> SutolCommandCalls = new();

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
            player.PrintToChat($"{Prefix}{CC.W} Sadece T ler bu komutu kullanabilir.");
            return;
        }
        if (LatestSutolCommandCalls.TryGetValue(player.SteamID, out var call))
        {
            if (DateTime.UtcNow < call.AddSeconds(30))
            {
                player.PrintToChat($"{Prefix} {CC.W}Tekrar sutol diyebilmek için {CC.DR}30 {CC.W}saniye beklemelisin!");
                return;
            }
        }
        GetPlayers()
        .Where(x => x.PawnIsAlive && x.SteamID == player!.SteamID)
        .ToList()
        .ForEach(x =>
        {
            SutolCommandCalls.Add(x.SteamID);
            SetColour(x, Color.FromArgb(128, 0, 128));
            RefreshPawnTP(x);
            RemoveWeapons(x, false);

            LatestSutolCommandCalls[x.SteamID] = DateTime.UtcNow;
            Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı oyuncu, {CC.LP}süt oldu.");
        });
    }

    [ConsoleCommand("suttemizle")]
    public void SutTemizle(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        SutolCommandCalls.Clear();

        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.LP}süt isimlerini temizledi.");
    }

    #endregion Sut
}