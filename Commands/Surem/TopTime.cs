using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region TopTime

    [ConsoleCommand("toptime")]
    [ConsoleCommand("topsure")]
    public void TopTime(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        player.PrintToChat($" {CC.LR}[ZMTR] {CC.W}TOP SURELER SUAN DISABLE DURUMDA, YAKINDA ACILACAK!");

        //var amount = 0;
        //var ordered=PlayerTimeTracking.OrderBy(x=>x.)
        //if (player?.SteamID != null && player!.SteamID != 0)
        //{
        //    if (PlayerTimeTracking.TryGetValue(player.SteamID, out var item))
        //    {
        //        amount = item.Total;
        //    }
        //}
        //player.PrintToChat($" {CC.LR}[ZMTR] {CC.W}Toplam {CC.G}{amount} {CC.W}dakikadır sunucudasın!");
    }

    #endregion TopTime
}