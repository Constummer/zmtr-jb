using CounterStrikeSharp.API.Core;

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
        player.strip_weapons(true);
        player.GiveNamedItem("weapon_flashbang");
        weapon_restrict = "flashbang";
    }

    public override void grenade_thrown(CCSPlayerController? player)
    {
        Lib.give_event_nade_delay(player, 1.4f, "weapon_flashbang");
    }

    public override void player_hurt(CCSPlayerController? player, int damage, int health, int hitgroup)
    {
        if (player != null && player.is_valid_alive())
        {
            player.slay();
        }
    }

    public override void ent_created(CEntityInstance entity)
    {
        Lib.remove_ent_delay(entity, 1.4f, "flashbang_projectile");
    }
}