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
            player.PrintToChat($" {CC.LR}[ZMTR] {CC.B}Sadece {CC.W} Komutçu bu menüyü açabilir");
            return;
        }
        if (CurrentCtRespawns >= 3)
        {
            player.PrintToChat($" {CC.LR}[ZMTR] {CC.B}En Fazla {CC.R}3 kere {CC.W} respawn atabilirsin");
            return;
        }
        var revmenu = new ChatMenu("Rev Menü");

        var players = GetPlayers(CsTeam.CounterTerrorist)
            .Where(x => x.PawnIsAlive == false)
            .ToList();
        if (players == null || players.Count == 0)
        {
            player.PrintToChat($" {CC.LR}[ZMTR] {CC.W} Revlenecek Hiç Ölü CT yok");
        }
        else
        {
            players.ForEach(x =>
               {
                   revmenu.AddMenuOption(x.PlayerName, (p, t) =>
                   {
                       CustomRespawn(x);
                       CurrentCtRespawns++;
                       Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.B}{x.PlayerName} {CC.W} Rev menüden revlendi | Son {3 - CurrentCtRespawns} rev");
                   });
               });
            ChatMenus.OpenMenu(player, revmenu);
        }
    }

    #endregion RevMenu
}