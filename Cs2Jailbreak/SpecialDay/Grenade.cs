using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class SDGrenade : SDBase
    {
        public override void setup()
        {
            announce("Grenade started");
            announce("Please 15 seconds for damage be enabled");
        }

        public override void start()
        {
            announce("Fight!");
        }

        public override void end()
        {
            announce("Grenade is over");
        }

        public override void setup_player(CCSPlayerController player)
        {
            strip_weapons(player, true);
            set_health(player, 175);
            player.GiveNamedItem("weapon_hegrenade");
            weapon_restrict = "hegrenade";
        }

        public override void grenade_thrown(CCSPlayerController? player)
        {
            give_event_nade_delay(player, 1.4f, "weapon_hegrenade");
        }
    }
}