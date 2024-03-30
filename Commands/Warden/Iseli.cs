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
    private static bool IsEliMenuCheck { get; set; } = false;
    private CounterStrikeSharp.API.Modules.Timers.Timer IseliTimer { get; set; } = null;

    [ConsoleCommand("iselikapa")]
    [ConsoleCommand("iseliiptal")]
    [ConsoleCommand("iselicancel")]
    [ConsoleCommand("canceliseli")]
    public void IseliIptal(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        if (LatestWCommandUser != player.SteamID)
        {
            if (!AdminManager.PlayerHasPermissions(player, "@css/lider"))
            {
                player.PrintToChat($"{Prefix} {CC.B}Sadece {CC.W} Komutçu bu komutu kullanabilir");
                return;
            }
        }

        if (LatestWCommandUser != player.SteamID)
        {
            player.PrintToChat($"{Prefix} {CC.B}Sadece {CC.W} Komutçu bu komutu kullanabilir");
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        IseliTimer?.Kill();
        RespawnKapatAction();
        IsEliMenuCheck = false;
    }

    [ConsoleCommand("iseli")]
    [CommandHelper(1, "<saniye>")]
    public void Iseli(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        if (LatestWCommandUser != player.SteamID)
        {
            if (!AdminManager.PlayerHasPermissions(player, "@css/lider"))
            {
                player.PrintToChat($"{Prefix} {CC.B}Sadece {CC.W} Komutçu bu komutu kullanabilir");
                return;
            }
        }
        var target = info.ArgCount > 1 ? info.ArgString.GetArg(0) : "20";
        if (int.TryParse(target, out var value))
        {
            if (value > 30)
            {
                player.PrintToChat("Max 30 sn girebilirsin");
                return;
            }
            else if (value < 5)
            {
                player.PrintToChat("Min 5 sn girebilirsin");
                return;
            }
            else
            {
                LogManagerCommand(player.SteamID, info.GetCommandString);
                IsEliStart(value); return;
            }
        }
        else
        {
            LogManagerCommand(player.SteamID, info.GetCommandString);
            IsEliStart(value); return;
        }
    }

    private void IsEliStart(int value)
    {
        Server.PrintToChatAll($"{Prefix}{CC.W} Hook el boyunca kapalı.");
        HookDisabled = true;
        ForceCloseDoor();
        RespawnAcAction();
        ForceEntInput("func_breakable", "Break");
        IsEliWardenNotify();
        BasicCountdown.CommandStartTextCountDown(this, $"İseli - İsyan Eli | Başlamasına {value} saniye kaldı!");
        IsEliTerroristTp();
        GetPlayers(CsTeam.CounterTerrorist)
         .Where(x => ValidateCallerPlayer(x, false))
         .ToList()
         .ForEach(x =>
         {
             if (value >= 20)
             {
                 if (x.PawnIsAlive == false)
                 {
                     CustomRespawn(x);
                 }
             }
             SetHp(x, 100);
             x.GiveNamedItem("item_assaultsuit");

             var gunMenu = new ChatMenu("Silah Menu");
             WeaponMenuHelper.GetGuns(gunMenu, hideIseli: true);
             ChatMenus.OpenMenu(x, gunMenu);
             x.PrintToChat($"{Prefix} {CC.W}Iseli başlayana kadar silah değiştirebilirsin !guns");
         });
        IsEliMenuCheck = true;

        IseliTimer?.Kill();
        IseliTimer = AddTimer(value, () =>
        {
            ForceOpenDoor();
            RespawnKapatAction();
            IsEliMenuCheck = false;
        }, SOM);
    }

    private static void IsEliWardenNotify()
    {
        var warden = GetWarden();
        if (warden != null)
        {
            warden.PrintToChat($"{Prefix} {CC.W} eğer {CC.R}İSELİ {CC.W} ise");
            warden.PrintToChat($"{Prefix} {CC.B} !rm {CC.W}veya {CC.B}!revmenu {CC.W} ile menüden seçerek veya");
            warden.PrintToChat($"{Prefix} {CC.B} !rmf {CC.W}veya");
            warden.PrintToChat($"{Prefix} konsoluna {CC.B} bind p rmf {CC.W} yazarak HIZLICA ");
            warden.PrintToChat($"{Prefix} {CC.W} Ölen ctleri 3 kere revleyebilirsin");
            PrintToCenterHtml(warden, $"{CC.W} eğer {CC.R}İSELİ {CC.W} ise {CC.B} !rm {CC.W}veya {CC.B}!revmenu {CC.W} ile menüden seçerek veya {CC.B} !rmf {CC.W} HIZLICA {CC.W} Ölen ctleri 3 kere revleyebilirsin");
            PrintToCenterHtml(warden, $"{CC.W} eğer {CC.R}İSELİ {CC.W} ise {CC.B} !rm {CC.W}veya {CC.B}!revmenu {CC.W} ile menüden seçerek veya {CC.B} !rmf {CC.W} HIZLICA {CC.W} Ölen ctleri 3 kere revleyebilirsin");
            PrintToCenterHtml(warden, $"{CC.W} eğer {CC.R}İSELİ {CC.W} ise {CC.B} !rm {CC.W}veya {CC.B}!revmenu {CC.W} ile menüden seçerek veya {CC.B} !rmf {CC.W} HIZLICA {CC.W} Ölen ctleri 3 kere revleyebilirsin");
            PrintToCenterHtml(warden, $"{CC.W} eğer {CC.R}İSELİ {CC.W} ise {CC.B} !rm {CC.W}veya {CC.B}!revmenu {CC.W} ile menüden seçerek veya {CC.B} !rmf {CC.W} HIZLICA {CC.W} Ölen ctleri 3 kere revleyebilirsin");
        }
    }

    private void IsEliTerroristTp()
    {
        var coord = Config.Map.MapCellCoords?.Where(x => x?.Text == Server.MapName)?.FirstOrDefault();
        if (coord == null) return;

        GetPlayers(CsTeam.Terrorist)
           .Where(x => x.PawnIsAlive)
                  .ToList()
                  .ForEach(x =>
                  {
                      x.PlayerPawn.Value.Teleport(coord.Coord, ANGLE_ZERO, VEC_ZERO);
                  });
    }
}