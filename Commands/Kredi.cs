using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Kredi

    private static Dictionary<ulong, DateTime> LatestKredimCommandCalls = new Dictionary<ulong, DateTime>();

    [ConsoleCommand("kredi")]
    [ConsoleCommand("kredim")]
    public void Kredi(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        if (LatestKredimCommandCalls.TryGetValue(player.SteamID, out var call))
        {
            if (DateTime.UtcNow < call.AddSeconds(5))
            {
                player.PrintToChat($" {CC.LR}[ZMTR] {CC.W}Tekrar kredi yazabilmek için {CC.DR}5 {CC.W}saniye beklemelisin!");
                return;
            }
        }

        var amount = 0;
        if (player?.SteamID != null && player!.SteamID != 0)
        {
            if (PlayerMarketModels.TryGetValue(player.SteamID, out var item))
            {
                amount = item.Credit;
            }
        }
        Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName} {CC.W}adlı oyuncunun {CC.LB}{amount} {CC.W}kredisi var!");
        LatestKredimCommandCalls[player.SteamID] = DateTime.UtcNow;
    }

    [ConsoleCommand("krediler")]
    public void Krediler(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($" {CC.LR}[ZMTR]{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        GetPlayers()
                .ToList()
                .ForEach(x =>
                {
                    PlayerMarketModel item = null;
                    if (x?.SteamID != null && x!.SteamID != 0)
                    {
                        _ = PlayerMarketModels.TryGetValue(x.SteamID, out item);
                    }
                    player.PrintToConsole($" {CC.LR}[ZMTR] {CC.G}{x.PlayerName} - {CC.B}{(item?.Credit ?? 0)}");
                });

        player.PrintToChat($" {CC.LR}[ZMTR] {CC.G}Konsoluna bak");
        LatestKredimCommandCalls[player.SteamID] = DateTime.UtcNow;
    }

    #endregion Kredi
}