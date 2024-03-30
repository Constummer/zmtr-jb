using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("isyantakimal")]
    [ConsoleCommand("isyantakimver")]
    [ConsoleCommand("isyanteamal")]
    [ConsoleCommand("isyanteamver")]
    [CommandHelper(minArgs: 1, "<isyan takima eklenecek kişi>")]
    public void IsyanTakimVer(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/yonetim"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }

        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        if (info.ArgCount != 2) return;
        var targetPlayer = info.ArgString.GetArg(0);
        var targetArgument = GetTargetArgument(targetPlayer);

        var players = GetPlayers()
               .Where(x =>
               (targetArgument == TargetForArgument.UserIdIndex
               ? GetUserIdIndex(targetPlayer) == x.UserId : targetArgument == TargetForArgument.Me ? x.SteamID == player.SteamID : false) || (x.PlayerName?.ToLower()?.Contains(targetPlayer?.ToLower()) ?? false))
               .ToList();
        if (players.Count == 0)
        {
            player.PrintToChat($"{Prefix} {CC.W}Eşleşen oyuncu bulunamadı!");
            return;
        }
        if (players.Count != 1)
        {
            player.PrintToChat($"{Prefix} {CC.W}Birden fazla oyuncu bulundu.");
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);

        var y = players.FirstOrDefault();
        if (ValidateCallerPlayer(y, false) == false) return;

        if (IsyanTeamPlayers.Any(x => x == y.SteamID))
        {
            player.PrintToChat($"{Prefix}{CC.B} {y.PlayerName}{CC.W} isimli oyuncu halihazirda isyan takımında.");
            return;
        }
        else
        {
            IsyanTeamPlayers.Add(y.SteamID);
            AddPlayerIsteamData(y.SteamID);
            SutTeamPlayers.Remove(y.SteamID);
            RemovePlayerSutteamData(y.SteamID);
            SetIsyanTeamClanTag(y);
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}{CC.B} {y.PlayerName}{CC.W} adlı oyuncuyu {CC.R}[İsyan Team]{CC.W}'e aldı");
        }
    }
}