using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Piyango

    [ConsoleCommand("jackpotiptal")]
    [ConsoleCommand("jackpotayril")]
    [ConsoleCommand("jackpotsil")]
    [ConsoleCommand("jackpotcik")]
    [ConsoleCommand("piyangoiptal")]
    [ConsoleCommand("piyangoayril")]
    [ConsoleCommand("piyangosil")]
    [ConsoleCommand("piyangocik")]
    public void Piyangoptal(CCSPlayerController? player, CommandInfo info)
    {
        if (PiyangoPlayers.TryGetValue(player.SteamID, out var ruletPlay))
        {
            var data = GetPlayerMarketModel(player.SteamID);
            if (data.Model == null)
            {
                return;
            }
            var amount = (int)(ruletPlay * 0.9);
            data.Model!.Credit += amount;
            PlayerMarketModels[player.SteamID] = data.Model;

            PiyangoPlayers.Remove(player.SteamID, out _);
            player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Yatırdığınız {CC.R}{amount}{CC.W} kredi vergisi kesilerek iade edildi.");
            return;
        }
        else
        {
            player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Piyango oynamamışsın. {CC.B}!piyango{CC.W} veya {CC.B}!jackpot <kredi> {CC.W}yazarak oynayabilirsiniz.");
        }
    }

    #endregion Piyango
}