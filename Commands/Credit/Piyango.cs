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
            player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Yatırdığınız {CC.R}{amount}{CC.W} kredi vergisi kesilerek iade edildi.");
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
        if (KumarKapatDisable)
        {
            player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Bu mübarek günde, yakışıyor mu müslüman din kardeşim - piyango kapalı -.");
            return;
        }
        if (IsGameBannedToday())
        {
            player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Bu mübarek günde, yakışıyor mu müslüman din kardeşim - piyango kapalı -.");
            return;
        }
        var total = PiyangoPlayers.ToList().Select(x => x.Value).Sum();

        var creditStr = info.GetArg(1);
        if (string.IsNullOrWhiteSpace(creditStr) || new String(creditStr.Where(Char.IsDigit).ToArray()).Length == 0)
        {
            player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Kullanım = {CC.G}!piyango <kredi>");
            player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Kasa = {total}");
            return;
        }

        if (!int.TryParse(creditStr, out int credit))
        {
            player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.R}GEÇERSİZ MİKTAR");
            player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Kasa = {total}");
            return;
        }
        else
        {
            if (credit < 10 || credit > 50)
            {
                player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.R}Min 10, Max 50 kredi girebilirsin");
                player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Kasa = {total}");
                return;
            }
            else
            {
                if (PiyangoPlayers.TryGetValue(player.SteamID, out var enteredCredit))
                {
                    player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.B}{enteredCredit} {CC.W}kredi bastın, değiştiremezsin!");
                    player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Kasa = {total}");
                    return;
                }
                else
                {
                    var data = GetPlayerMarketModel(player.SteamID);

                    if (data.Model == null || data.Model.Credit < credit || data.Model.Credit - credit < 0)
                    {
                        player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Yetersiz Bakiye!");
                        player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Kasa = {total}");
                        return;
                    }

                    PiyangoPlayers.Add(player.SteamID, credit);
                    data.Model!.Credit -= credit;
                    PlayerMarketModels[player.SteamID] = data.Model;
                    Server.PrintToChatAll($" {CC.Ol}[PİYANGO] {CC.Ol}{player.PlayerName} {CC.W}Piyango'ya{CC.G} {credit} {CC.W}kredi ile katıldı!");
                    player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.G} {credit} {CC.W}kredi bastın!");
                    player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Güncel Kredin: {CC.G}{data.Model!.Credit}");
                    player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Kasa = {total + credit}");
                }
            }
        }
    }

    public void PiyangoKazananSonuc()
    {
        if (IsGameBannedToday())
        {
            return;
        }
        var piyangoKazanan = PiyangoKazanan();
        if (piyangoKazanan == 0)
        {
            return;
        }
        int total = (int)(PiyangoPlayers.ToList().Select(x => x.Value).Sum() * 0.9);

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

        data.Model!.Credit += total;
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