using CounterStrikeSharp.API.Core;

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
        player.strip_weapons();
        weapon_restrict = "knife";
    }
}