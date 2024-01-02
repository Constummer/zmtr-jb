using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static bool FFMenuCheck = false;

    private static CounterStrikeSharp.API.Modules.Timers.Timer FFTimer { get; set; } = null;

    #region Ff

    [ConsoleCommand("ff")]
    [CommandHelper(1, "<0/1>")]
    public void Ff(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount < 2) return;

        var oneZeroStr = info.ArgCount > 1 ? info.GetArg(1) : null;
        int.TryParse(oneZeroStr, out var oneZero);
        if (oneZero < 0 || oneZero > 1)
        {
            player.PrintToChat($" {CC.LR}[ZMTR]{CC.W} 0 = kapatmak icin, 1 = acmak icin.");
            return;
        }
        switch (oneZero)
        {
            case 0:
                Ff(false);
                break;

            case 1:
                Ff(true);
                break;
        }
    }

    [ConsoleCommand("ffiptal")]
    public void Ffiptal(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        BasicCountdown.StopCountdown();

        FFTimer?.Kill();
        Ff(false);
    }

    [ConsoleCommand("ffac", "ff acar")]
    public void FfAc(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        Ff(true);
    }

    [ConsoleCommand("ffkapa", "ff kapatir")]
    [ConsoleCommand("ffkapat", "ff kapatir")]
    public void FfKapa(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        Ff(false);
    }

    [ConsoleCommand("ffsure")]
    [CommandHelper(1, "<saniye>")]
    public void FfSure(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);

        if (int.TryParse(target, out int value))
        {
            if (value > 120)
            {
                player.PrintToChat("Max 120 sn girebilirsin");
            }
            else
            {
                BasicCountdown.CommandStartTextCountDown(this, $"FF'in açılmasına {value} saniye kaldı!");

                FFTimer?.Kill();
                FFTimer = AddTimer(value, () =>
                {
                    Ff(true);
                });
            }
        }
    }

    [ConsoleCommand("ffdondur")]
    [CommandHelper(2, "<saniye> <mesaj>")]
    public void FfDondur(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye25"))
        {
            player.PrintToChat($" {CC.LR}[ZMTR]{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (info.ArgCount < 3) return;
        var target = info.GetArg(1);
        int index = info.ArgString.IndexOf($"{target} ");

        var msg = string.Empty;
        if (index != -1)
        {
            msg = info.ArgString.Remove(index, target.Length + 1);
        }
        else
        {
            msg = info.ArgString;
        }

        if (int.TryParse(target, out int value))
        {
            if (value > 120)
            {
                player.PrintToChat("Max 120 sn girebilirsin");
            }
            else
            {
                BasicCountdown.CommandStartTextCountDown(this, $"[{msg}] {Environment.NewLine}Donmak için {value} saniye!");
                FFTimer?.Kill();
                FFTimer = AddTimer(value, () =>
                {
                    GetPlayers()
                    .Where(x => x != null
                         && x.PlayerPawn.IsValid
                         && x.PawnIsAlive
                         && x.IsValid
                         && x?.PlayerPawn?.Value != null
                         && GetTeam(x) == CsTeam.Terrorist)
                    .ToList()
                    .ForEach(x =>
                    {
                        SetColour(x, Config.Burry.BuryColor);

                        x.PlayerPawn.Value!.MoveType = MoveType_t.MOVETYPE_OBSOLETE;
                        Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
                        Vector currentSpeed = new Vector(0, 0, 0);
                        QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
                        x.PlayerPawn.Value.Teleport(currentPosition, currentRotation, currentSpeed);
                    });
                    Ff(false);
                });
            }
        }
    }

    [ConsoleCommand("ffmenu")]
    [CommandHelper(1, "<saniye>")]
    public void FfMenu(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye22"))
        {
            player.PrintToChat($" {CC.LR}[ZMTR]{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);

        if (int.TryParse(target, out int value))
        {
            if (value > 120)
            {
                player.PrintToChat("Max 120 sn girebilirsin");
            }
            else
            {
                if (value >= 20)
                {
                    RespawnAcActive = true;
                }

                BasicCountdown.CommandStartTextCountDown(this, $"FF açılmasına {value} saniye kaldı");
                GetPlayers(CsTeam.Terrorist)
                 .Where(x => x.PawnIsAlive && ValidateCallerPlayer(x, false))
                 .ToList()
                 .ForEach(x =>
                 {
                     SetHp(x, 100);
                     x.GiveNamedItem("weapon_ak47");
                     x.GiveNamedItem("weapon_deagle");
                     x.GiveNamedItem("weapon_hegrenade");

                     var gunMenu = new ChatMenu("Silah Menu");
                     MenuHelper.GetGuns(gunMenu);
                     ChatMenus.OpenMenu(x, gunMenu);
                     x.PrintToChat($" {CC.LR}[ZMTR] {CC.W}FF başlayana kadar veya FF boyunca silah değiştirebilirsin, !guns");
                 });
                FFMenuCheck = true;

                FFTimer?.Kill();
                FFTimer = AddTimer(value, () =>
                {
                    RespawnAcActive = false;
                    Ff(true);
                });
            }
        }
    }

    [ConsoleCommand("guns")]
    public void FfSilahMenu(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        var mp_teammates_are_enemies = ConVar.Find("mp_teammates_are_enemies")?.GetPrimitiveValue<bool>();

        if (FFMenuCheck == true)
        {
            var gunMenu = new ChatMenu("Silah Menu");
            MenuHelper.GetGuns(gunMenu);
            ChatMenus.OpenMenu(player!, gunMenu);
        }
        else
        {
            player!.PrintToChat($" {CC.LR}[ZMTR] {CC.W}FF açık olmadığı için silah menüsüne erişemezsin.");
            return;
        }
    }

    private static void Ff(bool ac)
    {
        if (ac)
        {
            Server.ExecuteCommand("mp_teammates_are_enemies 1");
            Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.DR}FF {CC.W}açıldı.");
        }
        else
        {
            FFMenuCheck = false;
            Server.ExecuteCommand("mp_teammates_are_enemies 0");
            Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.DR}FF {CC.W}kapandı.");
        }
    }

    #endregion Ff
}