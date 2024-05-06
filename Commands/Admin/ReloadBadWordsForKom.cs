using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("ReloadBadWordsForKom")]
    public void ReloadBadWordsForKom(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player!.PrintToChat(NotEnoughPermission);
            return;
        }
        LogManagerCommand(player!.SteamID, info.GetCommandString);

        try
        {
            using (var con = Connection())
            {
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