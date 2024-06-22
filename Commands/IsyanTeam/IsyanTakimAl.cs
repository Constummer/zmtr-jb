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
        if (!AdminManager.PlayerHasPermissions(player, Perm_Yonetim))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }

        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        var target = info.ArgString.GetArgSkip(0);
        if (FindSinglePlayer(player, target, out var y) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);

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