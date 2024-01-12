using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static ulong? LatestWCommandUser { get; set; }

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

        player.VoiceFlags &= ~VoiceFlags.Muted;
        SetColour(player, Color.FromArgb(255, 0, 0, 255));
        RefreshPawn(player);

        LatestWCommandUser = player.SteamID;
        CoinGoWanted = true;
        WardenRefreshPawn();
        ClearLasers();
        CoinAfterNewCommander();
        WardenEnterSound();
        Server.PrintToChatAll($"{Prefix} {CC.W}{player.PlayerName} Komutçu oldu!");
        player.PrintToChat($"{Prefix} {CC.B}!wkomutlar {CC.W}veya {CC.B}!wcommands {CC.W}yazarak komutçu komutlarını öğrenebilirsin!");

        GetPlayers()
            .ToList()
            .ForEach((x) =>
            {
                x.PrintToCenter($"{Prefix} {CC.W}{player.PlayerName} Komutçu oldu!");
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
        LatestWCommandUser = null;
        ClearLasers();
        CoinRemove();
        WardenLeaveSound();
        CleanTagOnKomutcuAdmin();

        player.VoiceFlags |= VoiceFlags.Muted;
        SetColour(player, DefaultColor);
        RefreshPawn(player);

        Server.PrintToChatAll($"{Prefix} {CC.W}{player.PlayerName} artık komutçu değil!");

        GetPlayers()
            .ToList()
            .ForEach((x) =>
            {
                x.PrintToCenter($"{Prefix} {CC.W}{player.PlayerName} artık komutçu değil!");
            });
    }

    [ConsoleCommand("rw")]
    public void OnRWCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
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

        Server.PrintToChatAll($"{Prefix} {CC.W}{warden.PlayerName} artık komutçu değil!");

        GetPlayers()
            .ToList()
            .ForEach((x) =>
            {
                x.PrintToCenter($"{Prefix} {CC.W}{warden.PlayerName} artık komutçu değil!");
            });
    }

    #endregion OnWCommand
}