using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private CounterStrikeSharp.API.Modules.Timers.Timer SinirsizBombaTimer = null;
    private CounterStrikeSharp.API.Modules.Timers.Timer SinirsizMolyTimer = null;

    #region SinirsizBomba

    [ConsoleCommand("sinirsizbomba")]
    [ConsoleCommand("smbomba")]
    [CommandHelper(minArgs: 1, usage: "<0/1>")]
    public void SinirsizBomba(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        if (info.ArgCount < 2) return;
        var oneTwoStr = info.ArgCount == 2 ? info.GetArg(1) : "0";
        int.TryParse(oneTwoStr, out var oneTwo);
        if (oneTwo < 0 || oneTwo > 1)
        {
            player.PrintToChat($" {CC.LR}[ZMTR]{CC.W} 0 = kapatmak icin, 1 = acmak icin.");
            return;
        }
        SinirsizBombaTimer = GiveSinirsizCustomNade(oneTwo, SinirsizBombaTimer, "weapon_hegrenade");
    }

    [ConsoleCommand("sinirsizmolotof")]
    [ConsoleCommand("smmolotof")]
    [CommandHelper(minArgs: 1, usage: "<0/1>")]
    public void SinirsizMoly(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        if (info.ArgCount < 2) return;
        var oneTwoStr = info.ArgCount == 2 ? info.GetArg(1) : "0";
        int.TryParse(oneTwoStr, out var oneTwo);
        if (oneTwo < 0 || oneTwo > 1)
        {
            player.PrintToChat($" {CC.LR}[ZMTR]{CC.W} 0 = kapatmak icin, 1 = acmak icin.");
            return;
        }
        SinirsizMolyTimer = GiveSinirsizCustomNade(oneTwo, SinirsizMolyTimer, "weapon_incgrenade");
    }

    private CounterStrikeSharp.API.Modules.Timers.Timer? GiveSinirsizCustomNade(int oneTwo, CounterStrikeSharp.API.Modules.Timers.Timer sinirsizBombaTimer, string nadeName)
    {
        switch (oneTwo)
        {
            case 0:
                sinirsizBombaTimer?.Kill();
                sinirsizBombaTimer = null;
                break;

            case 1:
                sinirsizBombaTimer = AddTimer(3f, () =>
                {
                    GetPlayers()
                    .Where(x => x.PawnIsAlive)
                    .ToList()
                    .ForEach(x =>
                    {
                        if (x?.PlayerPawn?.Value?.WeaponServices?.MyWeapons != null)
                        {
                            if (x.PlayerPawn.Value.WeaponServices!.MyWeapons
                            .Any(weapon => weapon.Value != null
                                    && string.IsNullOrWhiteSpace(weapon.Value.DesignerName) == false
                                    && weapon.Value.DesignerName != "[null]" &&
                                    weapon.Value.DesignerName == nadeName
                            ))
                            {
                                return;
                            }
                            else
                            {
                                x.GiveNamedItem(nadeName);
                            }
                        }
                    });
                }, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
                break;

            default:
                break;
        }
        return sinirsizBombaTimer;
    }

    #endregion SinirsizBomba
}