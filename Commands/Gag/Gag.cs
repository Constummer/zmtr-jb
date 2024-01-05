using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.Logging;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Dictionary<ulong, DateTime> Gags = new Dictionary<ulong, DateTime>();

    #region Gag

    [ConsoleCommand("gag")]
    [RequiresPermissions("@css/chat")]
    [CommandHelper(1, "<playerismi>  [dakika]")]
    public void OnGagCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        var target = info.ArgCount > 2 ? info.GetArg(2) : "0";
        if (int.TryParse(target, out var value))
        {
            if (value < 1)
            {
                player.PrintToChat("Minimum 1 girebilirsin");
                return;
            }
        }
        else
        {
            player.PrintToChat("Minimum 1 girebilirsin");
            return;
        }

        var playerStr = string.Empty;
        if (info.ArgCount <= 1)
        {
            playerStr = info.GetArg(1);
        }
        Logger.LogInformation(playerStr);
        var players = GetPlayers()
               .Where(x => x.PlayerName.ToLower().Contains(playerStr.ToLower()))
               .ToList();
        if (players.Count == 0)
        {
            player.PrintToChat($" {CC.LR}[ZMTR] {CC.W}Eşleşen oyuncu bulunamadı!");
            return;
        }
        if (players.Count != 1)
        {
            player.PrintToChat($" {CC.LR}[ZMTR] {CC.W}Birden fazla oyuncu bulundu.");
            return;
        }
        var gagPlayer = players.FirstOrDefault();
        if (gagPlayer == null)
        {
            player.PrintToChat($" {CC.LR}[ZMTR] {CC.W} Oyuncu bulunamadı!");
            return;
        }

        if (value <= 0)
        {
            if (Gags.TryGetValue(gagPlayer.SteamID, out var dateTime))
            {
                Gags[gagPlayer.SteamID] = DateTime.UtcNow.AddYears(1);
            }
            else
            {
                Gags.Add(gagPlayer.SteamID, DateTime.UtcNow.AddYears(1));
            }
            Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{gagPlayer.PlayerName} {CC.B}Sınırsız{CC.W} gagladı.");
        }
        else
        {
            if (Gags.TryGetValue(gagPlayer.SteamID, out var dateTime))
            {
                Gags[gagPlayer.SteamID] = DateTime.UtcNow.AddMinutes(value);
            }
            else
            {
                Gags.Add(gagPlayer.SteamID, DateTime.UtcNow.AddMinutes(value));
            }
            Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{gagPlayer.PlayerName} {CC.B}{value}{CC.W} dakika boyunca gagladı.");
        }
    }

    private bool GagChecker(CCSPlayerController player, string arg)
    {
        if (Gags.TryGetValue(player.SteamID, out var call))
        {
            Logger.LogInformation($"{call.ToString()}");
            Logger.LogInformation($"{DateTime.UtcNow.ToString()}");
            if (call > DateTime.UtcNow)
            {
                player.PrintToChat($" {CC.LR}[ZMTR] {CC.W} GAGLISIN!");
                return true;
            }
        }
        if (PGags.Contains(player.SteamID))
        {
            player.PrintToChat($" {CC.LR}[ZMTR] {CC.W} SINIRSIZ GAGLISIN!");
            return true;
        }
        return false;
    }

    #endregion Gag
}