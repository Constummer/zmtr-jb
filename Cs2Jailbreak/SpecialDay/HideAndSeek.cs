using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class SDHideAndSeek : SDBase
    {
        public override void setup()
        {
            announce("hide and seek started");
            announce("T's have 15 seconds to hide");
        }

        public override void start()
        {
            // unfreeze all players
            foreach (CCSPlayerController? player in Utilities.GetPlayers())
            {
                if (player == null || !is_valid_alive(player))
                {
                    continue;
                }

                if (is_t(player))
                {
                    // dumb workaround
                    player.GiveNamedItem("weapon_knife");
                }

                unfreeze(player);
            }

            announce("Seekers released!");
        }

        public override void end()
        {
            announce("hide and seek is over");
        }

        public override void setup_player(CCSPlayerController player)
        {
            // lock them in place 500 hp, gun menu
            if (is_ct(player))
            {
                freeze(player);
                event_gun_menu(player);
                set_health(player, 500);
            }

            // invis
            else
            {
                set_colour(player, Color.FromArgb(0, 0, 0, 0));
                strip_weapons(player, true);
            }
        }

        public override void cleanup_player(CCSPlayerController player)
        {
            unfreeze(player);
        }
    }
}