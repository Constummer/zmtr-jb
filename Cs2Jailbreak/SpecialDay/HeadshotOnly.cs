// base lr class
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class SDHeadshotOnly : SDBase
    {
        public override void setup()
        {
            announce("Headshot only started");
            announce("Please 15 seconds for damage be enabled");
        }

        public override void start()
        {
            announce("Fight!");
        }

        public override void end()
        {
            announce("Headshot only is over");
        }

        public override void setup_player(CCSPlayerController player)
        {
            strip_weapons(player, true);
            player.GiveNamedItem("weapon_deagle");
            weapon_restrict = "deagle";
        }

        public override void player_hurt(CCSPlayerController? player, int health, int damage, int hitgroup)
        {
            if (player == null || !is_valid_alive(player))
            {
                return;
            }

            // dont allow damage when its not to head
            if (hitgroup != HITGROUP_HEAD)
            {
                restore_hp(player, damage, health);
            }
        }
    }
}