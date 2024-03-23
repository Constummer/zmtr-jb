using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using MySqlConnector;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("ReloadBadWordsForKom")]
    public void ReloadBadWordsForKom(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        try
        {
            using (var con = Connection())
            {
                var cmd = (MySqlCommand)null;
                if (con == null)
                {
                    return;
                }
                GetAllKomKalanInterceptorDatas(con);
            }
        }
        catch
        {
        }
    }
}