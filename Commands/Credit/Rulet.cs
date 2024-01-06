﻿using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Dictionary<ulong, int> RuletPlayers = new();

    #region Rulet

    [ConsoleCommand("rulet")]
    [CommandHelper(1, "<kredi> <yeşil/siyah/kırmızı>")]
    public void Rulet(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (info.ArgCount != 3)
        {
            player.PrintToChat($"{Prefix} {CC.W}Ruletini hangi renge vereceğini yazman lazım.");
            player.PrintToChat($"{Prefix} {CC.R}Kirmizi{CC.W}/{CC.R}K");
            player.PrintToChat($"{Prefix} {CC.G}Yeşil{CC.W}/{CC.G}Y");
            player.PrintToChat($"{Prefix} {CC.DB}Siyah{CC.W}/{CC.DB}S");
            return;
        }

        var target = info.GetArg(1);

        if (!int.TryParse(target, out int value))
        {
            player.PrintToChat($"{Prefix} {CC.R}GEÇERSİZ MİKTAR");
            return;
        }
        else
        {
            if (value < 100 || value > 5000)
            {
                player.PrintToChat($"{Prefix} {CC.R}Min 100, Max 5000 kredi girebilirsin");
                return;
            }
            else
            {
                if (RuletPlayers.TryGetValue(player.SteamID, out var enteredCredit))
                {
                    RuletPlayers[player.SteamID] = value;
                }
                else
                {
                    RuletPlayers.Add(player.SteamID, value);
                }
            }
        }
    }

    private static void RuletActivate()
    {
        string kazananRenk = RuletDondur();

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
                x.PrintToChat($"{Prefix} Kazanan renk: {kazananRenk}");

                if (kazananRenk == "yeşil")
                {
                    x.PrintToChat($"{Prefix} Tebrikler {kazananRenk} kazandın! Ruletten {enteredCredit * 14} kredi kazandın!");
                }
                else
                {
                    bool kazandiMi = RuletSonucunuKontrolEt(kazananRenk);
                    int kazancKayipMiktari = kazandiMi ? enteredCredit * 2 : -enteredCredit;

                    if (kazandiMi)
                    {
                        var win = enteredCredit * 2;
                        x.PrintToChat($"{Prefix} Tebrikler {kazananRenk} kazandın! Ruletten {win} kredi kazandın!");
                    }
                    else
                    {
                        x.PrintToChat($"{Prefix} Üzgünüm, {kazananRenk} kazandı! {enteredCredit} kredi kaybettin!");
                    }
                }
            });
        RuletPlayers.Clear();

        static bool RuletSonucunuKontrolEt(string kazananRenk)
        {
            // Kazanan renge göre kazanıp kaybedildiği kontrol edilir
            if (kazananRenk == "siyah" || kazananRenk == "kırmızı")
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
        static string RuletDondur()
        {
            int sayi = _random.Next(1, 101);

            if (sayi <= 2)
            {
                return "yeşil";
            }
            else if (sayi <= 51)
            {
                return "siyah";
            }
            else
            {
                return "kırmızı";
            }
        }
    }

    #endregion Rulet
}