using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Delay

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
            player.PrintToChat($"{Prefix} {CC.B}Sadece {CC.W} Komutçu bu komutu kullanabilir");
            return;
        }
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
            player.PrintToChat($"{Prefix} {CC.B}Sadece {CC.W} Komutçu bu komutu kullanabilir");
            return;
        }
        var target = info.ArgCount > 1 ? info.GetArg(1) : "20";
        if (int.TryParse(target, out var value))
        {
            if (value > 120)
            {
                player.PrintToChat("Max 120 sn girebilirsin");
                return;
            }
            else if (value < 1)
            {
                player.PrintToChat("Min 1 sn girebilirsin");
                return;
            }
            else
            {
                IsEliStart(value); return;
            }
        }
        else
        {
            IsEliStart(value); return;
        }
    }

    private void IsEliStart(int value)
    {
        RespawnAcAction();
        ForceEntInput("func_breakable", "Break", "DropEldenGidiyeah");
        BasicCountdown.CommandStartTextCountDown(this, $"İseli - İsyan Eli | Başlamasına {value} saniye kaldı!");
        ForceCloseDoor();

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
             x.GiveNamedItem("weapon_ak47");
             x.GiveNamedItem("weapon_deagle");
             x.GiveNamedItem("weapon_hegrenade");
             x.GiveNamedItem("item_assaultsuit");

             var gunMenu = new ChatMenu("Silah Menu");
             MenuHelper.GetGuns(gunMenu);
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
        });
    }

    private static void IsEliWardenNotify()
    {
        var warden = GetWarden();
        if (warden != null)
        {
            warden.PrintToChat($"{Prefix} {CC.W} eğer {CC.R}İSELİ {CC.W} ise");
            warden.PrintToChat($"{Prefix} {CC.B} !rm {CC.W}veya {CC.B}!revmenu {CC.W} ile menüden seçerek veya");
            warden.PrintToChat($"{Prefix} {CC.B} !rmf {CC.W} HIZLICA ");
            warden.PrintToChat($"{Prefix} {CC.W} Ölen ctleri 3 kere revleyebilirsin");
            SharpTimerPrintHtml(warden, $"{CC.W} eğer {CC.R}İSELİ {CC.W} ise {CC.B} !rm {CC.W}veya {CC.B}!revmenu {CC.W} ile menüden seçerek veya {CC.B} !rmf {CC.W} HIZLICA {CC.W} Ölen ctleri 3 kere revleyebilirsin");
        }
    }

    private void IsEliTerroristTp(CCSPlayerController? player)
    {
        if (ValidateCallerPlayer(player, printMsg: false) == false
           || GetTeam(player) != CsTeam.CounterTerrorist)
        {
            return;
        }

        GetPlayers()
        .Where(x => x.PawnIsAlive
                           && GetTargetAction(x, "@t", null))
               .ToList()
               .ForEach(x =>
               {
                   RemoveWeapons(x, true);

                   x.PlayerPawn.Value.Teleport(Hucre, ANGLE_ZERO, VEC_ZERO);
               });
    }

    #endregion Delay
}