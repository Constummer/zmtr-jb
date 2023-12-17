// base lr class
using CounterStrikeSharp.API.Core;

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
        Lib.enable_friendly_fire();
    }

    public override void end()
    {
        announce("Friendly fire day is over");
    }

    public override void setup_player(CCSPlayerController? player)
    {
        player.event_gun_menu();
    }
}