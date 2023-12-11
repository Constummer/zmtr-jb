using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Cvars;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Ff

    [ConsoleCommand("ffac", "ff acar")]
    public void FfAc(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        Ff(true);
    }

    [ConsoleCommand("ffkapa", "ff kapatir")]
    [ConsoleCommand("ffkapat", "ff kapatir")]
    public void FfKapa(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        Ff(false);
    }

    private static void Ff(bool ac)
    {
        var friendlyFire = ConVar.Find("mp_teammates_are_enemies");
        if (friendlyFire is null)
            return;

        friendlyFire.SetValue(ac);
        if (ac)
        {
            Server.PrintToChatAll("FF Kapalı");
        }
        else
        {
            Server.PrintToChatAll("FF Açık");
        }
    }

    private static void FfOld(bool ac)
    {
        var friendlyFire = ConVar.Find("mp_friendlyfire");
        if (friendlyFire is null)
            return;
        var teamid = ConVar.Find("sv_teamid_overhead");
        var teamidmax = ConVar.Find("sv_teamid_overhead_maxdist");

        bool ff = friendlyFire.GetPrimitiveValue<bool>();
        if (ff)
        {
            friendlyFire.SetValue(false);
            teamid?.SetValue(2000);
            teamidmax?.SetValue(1);
            Server.PrintToChatAll("FF Kapalı");
        }
        else
        {
            friendlyFire.SetValue(true);
            teamid?.SetValue(0);
            teamidmax?.SetValue(0);
            Server.PrintToChatAll("FF Açık");
        }
    }

    #endregion Ff
}