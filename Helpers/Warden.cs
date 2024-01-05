using CounterStrikeSharp.API;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static void WardenRefreshPawn()
    {
        var warden = GetWarden();
        if (warden != null && warden.PawnIsAlive)
        {
            return;
            RefreshPawn(warden);
        }
    }

    private static void WardenUnmute()
    {
        var warden = GetWarden();
        if (warden != null)
        {
            warden.VoiceFlags &= ~VoiceFlags.Muted;
        }
    }

    private static void LastWardenMute()
    {
        var warden = GetWarden();
        if (warden != null)
        {
            warden.VoiceFlags |= VoiceFlags.Muted;
        }
    }

    private void RemoveWarden()
    {
        Server.NextFrame(() =>
        {
            WardenRefreshPawn();
            LastWardenMute();
            ClearLasers();
            CoinRemove();
            WardenLeaveSound();
            LatestWCommandUser = null;
            CleanTagOnKomutcuAdmin();
        });
    }

    private static void IsEliWardenNotify()
    {
        var warden = GetWarden();
        if (warden != null)
        {
            warden.PrintToChat($" {CC.LR}[ZMTR] {CC.W} eğer {CC.R}İSELİ {CC.W} ise");
            warden.PrintToChat($" {CC.LR}[ZMTR] {CC.B} !rm {CC.W}veya {CC.B}!revmenu {CC.W} ile menüden seçerek veya");
            warden.PrintToChat($" {CC.LR}[ZMTR] {CC.B} !rmf {CC.W} HIZLICA ");
            warden.PrintToChat($" {CC.LR}[ZMTR] {CC.W} Ölen ctleri 3 kere revleyebilirsin");
        }
    }
}