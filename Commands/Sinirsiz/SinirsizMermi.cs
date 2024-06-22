using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private bool UnlimitedReserverAmmoActive = false;

    #region SinirsizMermi

    [ConsoleCommand("sinirsizmermiac")]
    [ConsoleCommand("smac")]
    public void SinirsizMermiAc(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.W} sınırsız mermi açtı.");
        LogManagerCommand(player.SteamID, info.GetCommandString);

        UnlimitedReserverAmmoActive = true;
    }

    [ConsoleCommand("smac2")]
    public void SinirsizMermiAc2(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.W} sınırsız mermi 2 deistirdi.");
        LogManagerCommand(player.SteamID, info.GetCommandString);

        Config.Additional.UnlimitedReserver = !Config.Additional.UnlimitedReserver;
    }

    #endregion SinirsizMermi
}