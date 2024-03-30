using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region UnGag

    [ConsoleCommand("pungag")]
    [ConsoleCommand("unpgag")]
    [CommandHelper(1, "<playerismi-@all-@t-@ct-@me-@alive-@dead>")]
    public void OnPunGagCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/lider"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        var target = info.ArgCount > 1 ? info.ArgString.GetArg(0) : null;
        if (target == null)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);

        var targetArgument = GetTargetArgument(target);
        GetPlayers()
            .Where(x => GetTargetAction(x, target, player))
            .ToList()
            .ForEach(gagPlayer =>
            {
                PGags = PGags.Where(x => x != gagPlayer.SteamID).ToList();
                RemoveFromPGag(gagPlayer.SteamID);

                if ((targetArgument & TargetForArgument.SingleUser) == targetArgument)
                {
                    Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{gagPlayer.PlayerName} {CC.W}pgagını kaldırdı.");
                }
            });
        if ((targetArgument & TargetForArgument.SingleUser) != targetArgument)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}pgagını kaldırdı.");
        }
    }

    #endregion UnGag
}