using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static ulong? KomutcuAdminId = null;
    #region KomutcuAdmin

    [ConsoleCommand("ka")]
    [ConsoleCommand("komutcuadmin ")]
    public void KomutcuAdmin(CCSPlayerController? player, CommandInfo info)
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
        var players = GetPlayers()
        .Where(x => AdminManager.PlayerHasPermissions(x, "@css/admin1"))
        .ToList();

        if (players.Count == 0)
        {
            player.PrintToChat($" {CC.LR}[ZMTR] {CC.W}Aktif hiç admin bulunamadı");
            return;
        }
        var kaMenu = new ChatMenu("Komutçu Admin Menü");
        kaMenu.AddMenuOption("Seçeceğin Admin Komutçu Admin Olacak!", null, true);
        players.ForEach(x =>
        {
            kaMenu.AddMenuOption(x.PlayerName, (p, t) =>
            {
                KomutcuAdminId = x.SteamID;
                x.Clan = $"{CC.P}[Komutçu Admin]";
                AddTimer(0.2f, () =>
                {
                    Utilities.SetStateChanged(x, "CCSPlayerController", "m_szClan");
                    Utilities.SetStateChanged(x, "CBasePlayerController", "m_iszPlayerName");
                });
                Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.B}{x.PlayerName} {CC.P} [Komutçu Admin]{CC.W} Olarak seçildi");
            });
        });
        ChatMenus.OpenMenu(player, kaMenu);
    }

    private static void CleanTagOnKomutcuAdmin()
    {
        if (KomutcuAdminId == null)
            return;
        var ka = GetPlayers().Where(x => ValidateCallerPlayer(x, false) && x.SteamID == KomutcuAdminId).FirstOrDefault();
        if (ka != null)
        {
            ka.Clan = "";
            //AddTimer(0.2f, () =>
            //{
            //    Utilities.SetStateChanged(ka, "CCSPlayerController", "m_szClan");
            //    Utilities.SetStateChanged(ka, "CBasePlayerController", "m_iszPlayerName");
            //});
        }
    }

    #endregion KomutcuAdmin
}