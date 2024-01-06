using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;

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
            player.PrintToChat($"{Prefix} {CC.B}Sadece {CC.W} Komutçu bu menüyü açabilir");
            return;
        }
        var players = GetPlayers()
        .Where(x => AdminManager.PlayerHasPermissions(x, "@css/admin1"))
        .ToList();

        if (players.Count == 0)
        {
            player.PrintToChat($"{Prefix} {CC.W}Aktif hiç admin bulunamadı");
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
                Server.PrintToChatAll($"{Prefix} {CC.B}{x.PlayerName} {CC.P} [Komutçu Admin]{CC.W} Olarak seçildi");
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
        KomutcuAdminId = null;
    }

    public static bool KomutcuAdminSay(CCSPlayerController? player, CommandInfo info)
    {
        var teamColor = GetTeam(player) switch
        {
            CsTeam.CounterTerrorist => CC.BG,
            CsTeam.Terrorist => CC.Y,
            CsTeam.Spectator => CC.LP,
            CsTeam.None => CC.Or,
        };
        var chatColor = GetTeam(player) switch
        {
            CsTeam.CounterTerrorist => CC.BG,
            CsTeam.Terrorist => CC.Y,
            CsTeam.Spectator => CC.P,
            CsTeam.None => CC.Or,
        };
        if (KomutcuAdminId == player.SteamID)
        {
            Server.PrintToChatAll($" {CC.P}[Komutçu Admin] {teamColor}{player.PlayerName} {CC.W}: {chatColor}{info.GetArg(1)}");
            return true;
        }
        return false;
    }

    #endregion KomutcuAdmin
}