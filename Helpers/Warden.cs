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

    private static void RemoveWarden()
    {
        Server.NextFrame(() =>
        {
            WardenRefreshPawn();
            LastWardenMute();
            ClearLasers();
            CoinRemove();
            WardenLeaveSound();
            LatestWCommandUser = null;
        });
    }

    private static void IsEliWardenNotify()
    {
        var warden = GetWarden();
        if (warden != null)
        {
            warden.PrintToChat($" {CC.LR}[ZMTR] {CC.R} EĞER İSELİ {CC.W} ise");
            warden.PrintToChat($" {CC.LR}[ZMTR] {CC.B} !rm {CC.W}veya {CC.B}!revmenu {CC.W} yazarak ölen ctleri 3 kere revleyebilirsin");
        }
    }
}