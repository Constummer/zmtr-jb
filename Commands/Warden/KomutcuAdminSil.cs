using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region KomutcuAdminSil

    [ConsoleCommand("kasil")]
    [ConsoleCommand("komutcuadminsil")]
    public void KomutcuAdminSil(CCSPlayerController? player, CommandInfo info)
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

        if (KomutcuAdminId == null || KomutcuAdminId <= 0)
        {
            player.PrintToChat($"{Prefix} {CC.W}Komutçu admin belirlenmemiş");
            player.PrintToChat($"{Prefix} {CC.B}!ka {CC.W},{CC.B}!komutcuadmin");
            player.PrintToChat($"{Prefix} {CC.W}Yazarak komutçu admin belirleyebilirsin");
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
        if (ValidateCallerPlayer(x, false) == false) return;
        KomutcuAdminId = null;
        x.Clan = null;
        AddTimer(0.2f, () =>
        {
            if (ValidateCallerPlayer(x, false) == false) return;
            Utilities.SetStateChanged(x, "CCSPlayerController", "m_szClan");
            Utilities.SetStateChanged(x, "CBasePlayerController", "m_iszPlayerName");
        });
        Server.PrintToChatAll($"{Prefix} {CC.B}{x.PlayerName} {CC.P} [Komutçu Admin]{CC.W}'likten çıkartıldı");
    }

    #endregion KomutcuAdminSil
}