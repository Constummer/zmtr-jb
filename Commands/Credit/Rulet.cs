using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using System.Numerics;
using System.Runtime.CompilerServices;

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
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }

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
            player.PrintToChat($"{Prefix} {CC.W}Yatırdığınız {CC.R}{amount}{CC.W} kredi vergili iade edildi");
            return;
        }
        else
        {
            player.PrintToChat($"{Prefix} {CC.W}Rulet oynamamışsınız {CC.B}!rulet <kredi> <yeşil/siyah/kırmızı> {CC.W}yazarak oynayabilirsiniz.");
        }
    }

    [ConsoleCommand("rulet")]
    [CommandHelper(1, "<kredi> <yeşil/siyah/kırmızı>")]
    public void Rulet(CCSPlayerController? player, CommandInfo info)
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
            if (credit < 100 || credit > 5000)
            {
                player.PrintToChat($"{Prefix} {CC.R}Min 100, Max 5000 kredi girebilirsin");
                return;
            }
            else
            {
                if (info.ArgCount != 3)
                {
                    player.PrintToChat($"{Prefix} {CC.W}Ruletini hangi renge vereceğini yazman lazım.");
                    player.PrintToChat($"{Prefix} {CC.R}Kirmizi{CC.W}/{CC.R}K");
                    player.PrintToChat($"{Prefix} {CC.G}Yeşil{CC.W}/{CC.G}Y");
                    player.PrintToChat($"{Prefix} {CC.DB}Siyah{CC.W}/{CC.DB}S");
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
                if (opt == RuletOptions.None)
                {
                    player.PrintToChat($"{Prefix} {CC.W}HATALI RENK.");
                    player.PrintToChat($"{Prefix} {CC.R}Kirmizi{CC.W}/{CC.R}K");
                    player.PrintToChat($"{Prefix} {CC.G}Yeşil{CC.W}/{CC.G}Y");
                    player.PrintToChat($"{Prefix} {CC.DB}Siyah{CC.W}/{CC.DB}S");
                    return;
                }

                if (RuletPlayers.TryGetValue(player.SteamID, out var ruletPlay))
                {
                    player.PrintToChat($"{Prefix} {CcOfRulet(ruletPlay.Option)}{ruletPlay.Option} {CC.W} seçeneğine {CC.B}{ruletPlay.Credit} {CC.W}kredi oynamıştın, değiştiremezsin!");
                    return;
                }
                else
                {
                    var data = GetPlayerMarketModel(player.SteamID);

                    if (data.Model == null || data.Model.Credit < credit || data.Model.Credit - credit < 0)
                    {
                        player.PrintToChat($"{Prefix} {CC.W}Yetersiz Bakiye!");
                        return;
                    }

                    ruletPlay = new() { Option = opt, Credit = credit };
                    RuletPlayers.Add(player.SteamID, ruletPlay);
                    data.Model!.Credit -= credit;
                    PlayerMarketModels[player.SteamID] = data.Model;
                    Server.PrintToChatAll($"{Prefix} {CC.W}Rulet: {player.PlayerName} {CcOfRulet(opt)}{opt} {CC.W} seçeneğine {CC.B}{credit} {CC.W}kredi oynadı!");
                    player.PrintToChat($"{Prefix} {CcOfRulet(opt)}{opt} {CC.W} seçeneğine {CC.B}{credit} {CC.W}kredi oynadın!");
                    player.PrintToChat($"{Prefix} {CC.W} Kalan Kredin = {CC.B}{data.Model!.Credit}");
                }
            }
        }
    }

    private void RuletActivate()
    {
        var kazananRenk = RuletDondur();
        Server.PrintToChatAll($"{Prefix} {CC.W}Rulet Kazanan renk: {CcOfRulet(kazananRenk)}{kazananRenk}");

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
                    x.PrintToChat($"{Prefix} Üzgünüm, {CcOfRulet(kazananRenk)}{kazananRenk} kazandı! {enteredCredit.Credit} kredi kaybettin!");
                }
                else
                {
                    if (kazananRenk == RuletOptions.Yesil)
                    {
                        x.PrintToChat($"{Prefix}{CC.W} Tebrikler {CcOfRulet(kazananRenk)}{kazananRenk} {CC.G}kazandın!{CC.W} Ruletten {CC.B}{enteredCredit.Credit * 14}{CC.W} kredi kazandın!");
                        var data = GetPlayerMarketModel(x.SteamID);
                        if (data.Model == null)
                        {
                            return;
                        }

                        data.Model!.Credit += enteredCredit.Credit * 14;
                        PlayerMarketModels[x.SteamID] = data.Model;
                    }
                    else
                    {
                        bool kazandiMi = RuletSonucunuKontrolEt(kazananRenk);

                        if (kazandiMi)
                        {
                            var win = enteredCredit.Credit * 2;
                            x.PrintToChat($"{Prefix}{CC.W} Tebrikler {CcOfRulet(kazananRenk)}{kazananRenk} {CC.G}kazandın!{CC.W} Ruletten {CC.B}{win}{CC.W} kredi kazandın!");
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
                            x.PrintToChat($"{Prefix} Üzgünüm, {CcOfRulet(kazananRenk)}{kazananRenk} kazandı! {enteredCredit.Credit} kredi kaybettin!");
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
        RuletOptions.Siyah => CC.DB,
        _ => CC.B
    };

    #endregion Rulet
}