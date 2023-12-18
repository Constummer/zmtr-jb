using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class SDDodgeball : SDBase
    {
        public override void setup()
        {
            announce("Dodgeball started");
            announce("Please 15 seconds for damage be enabled");
        }

        public override void start()
        {
            announce("Fight!");
        }

        public override void end()
        {
            announce("Dodgeball is over");
        }

        public override void setup_player(CCSPlayerController player)
        {
            strip_weapons(player, true);
            player.GiveNamedItem("weapon_flashbang");
            weapon_restrict = "flashbang";
        }

        public override void grenade_thrown(CCSPlayerController? player)
        {
            give_event_nade_delay(player, 1.4f, "weapon_flashbang");
        }

        public override void player_hurt(CCSPlayerController? player, int damage, int health, int hitgroup)
        {
            if (player != null && is_valid_alive(player))
            {
                slay(player);
            }
        }

        public override void ent_created(CEntityInstance entity)
        {
            remove_ent_delay(entity, 1.4f, "flashbang_projectile");
        }
    }
}