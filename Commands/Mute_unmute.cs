using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static List<ulong> Unmuteds = new List<ulong>();

    [ConsoleCommand("unmute")]
    [CommandHelper(1, "<playerismi-@all-@t-@ct-@me-@alive-@dead>")]
    public void UnMute(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye10", "@css/seviye10") == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.ArgString.GetArg(0);

        UnMuteAction(player, target);
    }

    [ConsoleCommand("ume")]
    public void UnMuteMe(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye10", "@css/seviye10") == false)
        {
            return;
        }
        UnMuteAction(player, "@me");
    }

    [ConsoleCommand("ut")]
    public void UnMuteT(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye10", "@css/seviye10") == false)
        {
            return;
        }
        UnMuteAction(player, "@t");
    }

    [ConsoleCommand("uct")]
    [ConsoleCommand("umct")]
    public void UnMuteCT(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye10", "@css/seviye10") == false)
        {
            return;
        }
        UnMuteAction(player, "@ct");
    }

    private static void UnMuteAction(CCSPlayerController? player, string target)
    {
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
                TargetForArgument.None => x.PlayerName?.ToLowerInvariant()?.Contains(target?.ToLowerInvariant()) ?? false,
                TargetForArgument.UserIdIndex => GetUserIdIndex(target) == x.UserId,
                _ => false
            }
            && ValidateCallerPlayer(x, false))
            .ToList()
            .ForEach(x =>
            {
                Unmuteds.Add(x.SteamID);
                x.VoiceFlags &= ~VoiceFlags.Muted;
                if (targetArgument == TargetForArgument.None)
                {
                    Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncunun {CC.B}susturmasını{CC.W} kaldırdı.");
                }
            });
        if (targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefinin {CC.B}susturmasını{CC.W} kaldırdı.");
        }
    }
}