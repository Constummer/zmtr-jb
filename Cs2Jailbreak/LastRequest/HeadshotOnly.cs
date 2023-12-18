using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class LRHeadshotOnly : LRBase
    {
        public LRHeadshotOnly(LastRequest manager, LastRequest.LRType type, int lr_slot, int player_slot, String choice) : base(manager, type, lr_slot, player_slot, choice)
        {
        }

        public override void init_player(CCSPlayerController player)
        {
            weapon_restrict = "deagle";

            player.GiveNamedItem("weapon_deagle");
        }

        public override void player_hurt(int health, int damage, int hitgroup)
        {
            // dont allow damage when its not to head
            if (hitgroup != HITGROUP_HEAD)
            {
                CCSPlayerController? player = Utilities.GetPlayerFromSlot(player_slot);
                restore_hp(player, damage, health);
            }
        }
    }
}