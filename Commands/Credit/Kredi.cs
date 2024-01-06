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
                player.PrintToChat($"{Prefix} {CC.W}Tekrar kredi yazabilmek için {CC.DR}5 {CC.W}saniye beklemelisin!");
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
        Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName} {CC.W}adlı oyuncunun {CC.LB}{amount} {CC.W}kredisi var!");
        LatestKredimCommandCalls[player.SteamID] = DateTime.UtcNow;
    }

    #endregion Kredi
}