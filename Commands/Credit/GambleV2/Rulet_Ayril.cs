using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Rulet

    [ConsoleCommand("ruletiptal")]
    [ConsoleCommand("ruletayril")]
    [ConsoleCommand("ruletsil")]
    [ConsoleCommand("ruletcik")]
    public void RuletIptal(CCSPlayerController? player, CommandInfo info)
    {
        if (RuletPlayers.TryGetValue(player.SteamID, out var ruletPlay))
        {
            var data = GetPlayerMarketModel(player.SteamID);
            if (data.Model == null)
            {
                return;
            }
            var amount = (int)(ruletPlay.Credit * 0.9);
            data.Model!.Credit += amount;
            PlayerMarketModels[player.SteamID] = data.Model;

            RuletPlayers.Remove(player.SteamID, out _);
            player.PrintToChat($" {CC.Ol}[RULET] {CC.W}Yatırdığınız {CC.R}{amount}{CC.W} kredi vergisi kesilerek iade edildi.");
            return;
        }
        else
        {
            player.PrintToChat($" {CC.Ol}[RULET] {CC.W}Rulet oynamamışsın. {CC.B}!rulet <kredi> <yeşil/siyah/kırmızı> {CC.W}yazarak oynayabilirsiniz.");
        }
    }

    #endregion Rulet
}