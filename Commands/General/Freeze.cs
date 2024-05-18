using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private CounterStrikeSharp.API.Modules.Timers.Timer fzTimer = null;

    #region Freeze-Unfreeze

    [ConsoleCommand("fz", "Freeze a player.")]
    [ConsoleCommand("freezetime", "Freeze a player.")]
    [CommandHelper(1, "<saniye>")]
    public void OnFzCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.ArgString.GetArg(0);

        if (int.TryParse(target, out int value))
        {
            if (value > 120)
            {
                player.PrintToChat("Max 120 sn girebilirsin");
            }
            else
            {
                LogManagerCommand(player.SteamID, info.GetCommandString);
                BasicCountdown.CommandStartTextCountDown(this, $"{T_PluralCamelPossesive} donmasına {value} saniye kaldı!");
                fzTimer?.Kill();
                fzTimer = AddTimer(value, () =>
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
                        if (ValidateCallerPlayer(x, false) == false) return;
                        if (TeamActive == false)
                        {
                            SetColour(x, Config.Burry.BuryColor);
                        }
                        SetMoveType(x, MoveType_t.MOVETYPE_OBSOLETE);

                        RefreshPawnTP(x);

                        //Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
                        //Vector currentSpeed = new Vector(0, 0, 0);
                        //QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
                        //x.PlayerPawn.Value.Teleport(currentPosition, currentRotation, currentSpeed);
                    });
                    FreezeOrUnfreezeSound();
                    Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{T_PluralLowerObjective} {CC.B}dondurdu{CC.W}.");
                }, SOM);
            }
        }
    }

    [ConsoleCommand("freeze", "Freeze a player.")]
    [ConsoleCommand("don", "Freeze a player.")]
    [CommandHelper(1, "<playerismi-@all-@t-@ct-@me>")]
    public void OnFreezeCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, Perm_Seviye4) == false)
        {
            return;
        }
        var target = info.ArgString.GetArgSkip(0);
        var targetArgument = GetTargetArgument(target);
        LogManagerCommand(player.SteamID, info.GetCommandString);

        GetPlayers()
                   .Where(x => x.PawnIsAlive && GetTargetAction(x, target, player))
                   .ToList()
                   .ForEach(x =>
                   {
                       if ((targetArgument & TargetForArgument.SingleUser) == targetArgument)
                       {
                           Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu{CC.B} dondurdu{CC.W}.");
                       }
                       if (TeamActive == false)
                       {
                           SetColour(x, Config.Burry.BuryColor);
                       }

                       SetMoveType(x, MoveType_t.MOVETYPE_OBSOLETE);
                       RefreshPawnTP(x);
                       //Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
                       //Vector currentSpeed = new Vector(0, 0, 0);
                       //QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
                       //x.PlayerPawn.Value.Teleport(currentPosition, currentRotation, currentSpeed);
                   });
        FreezeOrUnfreezeSound();
        if ((targetArgument & TargetForArgument.SingleUser) != targetArgument)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefini {CC.B}dondurdu");
        }
    }

    #endregion Freeze-Unfreeze
}