using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region AdminRefresh

    [ConsoleCommand("adminyetki")]
    [CommandHelper(minArgs: 3, usage: "<steamId> <nick> <cs2yetkiler> <cssyetkiler>", whoCanExecute: CommandUsage.SERVER_ONLY)]
    public void AdminYetki(CCSPlayerController? player, CommandInfo info)
    {
        return;
        Server.ExecuteCommand("C_reload_admins");
        Server.ExecuteCommand("css_admins_reload");
        Server.PrintToChatAll("Yetkiler yenilendi");
        if (false)
        {
            ReloadAllPlayerPermissions();
        }
    }

    #endregion AdminRefresh
}