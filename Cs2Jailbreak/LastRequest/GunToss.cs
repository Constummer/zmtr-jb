using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class LRGunToss : LRBase
    {
        public LRGunToss(LastRequest manager, LastRequest.LRType type, int lr_slot, int player_slot, String choice) : base(manager, type, lr_slot, player_slot, choice)
        {
        }

        public override void init_player(CCSPlayerController player)
        {
            weapon_restrict = "deagle";
            player.GiveNamedItem("weapon_knife");
            player.GiveNamedItem("weapon_deagle");

            // empty ammo so players dont shoot eachother
            var deagle = Lib.find_weapon(player, "weapon_deagle");

            if (deagle != null)
            {
                deagle.set_ammo(0, 0);
            }
        }

        public override bool weapon_equip(String name)
        {
            return name.Contains("knife") || name.Contains("deagle");
        }
    }
}