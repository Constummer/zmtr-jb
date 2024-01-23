using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Kick

    [ConsoleCommand("css_kick")]
    [ConsoleCommand("kick")]
    [CommandHelper(minArgs: 1, usage: "<#userid or name> [reason]")]
    public void OnKickCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (AdminManager.PlayerHasPermissions(player, "@css/lider") == false)
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        Server.PrintToChatAll("1");
        var target = info.ArgString.GetArg(0);
        Server.PrintToChatAll("2");

        var players = GetPlayers()
                      .Where(x => GetTargetAction(x, target, player.PlayerName))
                      .ToList();
        Server.PrintToChatAll("3");

        if (players.Count == 0)
        {
            Server.PrintToChatAll("4");

            player.PrintToChat($"{Prefix} {CC.W}Eşleşen oyuncu bulunamadı!");
            return;
        }
        Server.PrintToChatAll("5");

        if (players.Count != 1)
        {
            player.PrintToChat($"{Prefix} {CC.W}Birden fazla oyuncu bulundu.");
            return;
        }
        Server.PrintToChatAll("6");

        var x = players.FirstOrDefault();
        Server.PrintToChatAll("7");

        if (ValidateCallerPlayer(x, false) == false) return;
        Server.PrintToChatAll("8");

        if (x != null)
        {
            Server.PrintToChatAll("9");
            if (ValidateCallerPlayer(x, false) == false) return;
            if (x.UserId.HasValue && x.UserId > -1)
            {
                Server.ExecuteCommand($"kickid {x.UserId}");
            }
        }
    }

    #endregion Kick
}