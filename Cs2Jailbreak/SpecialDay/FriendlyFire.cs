// base lr class
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class SDFriendlyFire : SDBase
    {
        public override void setup()
        {
            announce("Friendly fire day started");
            announce("Please 15 seconds for friendly fire to be enabled");
        }

        public override void start()
        {
            announce("Friendly fire enabled");
            enable_friendly_fire();
        }

        public override void end()
        {
            announce("Friendly fire day is over");
        }

        public override void setup_player(CCSPlayerController? player)
        {
            event_gun_menu(player);
        }
    }
}