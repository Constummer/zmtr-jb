using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static int CurrentCtRespawns = 0;

    #region RevMenu

    [ConsoleCommand("revmenu")]
    [ConsoleCommand("rm")]
    public void RevMenu(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        if (LatestWCommandUser != player.SteamID)
        {
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.Blue}Sadece {ChatColors.White} Komutçu bu menüyü açabilir");
            return;
        }
        if (CurrentCtRespawns >= 3)
        {
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.Blue}En Fazla {ChatColors.Red}3 kere {ChatColors.White} respawn atabilirsin");
            return;
        }
        var revmenu = new ChatMenu("Rev Menü");

        var players = GetPlayers(CsTeam.CounterTerrorist)
            .Where(x => x.PawnIsAlive == false)
            .ToList();
        if (players == null || players.Count == 0)
        {
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.White} Revlenecek Hiç Ölü CT yok");
        }
        else
        {
            players.ForEach(x =>
               {
                   revmenu.AddMenuOption(x.PlayerName, (p, t) =>
                   {
                       Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Blue}{x.PlayerName} {ChatColors.White} Rev menüden revlendi");
                       CustomRespawn(x);
                       CurrentCtRespawns++;
                   });
               });
            ChatMenus.OpenMenu(player, revmenu);
        }
    }

    #endregion RevMenu
}