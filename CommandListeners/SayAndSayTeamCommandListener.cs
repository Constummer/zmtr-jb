using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using System.ComponentModel.Design;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void SayAndSayTeamCommandListener()
    {
        AddCommandListener("say", OnSay);
        AddCommandListener("say_team", OnSayTeam);
    }

    private HookResult OnSayTeam(CCSPlayerController? player, CommandInfo commandInfo)
    {
        return OnSayOrSayTeam(player, commandInfo, true);
    }

    private HookResult OnSay(CCSPlayerController? player, CommandInfo commandInfo)
    {
        return OnSayOrSayTeam(player, commandInfo, false);
    }

    private HookResult OnSayOrSayTeam(CCSPlayerController? player, CommandInfo info, bool isSayTeam)
    {
        if (player == null) return HookResult.Continue;
        var arg = info.GetArg(1);
        Server.PrintToConsole($"{player.SteamID}|{player.PlayerName} : {arg}");

        if (string.IsNullOrWhiteSpace(arg) || arg.Replace(" ", string.Empty) == string.Empty)
            return HookResult.Handled;

        if (ValidateCallerPlayer(player, false) == false)
        {
            return HookResult.Continue;
        }
        if (arg.StartsWith("!") || arg.StartsWith("/") || arg.StartsWith("css_"))
        {
            if (KomKalanIntercepter(player, arg))
            {
                return HookResult.Handled;
            }
            if (VoteInProgressIntercepter(player, arg) == true)
            {
                return HookResult.Handled;
            }
            if (GagChecker(player, arg, true, isSayTeam))
            {
                return HookResult.Handled;
            }
            if (OnSteamGroupPlayerChat(player, arg))
            {
                return HookResult.Handled;
            }
            return HookResult.Continue;
        }
        if (KomdkIntercepter(player, isSayTeam, arg))
        {
            return HookResult.Handled;
        }
        if (GagChecker(player, arg))
        {
            return HookResult.Handled;
        }
        if (KomutcuAdminSay(player, info, isSayTeam))
        {
            return HookResult.Handled;
        }
        if (LatestWCommandUser == player.SteamID)
        {
            WardenSay(player, info, isSayTeam);
            return HookResult.Handled;
        }
        if (CustomTagSay(player, info, isSayTeam))
        {
            return HookResult.Handled;
        }
        if (LevelSystemPlayer(player, info, isSayTeam))
        {
            return HookResult.Handled;
        }
        return HookResult.Continue;
    }
}