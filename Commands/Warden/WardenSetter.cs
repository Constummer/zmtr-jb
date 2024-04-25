using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static ulong? LatestWCommandUser { get; set; } = 0;

    #region OnWCommand

    [ConsoleCommand("w")]
    public void OnWCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        if (LatestWCommandUser != null)
        {
            var warden = GetWarden();
            if (warden != null)
            {
                player.PrintToChat($"{Prefix} {CC.W}Şu an {CC.G}{warden.PlayerName}{CC.W} isimli oyuncu komutçu. Komutçu olamazsın!");
                return;
            }
        }

        if (GetTeam(player) != CsTeam.CounterTerrorist)
        {
            player.PrintToChat($"{Prefix} {CC.W}Komutçu olabilmek için CT takımında olman gerekmektedir!");
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);

        player.VoiceFlags &= ~VoiceFlags.Muted;
        SetColour(player, Color.FromArgb(255, 0, 0, 255));
        RefreshPawn(player);

        KomStartTime = DateTime.UtcNow;
        LatestWCommandUser = player.SteamID;
        CoinGoWanted = true;
        WardenRefreshPawn();
        ClearLasers();
        CoinAfterNewCommander();
        WardenEnterSound();
        _ClientQueue.Enqueue(new(player.SteamID, null, "", QueueItemType.OnWChange));

        Server.PrintToChatAll($"{Prefix} {CC.B}{player.PlayerName}{CC.W} Komutçu oldu!");
        player.PrintToChat($"{Prefix} {CC.B}!wkomutlar {CC.W}veya {CC.B}!wcommands {CC.W}yazarak komutçu komutlarını öğrenebilirsin!");

        GetPlayers()
            .ToList()
            .ForEach((x) =>
            {
                x.PrintToCenter($"{Prefix} {CC.B}{player.PlayerName}{CC.B} Komutçu oldu!");
            });
    }

    [ConsoleCommand("wkomutlar")]
    [ConsoleCommand("wcommands")]
    public void OnWkomutlarCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        var warden = GetWarden();
        if (warden == null)
        {
            player.PrintToChat($"{Prefix} {CC.W}halihazirda komutçu bulunmuyor!");
            return;
        }

        if (warden.SteamID != player.SteamID || LatestWCommandUser != player.SteamID)
        {
            player.PrintToChat($"{Prefix} {CC.W}Şu an {CC.G}{warden.PlayerName}{CC.W} isimli oyuncu komutçu. Bu komutu sadece komutçu kullanabilir!");
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);

        player.PrintToChat($"{Prefix} {CC.W}!w - komutçu ol");
        player.PrintToChat($"{Prefix} {CC.W}!uw - komutçuluktan ayrıl.");
        player.PrintToChat($"{Prefix} {CC.W}!rw - yetki kullanarak zorla komutçuyu kov.");
        player.PrintToChat($"{Prefix} {CC.W}!iseli - isyan eli başlat. ");
    }

    [ConsoleCommand("uw")]
    [ConsoleCommand("lw")]
    public void OnUWCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        var warden = GetWarden();
        if (warden == null)
        {
            player.PrintToChat($"{Prefix} {CC.W}halihazirda komutçu bulunmuyor!");
            return;
        }

        if (warden.SteamID != player.SteamID || LatestWCommandUser != player.SteamID)
        {
            player.PrintToChat($"{Prefix} {CC.W}Şu an {CC.G}{warden.PlayerName}{CC.W} isimli oyuncu komutçu. Bu komutu sadece komutçu kullanabilir!");
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        KomStartTime = null;
        LatestWCommandUser = null;
        ClearLasers();
        CoinRemove();
        WardenLeaveSound();
        CleanTagOnKomutcuAdmin();
        _ClientQueue.Enqueue(new(player.SteamID, null, "", QueueItemType.OnWChange));

        player.VoiceFlags |= VoiceFlags.Muted;
        SetColour(player, DefaultColor);
        RefreshPawn(player);

        Server.PrintToChatAll($"{Prefix} {CC.B}{player.PlayerName}{CC.W} artık komutçu değil!");
        Server.PrintToChatAll($"{Prefix} {CC.B}{player.PlayerName}{CC.W} artık komutçu değil!");
    }

    [ConsoleCommand("rw")]
    public void OnRWCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        var warden = GetWarden();
        if (warden == null)
        {
            player.PrintToChat($"{Prefix} {CC.W}halihazirda komutçu bulunmuyor!");
            LatestWCommandUser = null;
            ClearLasers();
            CoinRemove();
            WardenLeaveSound();
        }
        else
        {
            RemoveWardenAction(warden);
        }
    }

    private void RemoveWardenAction(CCSPlayerController? warden)
    {
        LatestWCommandUser = null;
        ClearLasers();
        CoinRemove();
        WardenLeaveSound();
        CleanTagOnKomutcuAdmin();
        warden.VoiceFlags |= VoiceFlags.Muted;
        SetColour(warden, DefaultColor);
        RefreshPawn(warden);
        KomStartTime = null;
        _ClientQueue.Enqueue(new(warden.SteamID, null, "", QueueItemType.OnWChange));

        Server.PrintToChatAll($"{Prefix} {CC.W}{warden.PlayerName} artık komutçu değil!");

        GetPlayers()
            .ToList()
            .ForEach((x) =>
            {
                x.PrintToCenter($"{Prefix} {CC.W}{warden.PlayerName} artık komutçu değil!");
            });
    }

    public static void WardenSay(CCSPlayerController? player, CommandInfo info, bool isSayTeam)
    {
        var teamStr = $"{CC.B}[{CT_AllCap}]";

        var deadStr = player.PawnIsAlive == false ? $"{CC.R}*ÖLÜ*" : "";

        var str = $" {deadStr}"
                + $" {(isSayTeam ? $"{teamStr}" : "")}"
                + $" {CC.G}[K]"
                + $" {CC.B}{player.PlayerName}"
                + $" {CC.W}:"
                + $" {CC.W}{info.GetArg(1)}";
        if (isSayTeam)
        {
            GetPlayers(CsTeam.CounterTerrorist).ToList()
                .ForEach(x => x.PrintToChat(str));
        }
        else
        {
            Server.PrintToChatAll(str);
        }
    }

    private bool WLevelPlayer(CCSPlayerController player, CommandInfo info, bool isSayTeam)
    {
        if (PlayerTimeTracking.TryGetValue(player.SteamID, out var item) == false)
        {
            return false;
        }
        if (!HasPerm(player.SteamID, "@css/komutcu"))
        {
            return false;
        }
        var team = GetTeam(player);
        var teamStr = team switch
        {
            CsTeam.CounterTerrorist => $"{CC.B}[{CT_AllCap}]",
            CsTeam.Terrorist => $"{CC.R}[{T_AllCap}]",
            CsTeam.Spectator => $"{CC.P}[SPEC]",
            _ => ""
        };
        var c = team switch
        {
            CsTeam.CounterTerrorist => CC.B,
            CsTeam.Terrorist => CC.Or,
            CsTeam.Spectator => CC.W,
            _ => CC.W
        };
        var tagname = "";
        if (item.WTime < 20 * 60)
        {
            tagname = "Junior Komutçu";
        }
        else if (item.WTime < 30 * 60)
        {
            tagname = "Mid Komutçu";
        }
        else
        {
            tagname = "Senior Komutçu";
        }
        var deadStr = player.PawnIsAlive == false ? $"{CC.R}*ÖLÜ*" : "";
        var str = $" {deadStr}"
                + $" {(isSayTeam ? $"{teamStr}" : "")}"
                + $" {CC.G}[{tagname}]"
                + $" {c}{player.PlayerName}"
                + $" {CC.W}:"
                + $" {info.GetArg(1)}";
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

    #endregion OnWCommand
}