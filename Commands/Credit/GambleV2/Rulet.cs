using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Rulet

    [ConsoleCommand("rulet")]
    [CommandHelper(0, "<kredi> <yeşil/siyah/kırmızı>")]
    public void Rulet(CCSPlayerController? player, CommandInfo info)
    {
        if (KumarKapatDisable)
        {
            player.PrintToChat($" {CC.Ol}[RULET] {CC.W} - rulet kapalı -.");
            return;
        }
        if (IsGameBannedToday())
        {
            player.PrintToChat($" {CC.Ol}[RULET] {CC.W}Bu mübarek günde, yakışıyor mu müslüman din kardeşim - rulet kapalı -.");
            return;
        }
        var total = RuletPlayers.ToList().Select(x => x.Value.Credit).Sum();
        if (RuletPlayers.TryGetValue(player.SteamID, out var ruletPlayCheck))
        {
            player.PrintToChat($" {CC.Ol}[RULET] {CtOfRulet(ruletPlayCheck.Option)}- {CC.W}rengine {CC.G}{ruletPlayCheck.Credit} {CC.W}kredi bastın!");
            player.PrintToChat($" {CC.Ol}[RULET] {CC.W}Kasa = {total}");
            return;
        }
        var creditStr = info.ArgString.GetArg(0);
        if (string.IsNullOrWhiteSpace(creditStr) || new String(creditStr.Where(Char.IsDigit).ToArray()).Length == 0)
        {
            player.PrintToChat($" {CC.Ol}[RULET] {CC.W}Kullanım ={CC.G} !rulet <kredi> <yeşil/siyah/kırmızı>");
            player.PrintToChat($" {CC.Ol}[RULET] {CC.W}Kasa = {total}");

            return;
        }

        if (!int.TryParse(creditStr, out int credit))
        {
            player.PrintToChat($" {CC.Ol}[RULET] {CC.DR}GEÇERSİZ MİKTAR!");
            player.PrintToChat($" {CC.Ol}[RULET] {CC.W}Kasa = {total}");
            return;
        }
        else
        {
            if (credit < 100 || credit > 2500)
            {
                player.PrintToChat($" {CC.Ol}[RULET] {CC.R}Min 100, Max 2500 kredi girebilirsin.");
                player.PrintToChat($" {CC.Ol}[RULET] {CC.W}Kasa = {total}");
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
                    player.PrintToChat($" {CC.Ol}[RULET] {CC.W}Kasa = {total}");
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
                    player.PrintToChat($" {CC.Ol}[RULET] {CC.R}Kırmızı{CC.W}/{CC.R}K {CC.Ol}x2");
                    player.PrintToChat($" {CC.Ol}[RULET] {CC.G}Yeşil{CC.W}/{CC.G}Y {CC.Ol}x14");
                    player.PrintToChat($" {CC.Ol}[RULET] {CC.Gr}Siyah{CC.W}/{CC.Gr}S {CC.Ol}x2");
                    player.PrintToChat($" {CC.Ol}[RULET] {CC.W}Kasa = {total}");
                    return;
                }

                if (RuletPlayers.TryGetValue(player.SteamID, out var ruletPlay))
                {
                    player.PrintToChat($" {CC.Ol}[RULET] {CtOfRulet(ruletPlay.Option)} {CC.W}rengine {CC.G}{ruletPlay.Credit} {CC.W}kredi bastın, değiştiremezsin!");
                    player.PrintToChat($" {CC.Ol}[RULET] {CC.W}Kasa = {total}");
                    return;
                }
                else
                {
                    var data = GetPlayerMarketModel(player.SteamID);

                    if (data.Model == null || data.Model.Credit < credit || data.Model.Credit - credit < 0)
                    {
                        player.PrintToChat($" {CC.Ol}[RULET] {CC.W}Yetersiz Bakiye!");
                        player.PrintToChat($" {CC.Ol}[RULET] {CC.W}Kasa = {total}");
                        return;
                    }
                    RuletV2OnPlayerBet(player.SteamID, credit, opt);
                    ruletPlay = new() { Option = opt, Credit = credit };
                    RuletPlayers.Add(player.SteamID, ruletPlay);
                    data.Model!.Credit -= credit;
                    PlayerMarketModels[player.SteamID] = data.Model;
                    Server.PrintToChatAll($" {CC.Ol}[RULET] {CC.Ol}{player.PlayerName} {CtOfRulet(opt)} {CC.W}rengine {CC.G}{credit} {CC.W}kredi bastı!");
                    player.PrintToChat($" {CC.Ol}[RULET] {CtOfRulet(opt)} {CC.W}rengine {CC.G}{credit} {CC.W}kredi bastın!");
                    player.PrintToChat($" {CC.Ol}[RULET] {CC.W}Güncel Kredin: {CC.G}{data.Model!.Credit}");
                    player.PrintToChat($" {CC.Ol}[RULET] {CC.W}Kasa = {total + credit}");
                }
            }
        }
    }

    private void RuletActivate()
    {
        if (IsGameBannedToday())
        {
            return;
        }
        var kazananRenk = RuletDondur();
        RuletV2RoundEnd(kazananRenk);

        Server.PrintToChatAll($" {CC.Ol}[RULET] {CC.W}Rulet Dönüyor...");
        Server.PrintToChatAll($" {CC.Ol}[RULET] {CC.W}Ruleti Kazanan Renk: {CtOfRulet(kazananRenk)}");

        var players = GetPlayers()
            .Where(x => RuletPlayers.ContainsKey(x.SteamID))
            .ToList();
        foreach (var x in players)
        {
            if (RuletPlayers.TryGetValue(x.SteamID, out var enteredCredit) == false)
            {
                continue;
            }
            // Kazanan renk ve sonuç bildirilir
            if (enteredCredit.Option != kazananRenk)
            {
                x.PrintToChat($" {CC.Ol}[RULET] {CC.DR}Üzgünüm, {CtOfRulet(kazananRenk)} kazandı. {CC.G}{enteredCredit.Credit} {CC.W}kredi kaybettin!");
            }
            else
            {
                if (kazananRenk == RuletOptions.Yesil)
                {
                    var win = (int)((enteredCredit.Credit * 14) * 0.95);
                    x.PrintToChat($" {CC.Ol}[RULET]{CC.G} Tebrikler! {CtOfRulet(kazananRenk)} {CC.G}kazandı.{CC.W} Ruletten {CC.B}{win}{CC.W} kredi kazandın!");
                    var data = GetPlayerMarketModel(x.SteamID);
                    if (data.Model == null)
                    {
                        continue;
                    }

                    data.Model!.Credit += win;
                    PlayerMarketModels[x.SteamID] = data.Model;
                }
                else
                {
                    bool kazandiMi = RuletSonucunuKontrolEt(kazananRenk);

                    if (kazandiMi)
                    {
                        var win = (int)((enteredCredit.Credit * 2) * 0.95);
                        x.PrintToChat($" {CC.Ol}[RULET]{CC.G} Tebrikler! {CtOfRulet(kazananRenk)} {CC.G}kazandı.{CC.W} Ruletten {CC.B}{win}{CC.W} kredi kazandın!");
                        var data = GetPlayerMarketModel(x.SteamID);
                        if (data.Model == null)
                        {
                            continue;
                        }

                        data.Model!.Credit += win;
                        PlayerMarketModels[x.SteamID] = data.Model;
                    }
                    else
                    {
                        x.PrintToChat($" {CC.Ol}[RULET] {CC.DR}Üzgünüm, {CtOfRulet(kazananRenk)} kazandı. {CC.G}{enteredCredit.Credit} {CC.W}kredi kaybettin!");
                    }
                }
            }
        };
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

            if (sayi <= 4)
            {
                return RuletOptions.Yesil;
            }
            else if (sayi <= 53)
            {
                return RuletOptions.Siyah;
            }
            else
            {
                return RuletOptions.Kirmizi;
            }
        }
    }

    private string CtOfRulet(RuletOptions data) => data switch
    {
        RuletOptions.Yesil => $"{CC.G}Yeşil",
        RuletOptions.Kirmizi => $"{CC.R}Kırmızı",
        RuletOptions.Siyah => $"{CC.Gr}Siyah",
        _ => ""
    };

    #endregion Rulet
}