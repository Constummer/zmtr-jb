using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class LRGrenade : LRBase
    {
        public LRGrenade(LastRequest manager, LastRequest.LRType type, int lr_slot, int player_slot, String choice) : base(manager, type, lr_slot, player_slot, choice)
        {
        }

        public override void init_player(CCSPlayerController player)
        {
            weapon_restrict = "hegrenade";

            if (is_valid_alive(player))
            {
                set_health(player, 150);

                player.GiveNamedItem("weapon_hegrenade");

                switch (choice)
                {
                    case "Vanilla":
                        {
                            break;
                        }

                    case "Low gravity":
                        {
                            set_gravity(player, 0.6f);
                            break;
                        }
                }
            }
        }

        public override void grenade_thrown()
        {
            CCSPlayerController? player = Utilities.GetPlayerFromSlot(player_slot);
            strip_weapons(player, true);
            give_event_nade_delay(player, 1.4f, "weapon_hegrenade");
        }
    }
}