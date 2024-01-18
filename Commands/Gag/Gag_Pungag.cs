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
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
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
        var targetArgument = GetTargetArgument(target);
        GetPlayers()
            .Where(x => targetArgument switch
            {
                TargetForArgument.All => true,
                TargetForArgument.T => GetTeam(x) == CsTeam.Terrorist,
                TargetForArgument.Ct => GetTeam(x) == CsTeam.CounterTerrorist,
                TargetForArgument.Me => player.PlayerName == x.PlayerName,
                TargetForArgument.Alive => x.PawnIsAlive,
                TargetForArgument.Dead => x.PawnIsAlive == false,
                TargetForArgument.None => x.PlayerName?.ToLower()?.Contains(target) ?? false,
                TargetForArgument.UserIdIndex => GetUserIdIndex(target) == x.UserId,
                _ => false
            }
            && ValidateCallerPlayer(x, false))
            .ToList()
            .ForEach(gagPlayer =>
            {
                PGags = PGags.Where(x => x != gagPlayer.SteamID).ToList();
                RemoveFromPGag(gagPlayer.SteamID);

                if (targetArgument == TargetForArgument.None)
                {
                    Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{gagPlayer.PlayerName} {CC.W}pgagını kaldırdı.");
                }
            });
        if (targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}pgagını kaldırdı.");
        }
    }

    #endregion UnGag
}