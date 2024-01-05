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

        var amount = 0;
        if (player?.SteamID != null && player!.SteamID != 0)
        {
            if (PlayerTimeTracking.TryGetValue(player.SteamID, out var item))
            {
                amount = item.Total;
            }
        }
        player.PrintToChat($" {CC.LR}[ZMTR] {CC.W}Toplam {CC.G}{amount} {CC.W}dakikadır sunucudasın!");
    }

    #endregion Surem
}