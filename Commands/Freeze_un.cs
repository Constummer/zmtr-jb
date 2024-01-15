using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private CounterStrikeSharp.API.Modules.Timers.Timer unfzTimer = null;

    #region Unfreeze

    [ConsoleCommand("unfz", "Freeze a player.")]
    [ConsoleCommand("ufz", "Freeze a player.")]
    [ConsoleCommand("unfreezetime", "Freeze a player.")]
    [CommandHelper(1, "<saniye>")]
    public void OnUnFzCommand(CCSPlayerController? player, CommandInfo info)
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
                BasicCountdown.CommandStartTextCountDown(this, $"Mahkûmların donunun bozulmasına {value} saniye kaldı!");
                unfzTimer?.Kill();
                unfzTimer = AddTimer(value, () =>
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
                            SetColour(x, DefaultColor);
                        }
                        x.PlayerPawn.Value!.MoveType = MoveType_t.MOVETYPE_WALK;
                        Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
                        Vector currentSpeed = new Vector(0, 0, 0);
                        QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
                        x.PlayerPawn.Value.Teleport(currentPosition, currentRotation, currentSpeed);
                    });
                    FreezeOrUnfreezeSound();
                    Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}mahkûmların {CC.B}donunu bozdu{CC.W}.");
                }, SOM);
            }
        }
    }

    [ConsoleCommand("unfreeze", "Unfreeze a player.")]
    [CommandHelper(1, "<playerismi-@all-@t-@ct-@me>")]
    public void OnUnfreezeCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye4") == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);
        var targetArgument = GetTargetArgument(target);

        bool randomFreeze = false;
        GetPlayers()
            .Where(x => x.PawnIsAlive && GetTargetAction(x, target, player.PlayerName))
            .ToList()
            .ForEach(x =>
            {
                if (targetArgument == TargetForArgument.None)
                {
                    Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncunun{CC.B} donunu bozdu{CC.W}.");
                }
                if (TeamActive == false)
                {
                    SetColour(x, DefaultColor);
                }
                RefreshPawn(x);

                x.PlayerPawn.Value.MoveType = MoveType_t.MOVETYPE_WALK;
            });
        FreezeOrUnfreezeSound();
        if (targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefinin {CC.B}donunu {CC.W}bozdu.");
        }
    }

    #endregion Unfreeze
}