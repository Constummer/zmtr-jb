using CounterStrikeSharp.API.Core;

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
        player.strip_weapons(true);
        player.set_health(175);
        player.GiveNamedItem("weapon_hegrenade");
        weapon_restrict = "hegrenade";
    }

    public override void grenade_thrown(CCSPlayerController? player)
    {
        Lib.give_event_nade_delay(player, 1.4f, "weapon_hegrenade");
    }
}