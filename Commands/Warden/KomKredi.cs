using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region KomKredi

    [ConsoleCommand("komkredi")]
    public void KomKredi(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Config.Additional.KomPermName))
        {
            if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
            {
                player.PrintToChat($"{Prefix}{CC.W} Bu komutu sadece komutçular kullanabilir.");
                return;
            }
        }
        if (ValidateCallerPlayer(player, false) == false) return;

        LogManagerCommand(player.SteamID, info.GetCommandString);
        CheckAndGiveWeeklyKomCredit(player);
    }

    #endregion KomKredi
}