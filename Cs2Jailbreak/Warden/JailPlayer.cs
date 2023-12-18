using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class JailPlayer
    {
        public void purge_round()
        {
            is_rebel = false;
        }

        public void reset()
        {
            purge_round();

            // TODO: reset client specific settings
        }

        public void set_rebel(CCSPlayerController? player)
        {
            if (event_active())
            {
                return;
            }

            // ignore if they are in lr
            if (lr.in_lr(player))
            {
                return;
            }

            // dont care if player is invalid
            if (!is_valid(player) || player == null)
            {
                return;
            }

            // on T with no warday or sd active
            if (player.TeamNum == TEAM_T)
            {
                is_rebel = true;
            }
        }

        public void rebel_death(CCSPlayerController? player, CCSPlayerController? killer)
        {
            // event active dont care
            if (event_active())
            {
                return;
            }

            // players aernt valid dont care
            if (killer == null || player == null || !is_valid(player) || !is_valid(killer))
            {
                return;
            }

            // print death if player is rebel and killer on CT
            if (is_rebel && killer.TeamNum == TEAM_CT)
            {
                Server.PrintToChatAll($" {ChatColors.Green}[REBEL]: {ChatColors.White}{killer.PlayerName} killed the rebel {player.PlayerName}");
            }
        }

        public void rebel_weapon_fire(CCSPlayerController? player, String weapon)
        {
            // ignore weapons players are meant to have
            if (weapon != "knife" && weapon != "c4")
            {
                set_rebel(player);
            }
        }

        public void player_hurt(CCSPlayerController? player, CCSPlayerController? attacker, int health, int damage)
        {
            if (player == null || attacker == null || !is_valid(player) || !is_valid(attacker))
            {
                return;
            }

            // ct hit by T they are a rebel
            if (is_ct(player) && is_t(attacker))
            {
                set_rebel(attacker);
            }

            // log any ct damage
            else if (is_ct(attacker))
            {
                //print_console_all($"CT {attacker.PlayerName} hit {player.PlayerName} for {damage}");
            }
        }

        // TODO: Laser stuff needs to go here!
        // but we dont have access to the necessary primtives yet

        public bool is_rebel = false;
    };
}