using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("guns")]
    [ConsoleCommand("silah")]
    [ConsoleCommand("silahlar")]
    public void FfSilahMenu(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        if (FFMenuCheck == true)
        {
            ChatMenus.OpenMenu(player!, GunMenu);
        }
        else if (IsEliMenuCheck == true)
        {
            if (GetTeam(player) == CsTeam.CounterTerrorist)
            {
                ChatMenus.OpenMenu(player!, GunMenu);
            }
        }
        else
        {
            player!.PrintToChat($"{Prefix} {CC.W}FF veya Iseli açık olmadığı için silah menüsüne erişemezsin.");
            return;
        }
    }
}