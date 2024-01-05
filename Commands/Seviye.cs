using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Seviye

    private static Dictionary<ulong, DateTime> LatestSeviyemCommandCalls = new Dictionary<ulong, DateTime>();

    [ConsoleCommand("seviye")]
    [ConsoleCommand("tp")]
    [ConsoleCommand("seviyem")]
    public void Seviye(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        if (LatestSeviyemCommandCalls.TryGetValue(player.SteamID, out var call))
        {
            if (DateTime.UtcNow < call.AddSeconds(5))
            {
                player.PrintToChat($" {CC.LR}[ZMTR] {CC.W}Tekrar seviye yazabilmek için {CC.DR}5 {CC.W}saniye beklemelisin!");
                return;
            }
        }

        var amount = 0;
        if (player?.SteamID != null && player!.SteamID != 0)
        {
            if (PlayerLevels.TryGetValue(player.SteamID, out var item))
            {
                amount = item.Xp;
            }
            else
            {
                player.PrintToChat($" {CC.LR}[ZMTR] {CC.W}Seviyen yok, seviye alabilmek için  {CC.DR}!slotol {CC.W},{CC.DR} !seviyeol {CC.W}yazabilirsin!");
            }
        }
        Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName} {CC.W}adlı oyuncunun {CC.LB}{amount} {CC.W}TP'si var!");
        LatestSeviyemCommandCalls[player.SteamID] = DateTime.UtcNow;
    }

    #endregion Seviye
}