using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Dictionary<ulong, int> PiyangoPlayers = new();

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
            player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Yatırdığınız {CC.R}{amount}{CC.W} kredi vergisi kesilerek iade edildi");
            return;
        }
        else
        {
            player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Piyango oynamamışsın. {CC.B}!piyango{CC.W} veya {CC.B}!jackpot <kredi> {CC.W}yazarak oynayabilirsiniz.");
        }
    }

    [ConsoleCommand("jackpot")]
    [ConsoleCommand("piyango")]
    [CommandHelper(0, "<kredi>")]
    public void Piyango(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }

        var creditStr = info.GetArg(1);
        if (string.IsNullOrWhiteSpace(creditStr) || new String(creditStr.Where(Char.IsDigit).ToArray()).Length == 0)
        {
            var total = PiyangoPlayers.ToList().Select(x => x.Value).Sum();
            player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Kasa = {total}");
        }

        if (!int.TryParse(creditStr, out int credit))
        {
            player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.R}GEÇERSİZ MİKTAR");
            return;
        }
        else
        {
            if (credit < 10 || credit > 100)
            {
                player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.R}Min 10, Max 100 kredi girebilirsin");
                return;
            }
            else
            {
                if (PiyangoPlayers.TryGetValue(player.SteamID, out var enteredCredit))
                {
                    player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.B}{enteredCredit} {CC.W}kredi bastın, değiştiremezsin!");
                    return;
                }
                else
                {
                    var data = GetPlayerMarketModel(player.SteamID);

                    if (data.Model == null || data.Model.Credit < credit || data.Model.Credit - credit < 0)
                    {
                        player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Yetersiz Bakiye!");
                        return;
                    }

                    PiyangoPlayers.Add(player.SteamID, credit);
                    data.Model!.Credit -= credit;
                    PlayerMarketModels[player.SteamID] = data.Model;
                    Server.PrintToChatAll($" {CC.Ol}[PİYANGO] {CC.Ol}{player.PlayerName} {CC.G}{credit} {CC.W}kredi bastı!");
                    player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.G}{credit} {CC.W}kredi bastın!");
                    player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Güncel Kredin: {CC.G}{data.Model!.Credit}");
                }
            }
        }
    }

    public void PiyangoKazananSonuc()
    {
        var piyangoKazanan = PiyangoKazanan();
        double total = PiyangoPlayers.ToList().Select(x => x.Value).Sum();

        Server.PrintToChatAll($" {CC.Ol}[PİYANGO] {CC.W}Piyango Açıklanıyor...");
        _ = PiyangoPlayers.TryGetValue(piyangoKazanan, out var amount);
        var piyangoKazananPlayer = GetPlayers().Where(x => x.SteamID == piyangoKazanan).FirstOrDefault();
        if (piyangoKazananPlayer == null)
        {
            if (PlayerNamesDatas.TryGetValue(piyangoKazanan, out var player))
            {
                Server.PrintToChatAll($" {CC.Ol}[PİYANGO] {CC.W}Piyango Kazanan: {player} | {CC.B}{total} {CC.W}Kazandı");
            }
            return;
        }

        Server.PrintToChatAll($" {CC.Ol}[PİYANGO] {CC.W}Piyango Kazanan: {piyangoKazananPlayer.PlayerName} | {CC.B}{total} {CC.W}Kazandı");
        var data = GetPlayerMarketModel(piyangoKazanan);
        if (data.Model == null)
        {
            return;
        }

        data.Model!.Credit += (int)(amount * 0.9);
        PlayerMarketModels[piyangoKazanan] = data.Model;
        PiyangoPlayers.Clear();

        ulong PiyangoKazanan()
        {
            // Toplam değeri hesaplama
            double total = PiyangoPlayers.ToList().Select(x => x.Value).Sum();

            // Oranları hesaplama
            var percentages = new Dictionary<ulong, double>();
            foreach (var item in PiyangoPlayers)
            {
                double percentage = (item.Value / total) * 100;

                percentages.Add(item.Key, percentage);
            }

            // Rastgele bir eleman seçme
            double randomValue = _random.NextDouble() * 100; // 0 ile 100 arasında rastgele bir değer
            double cumulativePercentage = 0;
            foreach (var item in percentages)
            {
                cumulativePercentage += item.Value;
                if (randomValue <= cumulativePercentage)
                {
                    return item.Key;
                }
            }
            return 0;
        }
    }

    #endregion Piyango
}