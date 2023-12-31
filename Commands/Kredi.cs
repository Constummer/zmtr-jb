using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

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
                player.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.White}Tekrar kredi yazabilmek için {ChatColors.Darkred}5 {ChatColors.White}saniye beklemelisin!");
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
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName} {ChatColors.White}adlı oyuncunun {ChatColors.LightBlue}{amount} {ChatColors.White}kredisi var!");
        LatestKredimCommandCalls[player.SteamID] = DateTime.UtcNow;
    }

    #endregion Kredi
}