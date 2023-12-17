using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

public class LRNoScope : LRBase
{
    public LRNoScope(LastRequest manager, LastRequest.LRType type, int lr_slot, int player_slot, String choice) : base(manager, type, lr_slot, player_slot, choice)
    {
    }

    private void give_weapon(CCSPlayerController? player)
    {
        if (player == null || !player.is_valid())
        {
            return;
        }

        player.strip_weapons(true);

        switch (choice)
        {
            case "Scout":
                {
                    weapon_restrict = "ssg08";
                    player.GiveNamedItem("weapon_ssg08");
                    break;
                }

            case "Awp":
                {
                    weapon_restrict = "awp";
                    player.GiveNamedItem("weapon_awp");
                    break;
                }
        }
    }

    public override void init_player(CCSPlayerController player)
    {
        give_weapon(player);
    }

    public override void weapon_zoom()
    {
        CCSPlayerController? player = Utilities.GetPlayerFromSlot(player_slot);

        // re give the weapons so they cannot zoom
        give_weapon(player);
    }
}