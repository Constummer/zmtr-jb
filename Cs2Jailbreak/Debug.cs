using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

// debugging commands:
// TODO: these need to be sealed by admin
public static class Debug
{
    [RequiresPermissions("@jail/debug")]
    public static void nuke(CCSPlayerController? invoke, CommandInfo command)
    {
        Lib.announce(DEBUG_PREFIX, "Slaying all players");

        foreach (CCSPlayerController player in Utilities.GetPlayers())
        {
            player.slay();
        }
    }

    [RequiresPermissions("@jail/debug")]
    public static void force_open_cmd(CCSPlayerController? invoke, CommandInfo command)
    {
        Lib.force_open();
    }

    [RequiresPermissions("@jail/debug")]
    public static void test_laser(CCSPlayerController? invoke, CommandInfo command)
    {
        CCSPlayerPawn? pawn = invoke.pawn();

        if (pawn != null && pawn.AbsOrigin != null)
        {
            Vector end = new Vector(pawn.AbsOrigin.X + 100.0f, pawn.AbsOrigin.Y, pawn.AbsOrigin.Z + 100.0f);
            //Vector end = pawn.LookTargetPosition;

            if (invoke != null)
            {
                invoke.PrintToChat($"end: {end.X} {end.Y} {end.Z}");
            }

            Lib.draw_laser(pawn.AbsOrigin, end, 10.0f, 2.0f, Lib.CYAN);
        }
    }

    [RequiresPermissions("@jail/debug")]
    public static void test_strip_cmd(CCSPlayerController? invoke, CommandInfo command)
    {
        invoke.strip_weapons(true);
    }

    [RequiresPermissions("@jail/debug")]
    public static void join_ct_cmd(CCSPlayerController? invoke, CommandInfo command)
    {
        if (invoke != null && invoke.is_valid())
        {
            invoke.SwitchTeam(CsTeam.CounterTerrorist);
        }
    }

    [RequiresPermissions("@jail/debug")]
    public static void hide_weapon_cmd(CCSPlayerController? invoke, CommandInfo command)
    {
        if (invoke != null && invoke.is_valid())
        {
            invoke.PrintToChat("hiding weapons");
        }

        invoke.hide_weapon();
    }

    [RequiresPermissions("@jail/debug")]
    public static void is_muted_cmd(CCSPlayerController? invoke, CommandInfo command)
    {
        if (invoke == null || !invoke.is_valid())
        {
            return;
        }

        invoke.PrintToConsole("Is muted?");

        foreach (CCSPlayerController player in Utilities.GetPlayers())
        {
            invoke.PrintToConsole($"{player.PlayerName} : {player.VoiceFlags.HasFlag(VoiceFlags.Muted)} : {player.VoiceFlags.HasFlag(VoiceFlags.ListenAll)} : {player.VoiceFlags.HasFlag(VoiceFlags.ListenTeam)}");
        }
    }

    // are these commands allowed or not?
    public static readonly bool enable = true;

    public static readonly String DEBUG_PREFIX = $" {ChatColors.Green}[DEBUG]: {ChatColors.White}";
}