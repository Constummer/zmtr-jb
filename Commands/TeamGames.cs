using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("tgdurdur")]
    [ConsoleCommand("tgiptal")]
    [ConsoleCommand("iptaltg")]
    [ConsoleCommand("canceltg")]
    [ConsoleCommand("durdurtg")]
    [ConsoleCommand("durdurteamgames")]
    [ConsoleCommand("teamgamesdurdur")]
    public void OnTeamGamesCancelCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (LatestWCommandUser != player.SteamID)
        {
            if (!AdminManager.PlayerHasPermissions(player, "@css/lider"))
            {
                player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
                return;
            }
        }
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        TeamGamesCancel();
    }

    [ConsoleCommand("teamgames")]
    [ConsoleCommand("tg")]
    public void OnTeamGamesCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (LatestWCommandUser != player.SteamID)
        {
            if (!AdminManager.PlayerHasPermissions(player, "@css/lider"))
            {
                player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
                return;
            }
        }
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        InitializeTG(player);
    }
}