﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Komal

    private static bool KomActive = false;
    private static List<ulong> KomAdays = new List<ulong>();
    public static Dictionary<ulong, int> KomAlAnswers = new Dictionary<ulong, int>();
    public static bool KomAlVoteInProgress = false;

    [ConsoleCommand("komal")]
    public void KomAl(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye10"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        KomActive = false;
        KomAdays?.Clear();
        KomAlAnswers?.Clear();
        Server.PrintToChatAll($"{Prefix} {CC.G}Komutçu alımı başladı! Komutçu adayı olmak için !komaday yazın.");
        Server.PrintToChatAll($"{Prefix} {CC.G}Komutçu alımı başladı! Komutçu adayı olmak için !komaday yazın.");
        Server.PrintToChatAll($"{Prefix} {CC.G}Komutçu adaylığından ayrılmak için !komadayiptal yazın.");
        KomActive = true;
        var now = DateTime.UtcNow;
        AddTimer(30f, () =>
        {
            if (KomActive)
            {
                KomActive = false;
                KomAlStartVote();
            }
        });
    }

    [ConsoleCommand("komaday")]
    public void KomAday(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (KomActive)
        {
            if (KomAdays.Count > 6)
            {
                KomActive = false;
                player.PrintToChat($"{Prefix} {CC.W}Komaday doldu.");
            }
            else
            {
                if (KomAdays.Any(x => x == player.SteamID))
                {
                    player.PrintToChat($"{Prefix} {CC.W}Zaten katıldın!");
                }
                else
                {
                    KomAdays.Add(player.SteamID);
                    player.VoiceFlags &= ~VoiceFlags.Muted;
                    Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName} {CC.W}komutçu adayı oldu.");
                }
            }
        }
        else
        {
            player.PrintToChat($"{Prefix} {CC.DR}Komutçu alımı aktif değil.");
        }
    }

    [ConsoleCommand("komadayiptal")]
    public void KomAdayIptal(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye10"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        player.VoiceFlags |= VoiceFlags.Muted;
        KomAdays = KomAdays.Where(x => x != player.SteamID).ToList();
        Server.PrintToChatAll($"{Prefix} {CC.DR}{player.PlayerName} {CC.W}Komutçu adaylığından ayrıldı.");
    }

    private void KomAlStartVote()
    {
        if (KomAdays != null & KomAdays.Any() == true)
        {
            if (KomAdays.Count == 1)
            {
                var kvp = KomAdays[0];

                var winner = Utilities.GetPlayerFromSteamId(kvp);

                if (ValidateCallerPlayer(winner, false))
                {
                    AddTimer(1, () =>
                    {
                        winner.SwitchTeam(CsTeam.CounterTerrorist);
                    });
                }
            }
            else
            {
                var voters = GetPlayers().Where(x => KomAdays.Contains(x.SteamID));

                Server.PrintToChatAll($"{Prefix} {CC.G} komaday sure bitti, oylama basladi.");
                var komalVoteMenu = new ChatMenu("KomAl aday oylamasi");
                KomAlVoteInProgress = true;
                foreach (var voter in voters)
                {
                    komalVoteMenu.AddMenuOption(voter.PlayerName, (x, i) =>
                    {
                        if (KomAlVoteInProgress)
                        {
                            if (KomAlAnswers.TryGetValue(voter.SteamID, out var answer))
                            {
                                KomAlAnswers[voter.SteamID] = answer + 1;
                            }
                            else
                            {
                                KomAlAnswers.Add(voter.SteamID, 1);
                            }
                        }
                    });
                }
                var players = GetPlayers();
                foreach (var x in players)
                {
                    ChatMenus.OpenMenu(x, komalVoteMenu);
                }

                AddTimer(15, () =>
                {
                    if (KomAlVoteInProgress)
                    {
                        Server.PrintToChatAll($" {CC.R} Komaday sonuclari");

                        var list = KomAlAnswers.OrderByDescending(x => x.Value).ToList();

                        for (int i = 0; i < list.Count; i++)
                        {
                            var kvp = list[i];
                            if (i == 0)
                            {
                                var winner = Utilities.GetPlayerFromSteamId(kvp.Key);
                                if (ValidateCallerPlayer(winner, false))
                                {
                                    AddTimer(1, () =>
                                    {
                                        winner.SwitchTeam(CsTeam.CounterTerrorist);
                                    });
                                }
                            }

                            var voter = voters.FirstOrDefault(x => x.SteamID == kvp.Key);
                            if (voter != null)
                            {
                                Server.PrintToChatAll($"{voter.PlayerName} - {kvp.Value}");
                            }
                        }
                        KomAlAnswers.Clear();
                        KomAlVoteInProgress = false;
                    }
                }, TimerFlags.STOP_ON_MAPCHANGE);
            }
        }
        else
        {
            Server.PrintToChatAll($"{Prefix} {CC.G} oylama yapilamiyor, yeterli aday yok .");
        }
    }

    #endregion Komal
}