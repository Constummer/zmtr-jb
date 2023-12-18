using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class LRScoutKnife : LRBase
    {
        public LRScoutKnife(LastRequest manager, LastRequest.LRType type, int lr_slot, int player_slot, String choice) : base(manager, type, lr_slot, player_slot, choice)
        {
        }

        public override void init_player(CCSPlayerController player)
        {
            player.GiveNamedItem("weapon_knife");
            player.GiveNamedItem("weapon_ssg08");
          set_gravity(  player,0.1f);
        }

        public override bool weapon_equip(String name)
        {
            return name.Contains("knife") || name.Contains("ssg08");
        }
    }
}