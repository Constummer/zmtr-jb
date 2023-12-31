using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region RevMenu

    [ConsoleCommand("revmenu")]
    public void RevMenu(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false || LatestWCommandUser != player.SteamID)
        {
            return;
        }
        var revmenu = new ChatMenu("Rev Menü");

        GetPlayers(CsTeam.CounterTerrorist)
            .Where(x => x.PawnIsAlive == false)
            .ToList()
            .ForEach(x =>
            {
                revmenu.AddMenuOption(x.PlayerName, (p, t) =>
                {
                    Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Blue}{x.PlayerName} {ChatColors.White} revmenu'den revlendi");
                    x.Respawn();
                });
            });
        ChatMenus.OpenMenu(player, revmenu);
    }

    #endregion RevMenu
}