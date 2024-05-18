using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("mute")]
    [CommandHelper(1, "<playerismi-@all-@t-@ct-@me-@alive-@dead>")]
    public void Mute(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, Perm_Seviye10, Perm_Seviye10) == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        LogManagerCommand(player.SteamID, info.GetCommandString);
        var target = info.ArgString.GetArg(0);
        MuteAction(player, target);
    }

    [ConsoleCommand("mt")]
    public void MuteT(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, Perm_Seviye10, Perm_Seviye10) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        MuteAction(player, "@t");
    }

    [ConsoleCommand("mct")]
    public void MuteCT(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, Perm_Seviye10, Perm_Seviye10) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        MuteAction(player, "@ct");
    }

    [ConsoleCommand("mall")]
    public void MuteALL(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, Perm_Seviye10, Perm_Seviye10) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        MuteAction(player, "@all");
    }

    [ConsoleCommand("mme")]
    public void MuteMe(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, Perm_Seviye10, Perm_Seviye10) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        MuteAction(player, "@me");
    }

    private static void MuteAction(CCSPlayerController? player, string target)
    {
        var targetArgument = GetTargetArgument(target);
        GetPlayers()
            .Where(x => GetTargetAction(x, target, player))
            .ToList()
            .ForEach(x =>
            {
                if (TelliSeferActive || PatronuKoruActive)
                {
                    x.VoiceFlags |= VoiceFlags.ListenTeam;
                    x.VoiceFlags |= VoiceFlags.Team;
                }
                x.VoiceFlags |= VoiceFlags.Muted;
                if ((targetArgument & TargetForArgument.SingleUser) == targetArgument)
                {
                    Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu {CC.B}susturdu{CC.W}.");
                }
            });
        if ((targetArgument & TargetForArgument.SingleUser) != targetArgument)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefini {CC.B}susturdu{CC.W}.");
        }
    }
}