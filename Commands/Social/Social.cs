using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region RR

    [ConsoleCommand("css_ip", "discord server")]
    public void ip(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        player!.PrintToChat($"{Prefix} {CC.W}IP: {CC.M}185.171.25.27 {CC.W}veya {CC.M}jb.zmtr.org");
    }

    [ConsoleCommand("grup", "steam grup")]
    [ConsoleCommand("grub", "steam grup")]
    [ConsoleCommand("steam", "steam grup")]
    [ConsoleCommand("steamgrup", "steam grup")]
    public void Grup(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false) return;
        PlayerGroupCheck(player, info);
    }

    [ConsoleCommand("discord", "discord server")]
    [ConsoleCommand("dc", "discord server")]
    public void dc(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        player!.PrintToChat($"{Prefix} {CC.W}Discord: discord.gg/wnxt3py");
    }

    [ConsoleCommand("web", "site")]
    [ConsoleCommand("site", "site")]
    public void web(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        player!.PrintToChat($"{Prefix} {CC.W}Site: zmtr.org");
    }

    [ConsoleCommand("magaza", "magaza")]
    [ConsoleCommand("supporter", "magaza")]
    public void magaza(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        player!.PrintToChat($"{Prefix} {CC.W}Mağaza: zmtr.org/magaza");
    }

    [ConsoleCommand("adminlik", "magaza")]
    public void adminlik(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        player!.PrintToChat($"{Prefix} {CC.W}Kurucu: steamcommunity.com/id/m4m1");
    }

    [ConsoleCommand("owner", "kurucu")]
    [ConsoleCommand("kurucu", "kurucu")]
    public void owner(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        player!.PrintToChat($"{Prefix} {CC.W}Kurucu: steamcommunity.com/id/m4m1");
    }

    #endregion RR
}