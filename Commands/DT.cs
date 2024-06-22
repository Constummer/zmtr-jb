using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Disarm

    [ConsoleCommand("dt")]
    public void DisarmTemizle(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (!AdminManager.PlayerHasPermissions(player, Perm_Premium))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        TemizleAction(player);
        GetPlayers(CsTeam.Terrorist)
        .Where(x => x.PawnIsAlive)
        .ToList()
        .ForEach(x =>
        {
            RemoveWeapons(x, true);
        });
        Server.PrintToChatAll($"{Prefix} {CC.W}{T_PluralCamelPossesive} silahları silindi.");
    }

    #endregion Disarm
}