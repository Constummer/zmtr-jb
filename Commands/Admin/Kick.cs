using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Kick

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

        string reason = "Unknown";
        if (info.ArgCount >= 2 && info.ArgString.GetArg(1).Length > 0)
        {
            reason = info.ArgString.GetArg(1);
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var target = info.ArgString.GetArg(0);

        var players = GetPlayers()
                      .Where(x => GetTargetAction(x, target, player.PlayerName))
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
        var x = players.FirstOrDefault();
        if (x != null)
        {
            if (x.SteamID != player.SteamID)
            {
                if (info.ArgCount >= 2)
                {
                    KickPlayer((ushort)x.UserId!, reason);
                }
                else
                {
                    KickPlayer((ushort)x.UserId!);
                }
            }
        }
    }

    public static void KickPlayer(ushort userId, string? reason = null)
    {
        NativeAPI.IssueServerCommand($"kickid {userId} {reason}");
    }

    #endregion Kick
}