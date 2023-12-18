using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class LRRussianRoulette : LRBase
    {
        public LRRussianRoulette(LastRequest manager, LastRequest.LRType type, int lr_slot, int player_slot, String choice) : base(manager, type, lr_slot, player_slot, choice)
        {
        }

        public override void init_player(CCSPlayerController player)
        {
            weapon_restrict = "deagle";

            player.GiveNamedItem("weapon_deagle");

            var deagle = find_weapon(player, "weapon_" + weapon_restrict);

            if (deagle != null)
            {
                set_ammo(deagle, 0, 0);
            }

            restrict_damage = true;
        }

        public override void pair_activate()
        {
            (CCSPlayerController? winner, CCSPlayerController? loser, LRBase? winner_lr_base) = pick_rand_player();

            LRRussianRoulette? winner_lr = (LRRussianRoulette?)winner_lr_base;

            // Give the lucky player the first shot
            if (winner != null && loser != null && winner_lr != null)
            {
                announce(winner, LastRequest.LR_PREFIX, $"Randomly chose {winner.PlayerName} to shoot first");
                announce(loser, LastRequest.LR_PREFIX, $"Randomly chose {winner.PlayerName} to shoot first");

                winner_lr.reload_clip();
            }
        }

        public override void weapon_fire(String name)
        {
            CCSPlayerController? player = Utilities.GetPlayerFromSlot(player_slot);

            if (name.Contains(weapon_restrict) && player != null && is_valid(player))
            {
                Random rnd = new Random((int)DateTime.Now.Ticks);

                // Bang!
                if (rnd.Next(0, 7) == 6)
                {
                    slay(player);
                    announce(LastRequest.LR_PREFIX, $"{player.PlayerName} brains splattered against the wall");
                }
                else if (partner != null)
                {
                    announce(player, LastRequest.LR_PREFIX, "Click!");
                    var lr_shot = (LRRussianRoulette)partner;
                    lr_shot.reload_clip();
                }
            }
        }

        private void reload_clip()
        {
            CCSPlayerController? player = Utilities.GetPlayerFromSlot(player_slot);

            if (player != null && is_valid_alive(player))
            {
                player.PrintToChat($"{LastRequest.LR_PREFIX} Reload!");

                var deagle = find_weapon(player, "weapon_" + weapon_restrict);

                // NOTE: this doesn't update the unload state
                // however giving a new gun doesn't work either because it doesnt register fast enough
                // also taking a gun away too quickly after a shot will cause it not to register
                if (deagle != null)
                {
                    set_ammo(deagle, 1, 0);
                }
            }
        }
    }
}