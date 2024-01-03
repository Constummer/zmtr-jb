using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;

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
        if (KomutcuAdminSay(player, info) == true)
        {
            return HookResult.Handled;
        }
        return HookResult.Continue;
    }
}