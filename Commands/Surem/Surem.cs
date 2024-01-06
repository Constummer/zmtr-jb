using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Surem

    [ConsoleCommand("surem")]
    public void Surem(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        var item = (PlayerTime)null;
        if (player?.SteamID != null && player!.SteamID != 0)
        {
            if (PlayerTimeTracking.TryGetValue(player.SteamID, out item) == false)
            {
            }
        }
        if (item == null)
        {
            player.PrintToChat($"{Prefix} {CC.W} ------===------------===------");
            player.PrintToChat($"{Prefix} {CC.W}Toplam {CC.G}1 {CC.W}saattir sunucudasın!");
            player.PrintToChat($"{Prefix} {CC.W} ------===------------===------");
            return;
        }
        player.PrintToChat($"{Prefix} {CC.W} ------===------------===------");
        player.PrintToChat($"{Prefix} {CC.W}Toplam {CC.G}{item.Total / 60} {CC.W}saattir sunucudasın!");
        player.PrintToChat($"{Prefix} {CC.W}Toplam {CC.G}{item.CTTime / 60} {CC.W}saattir gardiyansın!");
        player.PrintToChat($"{Prefix} {CC.W}Toplam {CC.G}{item.TTime / 60} {CC.W}saattir teroristsin!");
        player.PrintToChat($"{Prefix} {CC.W}Toplam {CC.G}{item.WTime / 60} {CC.W}saattir komutçusun!");
        player.PrintToChat($"{Prefix} {CC.W} ------===------------===------");
    }

    #endregion Surem
}