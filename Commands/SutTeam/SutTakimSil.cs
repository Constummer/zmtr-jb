using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("suttakimkaldir")]
    [ConsoleCommand("suttakimsil")]
    [ConsoleCommand("sutteamkaldir")]
    [ConsoleCommand("sutteamsil")]
    [CommandHelper(minArgs: 1, "<sut takimdan silinecek kişi>")]
    public void SutTakimKaldir(CCSPlayerController? player, CommandInfo info)
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

        if (SutTeamPlayers.Any(x => x == y.SteamID) == false)
        {
            player.PrintToChat($"{Prefix}{CC.B} {y.PlayerName}{CC.W} isimli oyuncu süt takımında değil.");
            return;
        }
        else
        {
            SutTeamPlayers.Remove(y.SteamID);
            RemovePlayerSutteamData(y.SteamID);
            y.Clan = null;
            SetStatusClanTag(y);

            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}{CC.B} {y.PlayerName}{CC.W} adlı oyuncuyu {CC.R}[Süt Team]{CC.W}'dan çıkardı");
        }
    }
}