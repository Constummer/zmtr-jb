using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class SDTank : SDBase
    {
        public override void setup()
        {
            announce("tank day started");
            announce("Please 15 seconds for damage be enabled");
        }

        public override void make_boss(CCSPlayerController? tank, int count)
        {
            if (tank != null && is_valid_alive(tank))
            {
                announce($"{tank.PlayerName} is the tank!");

                // give the tank the HP and swap him
                set_health(tank, count * 100);
                set_colour(tank, RED);
                tank.SwitchTeam(CsTeam.CounterTerrorist);

                // Work around for colour updates
                tank.GiveNamedItem("weapon_decoy");
            }
            else
            {
                announce("Error picking tank");
            }
        }

        public override void start()
        {
            swap_all_t();

            (boss, int count) = pick_boss();
            make_boss(boss, count);
        }

        public override void end()
        {
            announce("tank day is over");
        }

        public override void setup_player(CCSPlayerController player)
        {
            event_gun_menu(player);
        }
    }
}