using CounterStrikeSharp.API;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Dictionary<ulong, int> PiyangoPlayers = new();

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
        int total = (int)(PiyangoPlayers.ToList().Select(x => x.Value).Sum() * 0.93);

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
}