using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static bool FFMenuCheck = false;

    #region Ff

    [ConsoleCommand("ff")]
    public void F(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var mp_teammates_are_enemies = ConVar.Find("mp_teammates_are_enemies")?.GetPrimitiveValue<bool>();

        if (mp_teammates_are_enemies.HasValue)
        {
            Ff(!mp_teammates_are_enemies.Value);
        }
        else
        {
            Ff(true);
        }
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
            BasicCountdown.CommandStartTextCountDown(this, $"FF açılmasına {value} saniye kaldı");

            _ = AddTimer(value, () =>
            {
                Ff(true);
            });
        }
    }

    [ConsoleCommand("ffmenu")]
    [CommandHelper(1, "<saniye>")]
    public void FfMenu(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);

        if (int.TryParse(target, out int value))
        {
            BasicCountdown.CommandStartTextCountDown(this, $"FF açılmasına {value} saniye kaldı");
            GetPlayers(CsTeam.Terrorist)
             .Where(x => x.PawnIsAlive && ValidateCallerPlayer(x, false))
             .ToList()
             .ForEach(x =>
             {
                 x.GiveNamedItem("weapon_ak47");
                 x.GiveNamedItem("weapon_deagle");
                 x.GiveNamedItem("weapon_hegrenade");

                 var gunMenu = new ChatMenu("Silah Menu");
                 MenuHelper.GetGuns(gunMenu);
                 ChatMenus.OpenMenu(x, gunMenu);
                 x.PrintToChat("FF başlayana kadar veya FF boyunca silah değiştirebilirsin, !guns");
             });
            FFMenuCheck = true;

            _ = AddTimer(value, () =>
            {
                Ff(true);
            });
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

        if (FFMenuCheck == true || mp_teammates_are_enemies == true)
        {
            var gunMenu = new ChatMenu("Silah Menu");
            MenuHelper.GetGuns(gunMenu);
            ChatMenus.OpenMenu(player!, gunMenu);
        }
        else
        {
            player!.PrintToChat("FF açık olmadığı için silah menüsüne erişemezsin");
            return;
        }
    }

    private static void Ff(bool ac)
    {
        if (ac)
        {
            Server.ExecuteCommand("mp_teammates_are_enemies 1");
            Server.PrintToChatAll("FF Açık");
        }
        else
        {
            FFMenuCheck = false;
            Server.ExecuteCommand("mp_teammates_are_enemies 0");
            Server.PrintToChatAll("FF Kapalı");
        }
    }

    #endregion Ff
}