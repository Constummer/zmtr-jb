// base lr class
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class SDScoutKnife : SDBase
    {
        public override void setup()
        {
            announce("Scout knife started");
            announce("Please 15 seconds for damage be enabled");
        }

        public override void start()
        {
            announce("Fight!");
        }

        public override void end()
        {
            announce("Scout knife is over");
        }

        public override void setup_player(CCSPlayerController player)
        {
            strip_weapons(player);
            player.GiveNamedItem("weapon_ssg08");
            set_gravity(player, 0.1f);
        }

        public override bool weapon_equip(CCSPlayerController player, String name)
        {
            return name.Contains("knife") || name.Contains("ssg08");
        }

        public override void cleanup_player(CCSPlayerController player)
        {
            set_gravity(player, 1.0f);
        }
    }
}