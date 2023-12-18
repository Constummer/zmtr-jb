// TODO: we want to just copy hooks from other plugin and name them in here
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using CSTimer = CounterStrikeSharp.API.Modules.Timers;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class Mute
    {
        private void mute_t()
        {
            if (config.mute_t_allways)
            {
                return;
            }

            announce(MUTE_PREFIX, "All t's are muted for the first 30 seconds");

            mute_t();

            mute_timer = instance.AddTimer(30.0f, unmute_all, CSTimer.TimerFlags.STOP_ON_MAPCHANGE);

            mute_active = true;
        }

        public void unmute_all()
        {
            announce(MUTE_PREFIX, "T's may now speak quietly");

            // Go through and unmute all alive players!
            foreach (CCSPlayerController player in Utilities.GetPlayers())
            {
                if (is_valid(player) && player.PawnIsAlive)
                {
                    unmute(player);
                }
            }

            mute_timer = null;

            mute_active = false;
        }

        public void round_start()
        {
            kill_timer(ref mute_timer);

            mute_t();
        }

        public void round_end()
        {
            kill_timer(ref mute_timer);

            unmute_all();
        }

        public void connect(CCSPlayerController? player)
        {
            // just connected mute them
            mute(player);
        }

        public void apply_listen_flags(CCSPlayerController player)
        {
            // default to listen all
            listen_all(player);

            // if ct cannot hear team, change listen flags to team only
            if (is_ct(player) && config.ct_voice_only)
            {
                listen_team(player);
            }
        }

        public void spawn(CCSPlayerController? player)
        {
            if (!is_valid(player) || player == null)
            {
                return;
            }

            apply_listen_flags(player);

            if (config.mute_t_allways && is_t(player))
            {
                mute(player);
                return;
            }

            // no mute active or on ct unmute
            if (!mute_active || is_ct(player))
            {
                unmute(player);
            }
        }

        public void death(CCSPlayerController? player)
        {
            // mute on death
            if (!is_valid(player) || player == null)
            {
                return;
            }

            player.PrintToChat($"{MUTE_PREFIX}You are muted until the end of the round");

            mute(player);
        }

        public void switch_team(CCSPlayerController? player, int new_team)
        {
            if (!is_valid(player) || player == null)
            {
                return;
            }

            apply_listen_flags(player);

            // player not alive mute
            if (!player.PawnIsAlive)
            {
                mute(player);
            }

            // player is alive
            else
            {
                // on ct fine to unmute
                if (new_team == TEAM_CT)
                {
                    unmute(player);
                }
                else
                {
                    // mute timer active, mute the client
                    if (mute_active || config.mute_t_allways)
                    {
                        mute(player);
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
}