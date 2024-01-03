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
}