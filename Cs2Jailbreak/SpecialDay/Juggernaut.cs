using CounterStrikeSharp.API.Core;

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
        Lib.enable_friendly_fire();
    }

    public override void end()
    {
        announce("Juggernaut is over");
    }

    public override void death(CCSPlayerController? player, CCSPlayerController? attacker)
    {
        if (player == null || !player.is_valid() || attacker == null || !attacker.is_valid_alive())
        {
            return;
        }

        // Give attacker 100 hp
        attacker.set_health(attacker.get_health() + 100);
    }

    public override void setup_player(CCSPlayerController? player)
    {
        player.event_gun_menu();
    }
}