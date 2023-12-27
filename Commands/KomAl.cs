using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Komal

    private static bool KomActive = false;
    private static List<ulong> KomAdays = new List<ulong>();

    [ConsoleCommand("komal")]
    [ConsoleCommand("domal")]
    public void KomAl(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye10"))
        {
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR]{ChatColors.White} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        KomActive = false;
        KomAdays?.Clear();
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}Komutçu alımı başladı! Komutçu adayı olmak için !komaday yazın.");
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}Komutçu alımı başladı! Komutçu adayı olmak için !komaday yazın.");
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}Komutçu adaylığından ayrılmak için !komadayiptal yazın.");
        KomActive = true;
        var now = DateTime.UtcNow;
        AddTimer(30f, () =>
        {
            KomActive = false;
            Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green} komaday sure bitti.");
        });
    }

    [ConsoleCommand("komaday")]
    public void KomAday(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (KomActive)
        {
            if (KomAdays.Count > 6)
            {
                KomActive = false;
                player.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.White}Komaday doldu.");
            }
            else
            {
                if (KomAdays.Any(x => x == player.SteamID))
                {
                    player.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.White}Zaten katıldın!");
                }
                else
                {
                    KomAdays.Add(player.SteamID);
                    player.VoiceFlags &= ~VoiceFlags.Muted;
                    Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName} {ChatColors.White}komutçu adayı oldu.");
                }
            }
        }
        else
        {
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.Darkred}Komutçu alımı aktif değil.");
        }
    }

    [ConsoleCommand("komadayiptal")]
    public void KomAdayIptal(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye10"))
        {
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR]{ChatColors.White} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        player.VoiceFlags |= VoiceFlags.Muted;
        KomAdays = KomAdays.Where(x => x != player.SteamID).ToList();
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Darkred}{player.PlayerName} {ChatColors.White}Komutçu adaylığından ayrıldı.");
    }

    #endregion Komal
}