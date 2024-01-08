using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Dictionary<ulong, RuletData> RuletPlayers = new();

    private class RuletData
    {
        public int Credit { get; set; }
        public RuletOptions Option { get; set; }
    }

    private enum RuletOptions
    {
        None = 0,
        Yesil,
        Siyah,
        Kirmizi
    }

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
            player.PrintToChat($" {CC.Ol}[RULET] {CC.W}Yatırdığınız {CC.R}{amount}{CC.W} kredi vergisi kesilerek iade edildi");
            return;
        }
        else
        {
            player.PrintToChat($" {CC.Ol}[RULET] {CC.W}Rulet oynamamışsın. {CC.B}!rulet <kredi> <yeşil/siyah/kırmızı> {CC.W}yazarak oynayabilirsiniz.");
        }
    }

    [ConsoleCommand("rulet")]
    [CommandHelper(0, "<kredi> <yeşil/siyah/kırmızı>")]
    public void Rulet(CCSPlayerController? player, CommandInfo info)
    {
        if (RuletPlayers.TryGetValue(player.SteamID, out var ruletPlayCheck))
        {
            player.PrintToChat($" {CC.Ol}[RULET] {CcOfRulet(ruletPlayCheck.Option)}{ruletPlayCheck.Option} {CC.W}rengine {CC.G}{ruletPlayCheck.Credit} {CC.W}kredi bastın!");
            return;
        }
        var creditStr = info.GetArg(1);
        if (string.IsNullOrWhiteSpace(creditStr) || new String(creditStr.Where(Char.IsDigit).ToArray()).Length == 0)
        {
            player.PrintToChat($" {CC.Ol}[RULET] {CC.W}Kullanım = <kredi> <yeşil/siyah/kırmızı>!");
            var total = RuletPlayers.ToList().Select(x => x.Value.Credit).Sum();
            player.PrintToChat($" {CC.Ol}[RULET] {CC.W}Kasa = {total}");

            return;
        }

        if (!int.TryParse(creditStr, out int credit))
        {
            player.PrintToChat($" {CC.Ol}[RULET] {CC.DR}GEÇERSİZ MİKTAR!");
            return;
        }
        else
        {
            if (credit < 100 || credit > 2500)
            {
                player.PrintToChat($" {CC.Ol}[RULET] {CC.R}Min 100, Max 2500 kredi girebilirsin.");
                return;
            }
            else
            {
                if (info.ArgCount < 3)
                {
                    player.PrintToChat($" {CC.Ol}[RULET] {CC.W}Rulete katılmak için bir renk seçmelisin.");
                    player.PrintToChat($" {CC.Ol}[RULET] {CC.R}Kirmizi{CC.W}/{CC.R}K {CC.Ol}x2");
                    player.PrintToChat($" {CC.Ol}[RULET] {CC.G}Yeşil{CC.W}/{CC.G}Y {CC.Ol}x14");
                    player.PrintToChat($" {CC.Ol}[RULET] {CC.Gr}Siyah{CC.W}/{CC.Gr}S {CC.Ol}x2");
                    return;
                }
                var target = info.ArgString?.Split(credit.ToString())?[1]?.Trim() ?? "";

                target = target?.ToLower() ?? "";
                var opt = target switch
                {
                    "s" => RuletOptions.Siyah,
                    "siyah" => RuletOptions.Siyah,
                    "sıyah" => RuletOptions.Siyah,
                    "si" => RuletOptions.Siyah,
                    "y" => RuletOptions.Yesil,
                    "ye" => RuletOptions.Yesil,
                    "yesıl" => RuletOptions.Yesil,
                    "yesil" => RuletOptions.Yesil,
                    "yeşıl" => RuletOptions.Yesil,
                    "yeşil" => RuletOptions.Yesil,
                    "kırmızı" => RuletOptions.Kirmizi,
                    "kirmizi" => RuletOptions.Kirmizi,
                    "ki" => RuletOptions.Kirmizi,
                    "k" => RuletOptions.Kirmizi,
                    _ => RuletOptions.None,
                };
                if (opt == RuletOptions.None && target.StartsWith("s"))
                {
                    opt = RuletOptions.Siyah;
                }
                else if (opt == RuletOptions.None && target.StartsWith("y"))
                {
                    opt = RuletOptions.Yesil;
                }
                else if (opt == RuletOptions.None && target.StartsWith("k"))
                {
                    opt = RuletOptions.Kirmizi;
                }

                if (opt == RuletOptions.None)
                {
                    player.PrintToChat($" {CC.Ol}[RULET] {CC.W}HATALI RENK.");
                    player.PrintToChat($" {CC.Ol}[RULET] {CC.R}Kirmizi{CC.W}/{CC.R}K {CC.Ol}x2");
                    player.PrintToChat($" {CC.Ol}[RULET] {CC.G}Yeşil{CC.W}/{CC.G}Y {CC.Ol}x14");
                    player.PrintToChat($" {CC.Ol}[RULET] {CC.Gr}Siyah{CC.W}/{CC.Gr}S {CC.Ol}x2");
                    return;
                }

                if (RuletPlayers.TryGetValue(player.SteamID, out var ruletPlay))
                {
                    player.PrintToChat($" {CC.Ol}[RULET] {CcOfRulet(ruletPlay.Option)}{ruletPlay.Option} {CC.W}rengine {CC.G}{ruletPlay.Credit} {CC.W}kredi bastın, değiştiremezsin!");
                    return;
                }
                else
                {
                    var data = GetPlayerMarketModel(player.SteamID);

                    if (data.Model == null || data.Model.Credit < credit || data.Model.Credit - credit < 0)
                    {
                        player.PrintToChat($" {CC.Ol}[RULET] {CC.W}Yetersiz Bakiye!");
                        return;
                    }

                    ruletPlay = new() { Option = opt, Credit = credit };
                    RuletPlayers.Add(player.SteamID, ruletPlay);
                    data.Model!.Credit -= credit;
                    PlayerMarketModels[player.SteamID] = data.Model;
                    Server.PrintToChatAll($" {CC.Ol}[RULET] {CC.Ol}{player.PlayerName} {CcOfRulet(opt)}{opt} {CC.W}rengine {CC.G}{credit} {CC.W}kredi bastı!");
                    player.PrintToChat($" {CC.Ol}[RULET] {CcOfRulet(opt)}{opt} {CC.W}rengine {CC.G}{credit} {CC.W}kredi bastın!");
                    player.PrintToChat($" {CC.Ol}[RULET] {CC.W}Güncel Kredin: {CC.G}{data.Model!.Credit}");
                }
            }
        }
    }

    private void RuletActivate()
    {
        var kazananRenk = RuletDondur();
        Server.PrintToChatAll($" {CC.Ol}[RULET] {CC.W}Rulet Dönüyor...");
        Server.PrintToChatAll($" {CC.Ol}[RULET] {CC.W}Ruleti Kazanan Renk: {CcOfRulet(kazananRenk)}{kazananRenk}");

        GetPlayers()
            .Where(x => RuletPlayers.ContainsKey(x.SteamID))
            .ToList()
            .ForEach(x =>
            {
                if (RuletPlayers.TryGetValue(x.SteamID, out var enteredCredit) == false)
                {
                    return;
                }
                // Kazanan renk ve sonuç bildirilir
                if (enteredCredit.Option != kazananRenk)
                {
                    x.PrintToChat($" {CC.Ol}[RULET] {CC.DR}Üzgünüm, {CcOfRulet(kazananRenk)}{kazananRenk} kazandı. {CC.G}{enteredCredit.Credit} {CC.W}kredi kaybettin!");
                }
                else
                {
                    if (kazananRenk == RuletOptions.Yesil)
                    {
                        var win = (int)((enteredCredit.Credit * 14) * 0.9);
                        x.PrintToChat($" {CC.Ol}[RULET]{CC.G} Tebrikler! {CcOfRulet(kazananRenk)}{kazananRenk} {CC.G}kazandı.{CC.W} Ruletten {CC.B}{win}{CC.W} kredi kazandın!");
                        var data = GetPlayerMarketModel(x.SteamID);
                        if (data.Model == null)
                        {
                            return;
                        }

                        data.Model!.Credit += win;
                        PlayerMarketModels[x.SteamID] = data.Model;
                    }
                    else
                    {
                        bool kazandiMi = RuletSonucunuKontrolEt(kazananRenk);

                        if (kazandiMi)
                        {
                            var win = (int)((enteredCredit.Credit * 2) * 0.9);
                            x.PrintToChat($" {CC.Ol}[RULET]{CC.G} Tebrikler! {CcOfRulet(kazananRenk)}{kazananRenk} {CC.G}kazandı.{CC.W} Ruletten {CC.B}{win}{CC.W} kredi kazandın!");
                            var data = GetPlayerMarketModel(x.SteamID);
                            if (data.Model == null)
                            {
                                return;
                            }

                            data.Model!.Credit += win;
                            PlayerMarketModels[x.SteamID] = data.Model;
                        }
                        else
                        {
                            x.PrintToChat($" {CC.Ol}[RULET] {CC.DR}Üzgünüm, {CcOfRulet(kazananRenk)}{kazananRenk} kazandı. {CC.G}{enteredCredit.Credit} {CC.W}kredi kaybettin!");
                        }
                    }
                }
            });
        RuletPlayers.Clear();

        static bool RuletSonucunuKontrolEt(RuletOptions kazananRenk)
        {
            // Kazanan renge göre kazanıp kaybedildiği kontrol edilir
            if (kazananRenk == RuletOptions.Siyah || kazananRenk == RuletOptions.Kirmizi)
            {
                // Siyah ve kırmızı renkler x2 kazanç sağlar
                return true;
            }
            else
            {
                // Yeşil renk x14 kazanç sağlar
                return false;
            }
        }
        static RuletOptions RuletDondur()
        {
            int sayi = _random.Next(1, 101);

            if (sayi <= 2)
            {
                return RuletOptions.Yesil;
            }
            else if (sayi <= 51)
            {
                return RuletOptions.Siyah;
            }
            else
            {
                return RuletOptions.Kirmizi;
            }
        }
    }

    private char CcOfRulet(RuletOptions data) => data switch
    {
        RuletOptions.Yesil => CC.G,
        RuletOptions.Kirmizi => CC.R,
        RuletOptions.Siyah => CC.Gr,
        _ => CC.B
    };

    #endregion Rulet
}