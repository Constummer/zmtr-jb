using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region KomutcuAdminSal

    [ConsoleCommand("kasal")]
    [ConsoleCommand("komutcuadminsal")]
    public void KomutcuAdminSal(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        if (KomutcuAdminId != player.SteamID)
        {
            player.PrintToChat($"{Prefix} {CC.B}Sadece {CC.W} [Komutçu Admin] bu menüyü açabilir");
            return;
        }

        var x = GetPlayers().Where(x => x.SteamID == KomutcuAdminId).FirstOrDefault();
        if (x == null)
        {
            player.PrintToChat($"{Prefix} {CC.W}Komutçu admin belirlenmemiş");
            player.PrintToChat($"{Prefix} {CC.B}!ka {CC.W},{CC.B}!komutcuadmin");
            player.PrintToChat($"{Prefix} {CC.W}Yazarak komutçu admin belirleyebilirsin");
            return;
        }
        var players = GetPlayers()
                        .Where(x => x.SteamID != KomutcuAdminId
                                    && AdminManager.PlayerHasPermissions(x, BasePermission))
                        .ToList();

        if (players.Count == 0)
        {
            player.PrintToChat($"{Prefix} {CC.W}Aktif hiç admin bulunamadı");
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        var kaMenu = new ChatMenu("Komutçu Admin Menü");
        kaMenu.AddMenuOption("Seçeceğin Admin Komutçu Admin Olacak!", null, true);
        players.ForEach(x =>
        {
            kaMenu.AddMenuOption(x.PlayerName, (p, t) =>
            {
                if (ValidateCallerPlayer(x, false) == false) return;
                KomutcuAdminId = x.SteamID;
                x.Clan = $"[Komutçu Admin]";
                SetStatusClanTag(x);

                Server.PrintToChatAll($"{Prefix} {CC.B}{player.PlayerName} {CC.P} [Komutçu Admin]{CC.W}'liğini {CC.B}{x.PlayerName} {CC.W}'e saldı.");
            });
        });
        MenuManager.OpenChatMenu(player, kaMenu);
    }

    #endregion KomutcuAdminSal
}