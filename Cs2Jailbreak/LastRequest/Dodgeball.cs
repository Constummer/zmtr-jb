using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

public class LRDodgeball : LRBase
{
    public LRDodgeball(LastRequest manager, LastRequest.LRType type, int lr_slot, int player_slot, String choice) : base(manager, type, lr_slot, player_slot, choice)
    {
    }

    public override void init_player(CCSPlayerController player)
    {
        weapon_restrict = "flashbang";

        if (player.is_valid_alive())
        {
            player.set_health(1);

            player.GiveNamedItem("weapon_flashbang");

            switch (choice)
            {
                case "Vanilla":
                    {
                        break;
                    }

                case "Low gravity":
                    {
                        player.set_gravity(0.6f);
                        break;
                    }
            }
        }
    }

    public override void player_hurt(int damage, int health, int hitgroup)
    {
        CCSPlayerController? player = Utilities.GetPlayerFromSlot(player_slot);

        if (player != null && player.is_valid_alive())
        {
            player.slay();
        }
    }

    public override void grenade_thrown()
    {
        CCSPlayerController? player = Utilities.GetPlayerFromSlot(player_slot);
        Lib.give_event_nade_delay(player, 1.4f, "weapon_flashbang");
    }

    public override void ent_created(CEntityInstance entity)
    {
        Lib.remove_ent_delay(entity, 1.4f, "flashbang_projectile");
    }
}