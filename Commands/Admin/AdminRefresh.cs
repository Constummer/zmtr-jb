using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region AdminRefresh

    [ConsoleCommand("adminrefresh")]
    [CommandHelper(whoCanExecute: CommandUsage.SERVER_ONLY)]
    public void AdminRefresh(CCSPlayerController? player, CommandInfo info)
    {
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