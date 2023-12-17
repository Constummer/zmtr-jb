using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class SDSpectre : SDBase
    {
        public override void setup()
        {
            announce("spectre started");
            announce("Please 15 seconds for damage be enabled");
        }

        public override void make_boss(CCSPlayerController? spectre, int count)
        {
            if (spectre != null && spectre.is_valid_alive())
            {
                announce($"{spectre.PlayerName} is the spectre!");

                // give the spectre the HP and swap him
                spectre.set_health(count * 60);
                spectre.SwitchTeam(CsTeam.CounterTerrorist);

                setup_player(spectre);
            }
            else
            {
                announce("Error picking spectre");
            }
        }

        public override bool weapon_equip(CCSPlayerController player, String name)
        {
            // spectre can only carry a knife
            if (is_boss(player))
            {
                return name.Contains("knife") || name.Contains("decoy");
            }

            return true;
        }

        public override void start()
        {
            Lib.swap_all_t();

            (boss, int count) = pick_boss();
            make_boss(boss, count);
        }

        public override void end()
        {
            announce("spectre is over");
        }

        public override void setup_player(CCSPlayerController player)
        {
            if (is_boss(player))
            {
                // invis and speed
                player.set_colour(Color.FromArgb(0, 0, 0, 0));
                player.set_velocity(2.5f);

                player.strip_weapons();

                // Work around for colour updates
                player.GiveNamedItem("weapon_decoy");
            }
            else
            {
                player.event_gun_menu();
            }
        }
    }
}