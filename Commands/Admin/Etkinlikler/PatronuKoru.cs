using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static bool PatronuKoruActive { get; set; } = false;
    private ulong? PatronuKoruCTLider { get; set; } = null;
    private ulong? PatronuKoruTLider { get; set; } = null;
    private ulong? PatronuKoruTKoruma1 { get; set; } = null;
    private ulong? PatronuKoruTKoruma2 { get; set; } = null;
    private ulong? PatronuKoruCTKoruma1 { get; set; } = null;
    private ulong? PatronuKoruCTKoruma2 { get; set; } = null;

    [ConsoleCommand("PatronuKoru")]
    [ConsoleCommand("PatronuKoruBasla")]
    public void PatronuKoru(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        PatronuKoruActive = !PatronuKoruActive;
        switch (PatronuKoruActive)
        {
            case true:
                Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.Go} PATRONU KORU{CC.W} etkinliğini başlattı");
                break;

            case false:
                Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.Go} PATRONU KORU{CC.W} etkinliğini bitirdi");
                break;
        }
    }

    [ConsoleCommand("PatronuKoruMapAc")]
    public void PatronuKoruMapAc(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        Server.ExecuteCommand($"host_workshop_map 3178317288");
    }

    [ConsoleCommand("PatronuKoruCTLider")]
    public void PatronuKoruCTLiderSec(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        SetPatronuKoruLider(CsTeam.CounterTerrorist, player);
        LogManagerCommand(player.SteamID, info.GetCommandString);
    }

    [ConsoleCommand("PatronuKoruTLider")]
    public void PatronuKoruTLiderSec(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        SetPatronuKoruLider(CsTeam.Terrorist, player);
        LogManagerCommand(player.SteamID, info.GetCommandString);
    }

    private static void PatronuKoruRoundStart()
    {
        HookDisabled = true;
        Server.ExecuteCommand($"sv_enablebunnyhopping 0;sv_autobunnyhopping 0");
        Server.ExecuteCommand("mp_autoteambalance 1");
        Server.ExecuteCommand("mp_forcecamera 1");

        Server.ExecuteCommand("mp_free_armor 1");
        Server.ExecuteCommand("sv_alltalk 1");
        Server.ExecuteCommand("sv_deadtalk 1");
        Server.ExecuteCommand("sv_voiceenable 1");
        Model0Action();
        Global?.AddTimer(3f, () =>
        {
            Model0Action();
        });
    }

    private void SetPatronuKoruLider(CsTeam team, CCSPlayerController? player)
    {
        if (PatronuKoruActive == false)
        {
            player.PrintToChat($"{Prefix} {CC.W}Patronu koru aktif değil. {CC.B}!PatronuKoru{CC.W} yazarak aktif edebilirsin");
            return;
        }
        var teamStr = "";
        switch (team)
        {
            case CsTeam.Terrorist:
                if (PatronuKoruTLider.HasValue)
                {
                    var p = GetPlayers().Where(x => x.SteamID == PatronuKoruTLider).FirstOrDefault();
                    if (p != null)
                    {
                        if (ValidateCallerPlayer(p, false))
                        {
                            p.Clan = null;
                            AddTimer(0.2f, () =>
                            {
                                Utilities.SetStateChanged(p, "CCSPlayerController", "m_szClan");
                                Utilities.SetStateChanged(p, "CBasePlayerController", "m_iszPlayerName");
                            });
                        }
                    }
                }
                teamStr = "T";
                break;

            case CsTeam.CounterTerrorist:
                teamStr = "CT";
                if (PatronuKoruCTLider.HasValue)
                {
                    var p = GetPlayers().Where(x => x.SteamID == PatronuKoruCTLider).FirstOrDefault();
                    if (p != null)
                    {
                        if (ValidateCallerPlayer(p, false))
                        {
                            p.Clan = null;
                            AddTimer(0.2f, () =>
                            {
                                Utilities.SetStateChanged(p, "CCSPlayerController", "m_szClan");
                                Utilities.SetStateChanged(p, "CBasePlayerController", "m_iszPlayerName");
                            });
                        }
                    }
                }
                break;
        }

        var players = GetPlayers(team)
                      .ToList();

        if (players.Count == 0)
        {
            player.PrintToChat($"{Prefix} {CC.W}hiç {teamStr} bulunamadı");
            return;
        }
        var kaMenu = new ChatMenu($"Patronu Koru - {teamStr} LIDER Menü");
        kaMenu.AddMenuOption($"Seçeceğin {teamStr}, Patronu koru etkinliginde {teamStr} LIDER Olacak!", null, true);
        players.ForEach(x =>
        {
            kaMenu.AddMenuOption(x.PlayerName, (p, t) =>
            {
                if (ValidateCallerPlayer(x, false) == false) return;
                switch (team)
                {
                    case CsTeam.Terrorist:
                        PatronuKoruTLider = x.SteamID;
                        break;

                    case CsTeam.CounterTerrorist:
                        PatronuKoruCTLider = x.SteamID;
                        break;
                }
                x.Clan = $"{CC.Go}[{teamStr} - LIDER]";
                AddTimer(0.2f, () =>
                {
                    if (ValidateCallerPlayer(x, false) == false) return;
                    Utilities.SetStateChanged(x, "CCSPlayerController", "m_szClan");
                    Utilities.SetStateChanged(x, "CBasePlayerController", "m_iszPlayerName");
                }, SOM);
                switch (team)
                {
                    case CsTeam.Terrorist:
                        Server.PrintToChatAll($"{Prefix} {CC.Go}{x.PlayerName} {CC.P} [{teamStr} - LIDER]{CC.W} Olarak seçildi");
                        break;

                    case CsTeam.CounterTerrorist:
                        Server.PrintToChatAll($"{Prefix} {CC.Go}{x.PlayerName} {CC.P} [{teamStr} - LIDER]{CC.W} Olarak seçildi");
                        break;
                }
            });
        });
        MenuManager.OpenChatMenu(player, kaMenu);
    }

    private void ChooseRandomTwoGuardian()
    {
        try
        {
            var ctPlayers = GetPlayers(CsTeam.CounterTerrorist).Where(x => x.SteamID != PatronuKoruCTLider).Select(x => new { x.PlayerName, x.SteamID });
            if (ctPlayers.Count() > 0)
            {
                var randomX = ctPlayers.Skip(_random.Next(ctPlayers.Count())).FirstOrDefault();
                if (randomX != null)
                {
                    PatronuKoruCTKoruma1 = randomX.SteamID;
                    Server.PrintToChatAll($"{Prefix} {CC.Go}PATRONU KORU ETKINLIGI {CC.P} [CT - KORUMA 1]{CC.B} {randomX.PlayerName}{CC.W} Olarak seçildi");
                    ctPlayers = ctPlayers.Where(x => x.SteamID != PatronuKoruCTKoruma1);
                    randomX = ctPlayers.Skip(_random.Next(ctPlayers.Count())).FirstOrDefault();
                    if (randomX != null)
                    {
                        PatronuKoruCTKoruma2 = randomX.SteamID;
                        Server.PrintToChatAll($"{Prefix} {CC.Go}PATRONU KORU ETKINLIGI {CC.P} [CT - KORUMA 2]{CC.B} {randomX.PlayerName}{CC.W} Olarak seçildi");
                    }
                    else
                    {
                        PatronuKoruCTKoruma2 = null;
                    }
                }
                else
                {
                    PatronuKoruCTKoruma1 = null;
                    PatronuKoruCTKoruma2 = null;
                }
            }
            else
            {
                PatronuKoruCTKoruma1 = null;
                PatronuKoruCTKoruma2 = null;
            }
        }
        catch
        {
            PatronuKoruCTKoruma1 = null;
            PatronuKoruCTKoruma2 = null;
        }
        try
        {
            var tPlayers = GetPlayers(CsTeam.Terrorist).Where(x => x.SteamID != PatronuKoruTLider).Select(x => new { x.PlayerName, x.SteamID });
            if (tPlayers.Count() > 0)
            {
                var randomX = tPlayers.Skip(_random.Next(tPlayers.Count())).FirstOrDefault();
                if (randomX != null)
                {
                    PatronuKoruTKoruma1 = randomX.SteamID;
                    Server.PrintToChatAll($"{Prefix} {CC.Go}PATRONU KORU ETKINLIGI {CC.P} [T - KORUMA 1]{CC.Or} {randomX.PlayerName}{CC.W} Olarak seçildi");
                    tPlayers = tPlayers.Where(x => x.SteamID != PatronuKoruTKoruma1);
                    randomX = tPlayers.Skip(_random.Next(tPlayers.Count())).FirstOrDefault();
                    if (randomX != null)
                    {
                        PatronuKoruTKoruma2 = randomX.SteamID;
                        Server.PrintToChatAll($"{Prefix} {CC.Go}PATRONU KORU ETKINLIGI {CC.P} [T - KORUMA 2]{CC.Or} {randomX.PlayerName}{CC.W} Olarak seçildi");
                    }
                    else
                    {
                        PatronuKoruTKoruma2 = null;
                    }
                }
                else
                {
                    PatronuKoruTKoruma1 = null;
                    PatronuKoruTKoruma2 = null;
                }
            }
            else
            {
                PatronuKoruTKoruma1 = null;
                PatronuKoruTKoruma2 = null;
            }
        }
        catch
        {
            PatronuKoruCTKoruma1 = null;
            PatronuKoruCTKoruma2 = null;
        }
    }

    private bool PatronuKoruSay(CCSPlayerController player, CommandInfo info, bool isSayTeam)
    {
        if (player.SteamID != PatronuKoruCTLider
            && player.SteamID != PatronuKoruTLider
            && player.SteamID != PatronuKoruCTKoruma1
            && player.SteamID != PatronuKoruCTKoruma2
            && player.SteamID != PatronuKoruTKoruma1
            && player.SteamID != PatronuKoruTKoruma2)
        {
            return false;
        }
        var team = GetTeam(player);
        var c = CC.W;
        var n = CC.W;
        var tagname = "";
        if (player.SteamID == PatronuKoruCTLider || player.SteamID == PatronuKoruTLider)
        {
            n = CC.Go;
            tagname = "Lider";
        }
        else
        {
            n = team switch
            {
                CsTeam.CounterTerrorist => CC.B,
                CsTeam.Terrorist => CC.Or,
            };
            tagname = "Koruma";
        }

        var teamStr = team switch
        {
            CsTeam.CounterTerrorist => $"{CC.B}[{CT_AllCap}]",
            CsTeam.Terrorist => $"{CC.R}[{T_AllCap}]",
            CsTeam.Spectator => $"{CC.P}[SPEC]",
            _ => ""
        };
        var deadStr = player.PawnIsAlive == false ? $"{CC.R}*ÖLÜ*" : "";
        var str = $" {deadStr}"
                + $" {(isSayTeam ? $"{teamStr}" : "")}"
                + $" {c}[{tagname}]"
                + $" {n}{player.PlayerName}"
                + $" {CC.W}:"
                + $" {CC.Gr}{info.GetArg(1)}";
        if (isSayTeam)
        {
            GetPlayers(team).ToList()
                .ForEach(x => x.PrintToChat(str));
        }
        else
        {
            Server.PrintToChatAll(str);
        }
        return true;
    }
}