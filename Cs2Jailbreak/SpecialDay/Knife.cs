using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class SDKnifeWarday : SDBase
    {
        public override void setup()
        {
            announce("knife warday started");
            announce("Please 15 seconds for damage be enabled");
        }

        public override void start()
        {
            announce("Fight!");
        }

        public override void end()
        {
            announce("knife warday is over");
        }

        public override void setup_player(CCSPlayerController player)
        {
            strip_weapons(player);
            weapon_restrict = "knife";
        }
    }
}