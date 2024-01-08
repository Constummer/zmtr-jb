using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Dictionary<ulong, int> PiyangoPlayers = new();

    #region Piyango

    [ConsoleCommand("piyango")]
    [CommandHelper(1, "<kredi>")]
    public void Piyango(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }

        var creditStr = info.GetArg(1);

        if (!int.TryParse(creditStr, out int credit))
        {
            player.PrintToChat($"{Prefix} {CC.R}GEÇERSİZ MİKTAR");
            return;
        }
        else
        {
            if (credit < 10 || credit > 1000)
            {
                player.PrintToChat($"{Prefix} {CC.R}Min 10, Max 1000 kredi girebilirsin");
                return;
            }
            else
            {
                if (PiyangoPlayers.TryGetValue(player.SteamID, out var enteredCredit))
                {
                    PiyangoPlayers[player.SteamID] = credit;
                }
                else
                {
                    PiyangoPlayers.Add(player.SteamID, credit);
                }
            }
        }
    }

    public void KazananiBelirle()
    {
        Console.WriteLine("Çekiliş Başlıyor...");

        // Rastgele bir oyuncu seçimi
        ulong kazananOyuncuId = 0;

        if (PiyangoPlayers.Count > 0)
        {
            int kazananIndex = _random.Next(0, PiyangoPlayers.Count);
            kazananOyuncuId = new List<ulong>(PiyangoPlayers.Keys)[kazananIndex];
        }

        // Kazananı açıklama
        if (kazananOyuncuId != 0)
        {
            Console.WriteLine($"Oyuncu {kazananOyuncuId} kazandı! Kazandığı kredi miktarı: {PiyangoPlayers[kazananOyuncuId]}");
        }
        else
        {
            Console.WriteLine("Üzgünüz, kimse kazanamadı. Hiçbir oyuncu kredi yatırmamış olabilir.");
        }
    }

    #endregion Piyango
}