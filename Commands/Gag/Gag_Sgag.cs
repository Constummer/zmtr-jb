using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Gag

    [ConsoleCommand("sgag")]
    [CommandHelper(2, "<playerismi-@all-@t-@ct-@me-@alive-@dead> [dakika]")]
    public void OnSGagCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye10", "@css/seviye10") == false)
        {
            return;
        }
        var target = info.ArgCount > 1 ? info.ArgString.GetArg(0) : null;
        if (target == null)
        {
            return;
        }
        var godOneTwoStr = info.ArgCount > 2 ? info.ArgString.GetArg(1) : "0";
        if (int.TryParse(godOneTwoStr, out var value) == false || value <= 0)
        {
            Server.PrintToChatAll($"{Prefix} {CC.G}geçersiz süre.");

            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);

        var targetArgument = GetTargetArgument(target);
        GetPlayers()
            .Where(x => GetTargetAction(x, target, player))
            .ToList()
            .ForEach(gagPlayer =>
            {
                if (Gags.TryGetValue(gagPlayer.SteamID, out var dateTime))
                {
                    Gags[gagPlayer.SteamID] = DateTime.UtcNow.AddMinutes(value);
                }
                else
                {
                    Gags.Add(gagPlayer.SteamID, DateTime.UtcNow.AddMinutes(value));
                }
                if ((targetArgument & TargetForArgument.SingleUser) == targetArgument)
                {
                    Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{gagPlayer.PlayerName} {CC.B}{value}{CC.W} dakika boyunca gagladı.");
                }
            });
        if ((targetArgument & TargetForArgument.SingleUser) != targetArgument)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefini {CC.B} {value}{CC.W} dakika gagladı.");
        }
    }

    #endregion Gag
}