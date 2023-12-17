// TODO: we want to just copy hooks from other plugin and name them in here
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using CSTimer = CounterStrikeSharp.API.Modules.Timers;

public class Mute
{
    private void mute_t()
    {
        if (config.mute_t_allways)
        {
            return;
        }

        Lib.announce(MUTE_PREFIX, "All t's are muted for the first 30 seconds");

        Lib.mute_t();

        if (JailPlugin.global_ctx != null)
        {
            mute_timer = JailPlugin.global_ctx.AddTimer(30.0f, unmute_all, CSTimer.TimerFlags.STOP_ON_MAPCHANGE);
        }

        mute_active = true;
    }

    public void unmute_all()
    {
        Lib.announce(MUTE_PREFIX, "T's may now speak quietly");

        // Go through and unmute all alive players!
        foreach (CCSPlayerController player in Utilities.GetPlayers())
        {
            if (player.is_valid() && player.PawnIsAlive)
            {
                player.unmute();
            }
        }

        mute_timer = null;

        mute_active = false;
    }

    public void round_start()
    {
        Lib.kill_timer(ref mute_timer);

        mute_t();
    }

    public void round_end()
    {
        Lib.kill_timer(ref mute_timer);

        Lib.unmute_all();
    }

    public void connect(CCSPlayerController? player)
    {
        // just connected mute them
        player.mute();
    }

    public void apply_listen_flags(CCSPlayerController player)
    {
        // default to listen all
        player.listen_all();

        // if ct cannot hear team, change listen flags to team only
        if (player.is_ct() && config.ct_voice_only)
        {
            player.listen_team();
        }
    }

    public void spawn(CCSPlayerController? player)
    {
        if (!player.is_valid() || player == null)
        {
            return;
        }

        apply_listen_flags(player);

        if (config.mute_t_allways && player.is_t())
        {
            player.mute();
            return;
        }

        // no mute active or on ct unmute
        if (!mute_active || player.is_ct())
        {
            player.unmute();
        }
    }

    public void death(CCSPlayerController? player)
    {
        // mute on death
        if (!player.is_valid() || player == null)
        {
            return;
        }

        player.PrintToChat($"{MUTE_PREFIX}You are muted until the end of the round");

        player.mute();
    }

    public void switch_team(CCSPlayerController? player, int new_team)
    {
        if (!player.is_valid() || player == null)
        {
            return;
        }

        apply_listen_flags(player);

        // player not alive mute
        if (!player.PawnIsAlive)
        {
            player.mute();
        }

        // player is alive
        else
        {
            // on ct fine to unmute
            if (new_team == Lib.TEAM_CT)
            {
                player.unmute();
            }
            else
            {
                // mute timer active, mute the client
                if (mute_active || config.mute_t_allways)
                {
                    player.mute();
                }
            }
        }
    }

    public JailConfig config = new JailConfig();

    private CSTimer.Timer? mute_timer = null;

    private static readonly String MUTE_PREFIX = $" {ChatColors.Green}[MUTE]: {ChatColors.White}";

    // has the mute timer finished?
    private bool mute_active = false;
};