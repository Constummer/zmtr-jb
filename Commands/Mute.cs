using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("mute")]
    [CommandHelper(1, "<playerismi-@all-@t-@ct-@me-@alive-@dead>")]
    public void Mute(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye10", "@css/seviye10") == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.ArgString.GetArg(0);
        MuteAction(player, target);
    }

    [ConsoleCommand("mt")]
    public void MuteT(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye10", "@css/seviye10") == false)
        {
            return;
        }
        MuteAction(player, "@t");
    }

    [ConsoleCommand("mct")]
    public void MuteCT(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye10", "@css/seviye10") == false)
        {
            return;
        }
        MuteAction(player, "@ct");
    }

    private static void MuteAction(CCSPlayerController? player, string target)
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
                TargetForArgument.None => x.PlayerName?.ToLower()?.Contains(target) ?? false,
                TargetForArgument.UserIdIndex => GetUserIdIndex(target) == x.UserId,
                _ => false
            }
            && ValidateCallerPlayer(x, false))
            .ToList()
            .ForEach(x =>
            {
                x.VoiceFlags |= VoiceFlags.Muted;
                if (targetArgument == TargetForArgument.None)
                {
                    Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu {CC.B}susturdu{CC.W}.");
                }
            });
        if (targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefini {CC.B}susturdu{CC.W}.");
        }
    }
}