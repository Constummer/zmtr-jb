using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region UnGag

    [ConsoleCommand("ungag")]
    [RequiresPermissions("@css/chat")]
    [CommandHelper(1, "<playerismi>")]
    public void OnUnGagCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        var playerStr = string.Empty;
        if (info.ArgCount <= 1)
        {
            playerStr = info.GetArg(1);
        }

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
        Gags.Remove(gagPlayer.SteamID);
        PGags = PGags.Where(x => x != gagPlayer.SteamID).ToList();
        RemoveFromPGag(player.SteamID);
        Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{gagPlayer.PlayerName} {CC.W}gagını kaldırdı.");
    }

    #endregion UnGag
}