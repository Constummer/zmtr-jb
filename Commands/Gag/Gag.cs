using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Dictionary<ulong, DateTime> Gags = new Dictionary<ulong, DateTime>();

    #region Gag

    [ConsoleCommand("gag")]
    [RequiresPermissions("@css/chat")]
    [CommandHelper(1, "<playerismi-@all-@t-@ct-@me-@alive-@dead>")]
    public void OnGagCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        var target = info.ArgCount > 1 ? info.GetArg(1) : null;
        if (target == null)
        {
            return;
        }
        var godOneTwoStr = "0";
        if (int.TryParse(godOneTwoStr, out var value) == false)
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
                if (value <= 0)
                {
                    if (Gags.TryGetValue(gagPlayer.SteamID, out var dateTime))
                    {
                        Gags[gagPlayer.SteamID] = DateTime.UtcNow.AddYears(1);
                    }
                    else
                    {
                        Gags.Add(gagPlayer.SteamID, DateTime.UtcNow.AddYears(1));
                    }
                    if (targetArgument == TargetForArgument.None)
                    {
                        Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{gagPlayer.PlayerName} {CC.B}Sınırsız{CC.W} gagladı.");
                    }
                }
            });
        if (targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{target} {CC.W}hedefini {CC.B}gagladı{CC.W}.");
        }
    }

    private bool GagChecker(CCSPlayerController player, string arg)
    {
        if (Gags.TryGetValue(player.SteamID, out var call))
        {
            if (call > DateTime.UtcNow)
            {
                player.PrintToChat($"{Prefix} {CC.W} GAGLISIN!");
                return true;
            }
            else
            {
                Gags.Remove(player.SteamID);
            }
        }
        if (PGags.Contains(player.SteamID))
        {
            player.PrintToChat($"{Prefix} {CC.W} SINIRSIZ GAGLISIN!");
            return true;
        }
        return false;
    }

    #endregion Gag
}