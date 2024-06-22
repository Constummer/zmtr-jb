using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Drug

    private CounterStrikeSharp.API.Modules.Timers.Timer DrugTimer;
    private List<CEnvShake?> Drugs = new();
    private List<CEnvFade?> Blinds = new();

    [ConsoleCommand("drugiptal")]
    [ConsoleCommand("drugsil")]
    public void OnUnDrugCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Lider))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }

        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        ClearDrugs();
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}herkesin {CC.B}drugunu {CC.W}kaldirdi.");
        return;
    }

    [ConsoleCommand("blindiptal")]
    [ConsoleCommand("blindsil")]
    public void OnUnBlindCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Lider))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }

        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        ClearBlinds(true);
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}herkesin {CC.B}blindini {CC.W}kaldirdi.");

        return;
    }

    private void ClearBlinds(bool withNew = false)
    {
        foreach (var item in Blinds)
        {
            if (item?.IsValid ?? false)
            {
                if (withNew)
                {
                    item.Duration = 0f;
                    item.HoldDuration = 600f;
                    item.FadeColor = Color.Transparent;
                    item.AcceptInput("Fade");
                }
                item?.Remove();
            }
        }
        Blinds.Clear();
    }

    private void ClearDrugs()
    {
        foreach (var item in Drugs)
        {
            if (item?.IsValid ?? false)
            {
                item.Duration = 1f;
                item.AcceptInput("StartShake");
                item?.Remove();
            }
        }
        Drugs.Clear();
    }

    [ConsoleCommand("blind")]
    public void OnBlindCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Lider))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }

        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        //var target = info.ArgCount > 1 ? info.ArgString.GetArg(0) : null;

        //var targetArgument = GetTargetArgument(target);

        //GetPlayers()
        //           .Where(x => x.PawnIsAlive && GetTargetAction(x, target, player))
        //           .ToList()
        //           .ForEach(x =>
        //           {
        //               if (ValidateCallerPlayer(x, false) == false) return;

        var blind = Utilities.CreateEntityByName<CEnvFade>("env_fade");
        if (blind == null)
        {
            return;
        }

        var xyz = player.PlayerPawn.Value.AbsOrigin;
        blind.Duration = 1f;
        blind.HoldDuration = 600f;
        blind.FadeColor = Color.Black;

        blind.Teleport(new Vector(xyz.X, xyz.Y, xyz.Z + 100), ANGLE_ZERO, VEC_ZERO);
        blind.DispatchSpawn();
        blind.AcceptInput("Fade");

        CustomSetParent(blind, player.PlayerPawn.Value);
        Blinds.Add(blind);
        //    if ((targetArgument & TargetForArgument.SingleUser) == targetArgument)
        //    {
        //        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuya {CC.B} blind {CC.W}verdi.");
        //    }
        //});
        //if ((targetArgument & TargetForArgument.SingleUser) != targetArgument)
        //{
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}herkese {CC.B}blind {CC.W}verdi.");
        //Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefine {CC.B}blind {CC.W}verdi.");
        //}
    }

    [ConsoleCommand("drug")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me>")]
    public void OnDrug2Command(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Lider))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }

        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        var target = info.ArgCount > 1 ? info.ArgString.GetArg(0) : null;

        var targetArgument = GetTargetArgument(target);

        GetPlayers()
                   .Where(x => x.PawnIsAlive && GetTargetAction(x, target, player))
                   .ToList()
                   .ForEach(x =>
                   {
                       if (ValidateCallerPlayer(x, false) == false) return;

                       var drug = Utilities.CreateEntityByName<CEnvShake>("env_shake");
                       if (drug == null)
                       {
                           return;
                       }

                       var xyz = x.PlayerPawn.Value.AbsOrigin;

                       drug.Amplitude = 10;
                       drug.Frequency = 255;
                       drug.Duration = 60;
                       drug.Radius = 50;

                       //coin.Duration = 50;
                       //coin.StopTime = 10;
                       //coin.NextShake = 10;
                       //coin.CurrentAmp = 10;

                       drug.Teleport(new Vector(xyz.X, xyz.Y, xyz.Z + 72), ANGLE_ZERO, VEC_ZERO);
                       drug.DispatchSpawn();
                       drug.AcceptInput("StartShake");

                       CustomSetParent(drug, x.PlayerPawn.Value);
                       Drugs.Add(drug);
                       if ((targetArgument & TargetForArgument.SingleUser) == targetArgument)
                       {
                           Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuya{CC.B} drug {CC.W}verdi.");
                       }
                   });
        if ((targetArgument & TargetForArgument.SingleUser) != targetArgument)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefine {CC.B}drug {CC.W}verdi.");
        }
    }

    #endregion Drug
}