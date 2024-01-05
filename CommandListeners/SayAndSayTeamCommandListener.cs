using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using System.ComponentModel.Design;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void SayAndSayTeamCommandListener()
    {
        AddCommandListener("say", OnSayOrSayTeam);
        AddCommandListener("say_team", OnSayOrSayTeam);
    }

    private HookResult OnSayOrSayTeam(CCSPlayerController? player, CommandInfo info)
    {
        if (player == null) return HookResult.Continue;
        var arg = info.GetArg(1);
        if (string.IsNullOrWhiteSpace(arg) || arg.Replace(" ", string.Empty) == string.Empty)
            return HookResult.Handled;
        if (arg.StartsWith("!") || arg.StartsWith("/"))
        {
            if (VoteInProgressIntercepter(player, arg) == true)
            {
                return HookResult.Handled;
            }
            if (KomutcuAdminSay(player, info) == true)
            {
                return HookResult.Handled;
            }

            return HookResult.Continue;
        }
        if (ValidateCallerPlayer(player, false) == false)
        {
            return HookResult.Continue;
        }
        if (KomutcuAdminSay(player, info) == true)
        {
            return HookResult.Handled;
        }
        if (LevelSystemPlayer(player, info) == true)
        {
            return HookResult.Handled;
        }
        return HookResult.Continue;
    }
}