using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("suttakimal")]
    [ConsoleCommand("suttakimver")]
    [ConsoleCommand("sutteamal")]
    [ConsoleCommand("sutteamver")]
    [CommandHelper(minArgs: 1, "<sut takima eklenecek kişi>")]
    public void SutTakimVer(CCSPlayerController? player, CommandInfo info)
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

        if (SutTeamPlayers.Any(x => x == y.SteamID))
        {
            player.PrintToChat($"{Prefix}{CC.B} {y.PlayerName}{CC.W} isimli oyuncu halihazirda sut takımında.");
            return;
        }
        else
        {
            SutTeamPlayers.Add(y.SteamID);
            AddPlayerSutteamData(y.SteamID);
            IsyanTeamPlayers.Remove(y.SteamID);
            RemovePlayerIsteamData(y.SteamID);
            SetSutTeamClanTag(y);
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}{CC.B} {y.PlayerName}{CC.W} adlı oyuncuyu {CC.R}[Süt Team]{CC.W}'e aldı");
        }
    }
}