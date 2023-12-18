using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class SDJuggernaut : SDBase
    {
        public override void setup()
        {
            announce("Juggernaut started");
            announce("Please 15 seconds for friendly fire to be enabled");
        }

        public override void start()
        {
            announce("Friendly fire enabled");
            enable_friendly_fire();
        }

        public override void end()
        {
            announce("Juggernaut is over");
        }

        public override void death(CCSPlayerController? player, CCSPlayerController? attacker)
        {
            if (player == null || !is_valid(player) || attacker == null || !is_valid_alive(attacker))
            {
                return;
            }

            // Give attacker 100 hp
            set_health(attacker, get_health(attacker) + 100);
        }

        public override void setup_player(CCSPlayerController? player)
        {
            event_gun_menu(player);
        }
    }
}