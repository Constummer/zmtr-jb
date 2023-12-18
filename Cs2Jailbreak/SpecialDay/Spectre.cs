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
            if (spectre != null && is_valid_alive(spectre))
            {
                announce($"{spectre.PlayerName} is the spectre!");

                // give the spectre the HP and swap him
                set_health(spectre, count * 60);
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
            swap_all_t();

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
                set_colour(player, Color.FromArgb(0, 0, 0, 0));
                set_velocity(player, 2.5f);

                strip_weapons(player);

                // Work around for colour updates
                player.GiveNamedItem("weapon_decoy");
            }
            else
            {
                event_gun_menu(player);
            }
        }
    }
}