using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region KomDk

    public DateTime? KomStartTime { get; set; } = null;

    [ConsoleCommand("komdk")]
    public void KomDk(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false) return;
        if (KomStartTime == null)
        {
            player.PrintToChat($"{Prefix} {CC.W}Mevcutta kom yok");
            return;
        }

        var substract = DateTime.UtcNow - KomStartTime.Value;
        TimeSpan remainingTime = TimeSpan.FromMinutes(45) - substract;
        if (remainingTime.TotalMinutes > 45)
        {
            player.PrintToChat($"{Prefix} {CC.W} Komdk atılabilinir.");
            return;
        }
        else
        {
            player.PrintToChat($"{Prefix} {CC.W} Komdk'ya daha {CC.B}{(new DateTime(remainingTime.Ticks).ToString("mm:ss"))}{CC.W} süre var.");
            return;
        }
    }

    [ConsoleCommand("komdkyap")]
    [ConsoleCommand("komdkbaslat")]
    [ConsoleCommand("komdkbasla")]
    [ConsoleCommand("komdkac")]
    public void KomDkBaslat(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false) return;
        if (KomStartTime == null)
        {
            player.PrintToChat($"{Prefix} {CC.W}Mevcutta kom yok");
            return;
        }

        var substract = DateTime.UtcNow - KomStartTime.Value;
        TimeSpan remainingTime = TimeSpan.FromMinutes(45) - substract;
        if (remainingTime.TotalMinutes > 45)
        {
            KomStartTime = DateTime.UtcNow;
            VoteAction(player, "Komutçu Değiş Kal");
            return;
        }
        else
        {
            player.PrintToChat($"{Prefix} {CC.W} Komdk'ya daha {CC.B}{(new DateTime(remainingTime.Ticks).ToString("mm:ss"))}{CC.W} süre var.");
            return;
        }
    }

    #endregion KomDk
}