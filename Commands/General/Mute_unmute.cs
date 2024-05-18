using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static List<ulong> Unmuteds = new List<ulong>();

    [ConsoleCommand("unmute")]
    [CommandHelper(1, "<playerismi-@all-@t-@ct-@me-@alive-@dead>")]
    public void UnMute(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, Perm_Seviye10, Perm_Seviye10) == false)
        {
            return;
        }
        var target = info.ArgString.GetArgSkip(0);
        LogManagerCommand(player.SteamID, info.GetCommandString);

        UnMuteAction(player, target);
    }

    [ConsoleCommand("ume")]
    public void UnMuteMe(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, Perm_Seviye10, Perm_Seviye10) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        UnMuteAction(player, "@me");
    }

    [ConsoleCommand("uall")]
    public void UnMuteAll(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, Perm_Seviye10, Perm_Seviye10) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        UnMuteAction(player, "@all");
    }

    [ConsoleCommand("ut")]
    public void UnMuteT(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, Perm_Seviye10, Perm_Seviye10) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        UnMuteAction(player, "@t");
    }

    [ConsoleCommand("uct")]
    [ConsoleCommand("umct")]
    public void UnMuteCT(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, Perm_Seviye10, Perm_Seviye10) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        UnMuteAction(player, "@ct");
    }

    private static void UnMuteAction(CCSPlayerController? player, string target)
    {
        var targetArgument = GetTargetArgument(target);
        GetPlayers()
            .Where(x => GetTargetAction(x, target, player))
            .ToList()
            .ForEach(x =>
            {
                Unmuteds.Add(x.SteamID);
                if (TelliSeferActive || PatronuKoruActive)
                {
                    x.VoiceFlags &= ~VoiceFlags.All;
                    x.VoiceFlags &= ~VoiceFlags.ListenAll;
                }
                x.VoiceFlags &= ~VoiceFlags.Muted;
                if ((targetArgument & TargetForArgument.SingleUser) == targetArgument)
                {
                    Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncunun {CC.B}susturmasını{CC.W} kaldırdı.");
                }
            });
        if ((targetArgument & TargetForArgument.SingleUser) != targetArgument)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefinin {CC.B}susturmasını{CC.W} kaldırdı.");
        }
    }
}