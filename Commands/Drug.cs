using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Timers;
using static JailbreakExtras.JailbreakExtras;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Drug

    private CounterStrikeSharp.API.Modules.Timers.Timer DrugTimer;

    [ConsoleCommand("drugiptal")]
    [ConsoleCommand("drugsil")]
    public void OnUnDrugCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }

        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        DrugTimer?.Kill();
        DrugTimer = null;
        return;
    }

    [ConsoleCommand("drug1")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me> <0,1>")]
    public void OnDrugCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        Server.PrintToChatAll("1");

        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        Server.PrintToChatAll("2");

        var target = info.ArgCount > 1 ? info.ArgString.GetArg(0) : null;
        var godOneTwoStr = info.ArgCount > 2 ? info.ArgString.GetArg(1) : null;
        Server.PrintToChatAll("3");
        if (string.IsNullOrWhiteSpace(target) || string.IsNullOrWhiteSpace(godOneTwoStr))
        {
            Server.PrintToChatAll($"{target}");
            Server.PrintToChatAll($"{godOneTwoStr}");

            return;
        }
        Server.PrintToChatAll("4");
        if (int.TryParse(godOneTwoStr, out var value) == false)
        {
            player.PrintToChat($"{Prefix}{CC.W} gecersiz miktar");
            return;
        }
        if (value < 0 || value > 1)
        {
            player.PrintToChat($"{Prefix}{CC.W} 0 = kapatmak icin, 1 = acmak icin.");
            return;
        }
        var targetArgument = GetTargetArgument(target);
        DrugTimer = AddTimer(1f, () =>
        {
            GetPlayers()
                       .Where(x => x.PawnIsAlive && GetTargetAction(x, target, player))
                       .ToList()
                       .ForEach(x =>
                       {
                           if (ValidateCallerPlayer(x, false) == false) return;
                           Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefini {CC.B}drugladi");

                           var @event = new EventFlashbangDetonate(false)
                           {
                               Userid = x,
                               X = x.PlayerPawn.Value.AbsOrigin.X,
                               Y = x.PlayerPawn.Value.AbsOrigin.Y,
                               Z = x.PlayerPawn.Value.AbsOrigin.Z,
                           };
                           @event.FireEvent(false);

                           @event = null;
                       });
        }, Full);
    }

    [ConsoleCommand("drug2")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me> <0,1>")]
    public void OnDrug2Command(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }

        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var target = info.ArgCount > 1 ? info.ArgString.GetArg(0) : null;
        var godOneTwoStr = info.ArgCount > 2 ? info.ArgString.GetArg(1) : null;
        if (string.IsNullOrWhiteSpace(target) || string.IsNullOrWhiteSpace(godOneTwoStr))
        {
            return;
        }
        if (int.TryParse(godOneTwoStr, out var value) == false)
        {
            player.PrintToChat($"{Prefix}{CC.W} gecersiz miktar");
            return;
        }
        if (value < 0 || value > 1)
        {
            player.PrintToChat($"{Prefix}{CC.W} 0 = kapatmak icin, 1 = acmak icin.");
            return;
        }
        var targetArgument = GetTargetArgument(target);
        DrugTimer = AddTimer(1f, () =>
        {
            GetPlayers()
                       .Where(x => x.PawnIsAlive && GetTargetAction(x, target, player))
                       .ToList()
                       .ForEach(x =>
                       {
                           if (ValidateCallerPlayer(x, false) == false) return;

                           //var @event = new EventPlayerBlind(true)
                           //{
                           //    BlindDuration = 5f,
                           //    Userid = x,
                           //    Attacker = player
                           //};

                           //SetStateChanged(x.PlayerPawn.Value, "CCSPlayerPawnBase", "m_flFlashDuration");
                           //@event.FireEventToClient(x);
                           //@event.FireEvent(true);

                           x.PlayerPawn.Value.FlashDuration = 5f;
                           x.PlayerPawn.Value.BlindStartTime = 5f;
                           Utilities.SetStateChanged(x.PlayerPawn.Value, "CCSPlayerPawnBase", "m_flFlashDuration");

                           //@event = null;

                           //x.GiveNamedItem("weapon_flashbang");
                           //var ent = GetWeapon(x, "weapon_flashbang");
                           //if (ent != null && ent.IsValid)
                           //{
                           //    var @event = new EventPlayerBlind(false)
                           //    {
                           //        Userid = x,
                           //        BlindDuration = 5f,
                           //        Entityid = (int)ent.Index,
                           //        Attacker = x,
                           //    };
                           //    //var @event1 = new EventFlashbangDetonate(false)
                           //    //{
                           //    //    Userid = x,
                           //    //    X = x.PlayerPawn.Value.AbsOrigin.X,
                           //    //    Y = x.PlayerPawn.Value.AbsOrigin.Y,
                           //    //    Z = x.PlayerPawn.Value.AbsOrigin.Z,
                           //    //    Entityid = (int)ent.Index,
                           //    //};
                           //    @event.FireEventToClient(x);
                           //    @event.FireEvent(false);
                           //    //@event1.FireEventToClient(x);

                           //    //@event1 = null;
                           //    @event = null;
                           //}
                       });
        }, Full);
    }

    [ConsoleCommand("drug3")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me> <0,1>")]
    public void OnDrug3Command(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }

        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var target = info.ArgCount > 1 ? info.ArgString.GetArg(0) : null;
        var godOneTwoStr = info.ArgCount > 2 ? info.ArgString.GetArg(1) : null;
        if (string.IsNullOrWhiteSpace(target) || string.IsNullOrWhiteSpace(godOneTwoStr))
        {
            return;
        }
        if (int.TryParse(godOneTwoStr, out var value) == false)
        {
            player.PrintToChat($"{Prefix}{CC.W} gecersiz miktar");
            return;
        }
        if (value < 0 || value > 1)
        {
            player.PrintToChat($"{Prefix}{CC.W} 0 = kapatmak icin, 1 = acmak icin.");
            return;
        }
        var targetArgument = GetTargetArgument(target);
        DrugTimer = AddTimer(1f, () =>
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefini {CC.B}drugladi");

            GetPlayers()
                       .Where(x => x.PawnIsAlive && GetTargetAction(x, target, player))
                       .ToList()
                       .ForEach(x =>
                       {
                           if (ValidateCallerPlayer(x, false) == false) return;

                           var @event = new EventPlayerFalldamage(false)
                           {
                               Userid = x,
                               Damage = 0.1f
                           };
                           @event.FireEvent(false);

                           @event = null;
                       });
        }, Full);
    }

    #endregion Drug
}