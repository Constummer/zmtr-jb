using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

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
        if (ac)
        {
            Server.ExecuteCommand("mp_teammates_are_enemies 1");
            Server.PrintToChatAll("FF Açık");
        }
        else
        {
            Server.ExecuteCommand("mp_teammates_are_enemies 0");
            Server.PrintToChatAll("FF Kapalı");
        }
    }

    #endregion Ff
}