using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class LRKnife : LRBase
    {
        public LRKnife(LastRequest manager, LastRequest.LRType type, int lr_slot, int player_slot, String choice) : base(manager, type, lr_slot, player_slot, choice)
        {
        }

        public override void init_player(CCSPlayerController player)
        {
            // give player a knife and restrict them to it
            player.GiveNamedItem("weapon_knife");
            weapon_restrict = "knife";

            // Handle options
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

                case "High speed":
                    {
                        set_velocity(player, 2.5f);
                        break;
                    }

                case "One hit":
                    {
                        set_health(player, 50);
                        break;
                    }
            }
        }
    }
}