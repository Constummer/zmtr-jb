using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class LRShotgunWar : LRBase
    {
        public LRShotgunWar(LastRequest manager, LastRequest.LRType type, int lr_slot, int player_slot, String choice) : base(manager, type, lr_slot, player_slot, choice)
        {
        }

        public override void init_player(CCSPlayerController player)
        {
            // give shotty health and plenty of ammo
            weapon_restrict = "xm1014";
            player.GiveNamedItem("weapon_xm1014");

            player.set_health(1000);

            var shotgun = Lib.find_weapon(player, "xm1014");

            if (shotgun != null)
            {
                shotgun.set_ammo(999, 999);
            }
        }
    }
}