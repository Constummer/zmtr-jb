using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region KomDk

    public DateTime? KomStartTime { get; set; } = null;

    [ConsoleCommand("komdk")]
    [ConsoleCommand("komutcudk")]
    public void KomDk(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false) return;

        player.PrintToChat($"{Prefix} {CC.B}!komkalan {CC.W}komutunu kullanmalısın");
        return;
    }

    [ConsoleCommand("komkalan")]
    [ConsoleCommand("komsure")]
    public void KomKalan(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false) return;
        if (KomStartTime == null)
        {
            player.PrintToChat($"{Prefix} {CC.W}Mevcutta kom yok");
            return;
        }

        var substract = (DateTime.UtcNow - KomStartTime.Value).TotalSeconds;
        TimeSpan remainingTime = TimeSpan.FromMinutes(45) - TimeSpan.FromSeconds(substract);
        if (remainingTime.TotalSeconds > 0)
        {
            var dk = $"{CC.B}{remainingTime.ToString("mm")}{CC.W}";
            var sn = $"{CC.B}{remainingTime.ToString("ss")}{CC.W}";
            player.PrintToChat($"{Prefix} {CC.W} Komdk'ya daha {dk} dakika {sn} saniye var.");
        }
        else
        {
            player.PrintToChat($"{Prefix} {CC.W} Komdk atılabilir.");
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
        var substract = (DateTime.UtcNow - KomStartTime.Value).TotalSeconds;
        TimeSpan remainingTime = TimeSpan.FromMinutes(45) - TimeSpan.FromSeconds(substract);
        if (remainingTime.TotalSeconds > 0)
        {
            var dk = $"{CC.B}{remainingTime.ToString("mm")}{CC.W}";
            var sn = $"{CC.B}{remainingTime.ToString("ss")}{CC.W}";
            player.PrintToChat($"{Prefix} {CC.W} Komdk'ya daha {dk} dakika {sn} saniye var.");
        }
        else
        {
            KomStartTime = DateTime.UtcNow;
            VoteAction(player, "Komutçu Değiş Kal");
        }
    }

    #endregion KomDk
}