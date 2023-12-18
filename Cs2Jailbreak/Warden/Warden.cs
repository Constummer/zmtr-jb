using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;
using CSTimer = CounterStrikeSharp.API.Modules.Timers;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class Warden
    {
        public Warden()
        {
            for (int p = 0; p < jail_players.Length; p++)
            {
                jail_players[p] = new JailPlayer();
            }
        }

        private void announce(String message)
        {
            JailbreakExtras.announce(WARDEN_PREFIX, message);
        }

        // Give a player warden
        public void set_warden(int? new_slot_opt)
        {
            if (new_slot_opt == null)
            {
                return;
            }

            warden_slot = new_slot_opt.Value;

            var player = Utilities.GetPlayerFromSlot(warden_slot);

            // one last saftey check
            if (!is_valid(player))
            {
                warden_slot = INAVLID_SLOT;
                return;
            }

            announce($"{player.PlayerName} is now the warden");

            JailbreakExtras.announce(player, WARDEN_PREFIX, "Type !wcommands to see a full list of warden commands");

            // change player color!
            set_colour(player, Color.FromArgb(255, 0, 0, 255));
        }

        public bool is_warden(CCSPlayerController? player)
        {
            return slot(player) == warden_slot;
        }

        public void remove_warden()
        {
            var player = Utilities.GetPlayerFromSlot(warden_slot);

            if (is_valid(player))
            {
                set_colour(player, Color.FromArgb(255, 255, 255, 255));
                announce($"{player.PlayerName} is no longer the warden");
            }

            warden_slot = INAVLID_SLOT;
        }

        public void remove_if_warden(CCSPlayerController? player)
        {
            if (!is_valid(player) || player == null)
            {
                return;
            }

            if (is_warden(player))
            {
                remove_warden();
            }
        }

        public void leave_warden_cmd(CCSPlayerController? player, CommandInfo command)
        {
            remove_if_warden(player);
        }

        [RequiresPermissions("@css/generic")]
        public void remove_warden_cmd(CCSPlayerController? player, CommandInfo command)
        {
            announce("Warden removed");
            remove_warden();
        }

        public void warday_cmd(CCSPlayerController? player, CommandInfo command)
        {
            if (player == null || !is_valid(player))
            {
                return;
            }

            // must be warden
            if (!is_warden(player))
            {
                player.PrintToChat($"{WARDEN_PREFIX}You must be a warden to call a warday");
                return;
            }

            // must specify location
            if (command.ArgCount != 2)
            {
                player.PrintToChat($"{WARDEN_PREFIX}Usage !wd <location>");
                return;
            }

            // attempt the start the warday
            String location = command.ArgByIndex(1);

            if (!warday.start_warday(location))
            {
                player.PrintToChat($"{WARDEN_PREFIX}You cannot call a warday for another {Warday.ROUND_LIMIT - warday.round_counter} rounds");
            }
        }

        public void wub_cmd(CCSPlayerController? player, CommandInfo command)
        {
            if (player == null || !is_valid(player))
            {
                return;
            }

            // must be warden
            if (!is_warden(player))
            {
                player.PrintToChat($"{WARDEN_PREFIX}You must be a warden to use wub");
                return;
            }

            block.unblock_all();
        }

        public void wb_cmd(CCSPlayerController? player, CommandInfo command)
        {
            if (player == null || !is_valid(player))
            {
                return;
            }

            // must be warden
            if (!is_warden(player))
            {
                player.PrintToChat($"{WARDEN_PREFIX}You must be a warden to use wb");
                return;
            }

            block.block_all();
        }

        // debug command
        [RequiresPermissions("@jail/debug")]
        public void is_rebel_cmd(CCSPlayerController? invoke, CommandInfo command)
        {
            if (invoke == null || !is_valid(invoke))
            {
                return;
            }

            invoke.PrintToConsole("rebels\n");

            foreach (CCSPlayerController player in Utilities.GetPlayers())
            {
                if (!is_valid(player))
                {
                    continue;
                }

                int? slots = slot(player);

                if (slots != null)
                {
                    invoke.PrintToConsole($"{jail_players[slots.Value].is_rebel} : {player.PlayerName}\n");
                }
            }
        }

        public void cmd_info(CCSPlayerController? player, CommandInfo command)
        {
            if (player == null || !is_valid(player))
            {
                return;
            }

            player.PrintToChat("!w - take warden");
            player.PrintToChat("!wd - start a warday");
            player.PrintToChat("!uw - leave warden");
            player.PrintToChat("!wb - enable block");
            player.PrintToChat("!wub - disable block");
            player.PrintToChat("!rw - admin remove warden");
        }

        public void take_warden_cmd(CCSPlayerController? player, CommandInfo command)
        {
            // invalid player we dont care
            if (!is_valid(player) || player == null)
            {
                return;
            }

            // player must be alive
            if (!player.PawnIsAlive)
            {
                player.PrintToChat($"{WARDEN_PREFIX}You must be alive to warden");
            }

            // check team is valid
            else if (!is_ct(player))
            {
                player.PrintToChat($"{WARDEN_PREFIX}You must be a CT to warden");
            }

            // check there is no warden
            else if (warden_slot != INAVLID_SLOT)
            {
                var warden = Utilities.GetPlayerFromSlot(warden_slot);

                player.PrintToChat($"{WARDEN_PREFIX}{warden.PlayerName} is allready a warden");
            }

            // player is valid to take warden
            else
            {
                set_warden(slot(player));
            }
        }

        // reset variables for a new round
        private void purge_round()
        {
            warden_slot = INAVLID_SLOT;

            // reset player structs
            foreach (JailPlayer jail_player in jail_players)
            {
                jail_player.purge_round();
            }
        }

        public void map_start()
        {
            warday.map_start();
        }

        private void set_warden_if_last()
        {
            // if there is only one ct automatically give them warden!
            var ct_players = get_alive_ct();

            if (ct_players.Count == 1)
            {
                int? slot2 = slot(ct_players[0]);

                set_warden(slot2);
            }
        }

        private void round_timer_callback()
        {
            start_timer = null;
        }

        public void round_start()
        {
            Server.ExecuteCommand("mp_force_pick_time 3000");

            purge_round();

            start_timer = instance.AddTimer(20.0F, round_timer_callback, CSTimer.TimerFlags.STOP_ON_MAPCHANGE);

            // handle submodules
            mute.round_start();
            block.round_start();
            warday.round_start();

            foreach (CCSPlayerController player in Utilities.GetPlayers())
            {
                set_colour(player, Color.FromArgb(255, 255, 255, 255));
            }

            set_warden_if_last();
        }

        public void round_end()
        {
            kill_timer(ref start_timer);
            mute.round_end();
            warday.round_end();
            purge_round();
        }

        public void connect(CCSPlayerController? player)
        {
            var slota = slot(player);

            if (slota != null)
            {
                jail_players[slota.Value].reset();
            }

            mute.connect(player);
        }

        public void disconnect(CCSPlayerController? player)
        {
            remove_if_warden(player);
        }

        public void setup_player_guns(CCSPlayerController? player)
        {
            if (player == null || !is_valid_alive(player))
            {
                return;
            }

            var jail_player = jail_player_from_player(player);

            if (jail_player != null)
            {
                strip_weapons(player);

                if (is_ct(player))
                {
                    if (config.ct_guns)
                    {
                        player.GiveNamedItem("weapon_deagle");
                        player.GiveNamedItem("weapon_m4a1");
                    }

                    if (config.ct_armour)
                    {
                        player.GiveNamedItem("item_assaultsuit");
                    }
                }
            }
        }

        public void voice(CCSPlayerController? player)
        {
            if (player == null || !is_valid_alive(player))
            {
                return;
            }

            if (!config.warden_on_voice)
            {
                return;
            }

            if (warden_slot == INAVLID_SLOT && is_ct(player))
            {
                set_warden(slot(player));
            }
        }

        public void spawn(CCSPlayerController? player)
        {
            if (player == null || !is_valid_alive(player))
            {
                return;
            }

            setup_player_guns(player);

            mute.spawn(player);
        }

        public void switch_team(CCSPlayerController? player, int new_team)
        {
            remove_if_warden(player);
            mute.switch_team(player, new_team);
        }

        // warden death has occured
        public void warden_death()
        {
            remove_warden();
        }

        public void death(CCSPlayerController? player, CCSPlayerController? killer)
        {
            // player is no longer on server
            if (!is_valid(player) || player == null)
            {
                return;
            }

            // handle warden death
            remove_if_warden(player);

            // mute player
            mute.death(player);

            var jail_player = jail_player_from_player(player);

            if (jail_player != null)
            {
                jail_player.rebel_death(player, killer);
            }

            // if a t dies we dont need to regive the warden
            if (is_ct(player))
            {
                set_warden_if_last();
            }
        }

        private static readonly String TEAM_PREFIX = $" {ChatColors.Green}[TEAM]: {ChatColors.White}";

        public bool join_team(CCSPlayerController? invoke, CommandInfo command)
        {
            if (invoke == null || !is_valid(invoke))
            {
                return true;
            }

            if (command.ArgCount != 3)
            {
                return true;
            }

            CCSPlayerPawn? pawn = ppawn(invoke);

            if (!Int32.TryParse(command.ArgByIndex(1), out int team))
            {
                return true;
            }

            JailPlayer? jail_player = jail_player_from_player(invoke);

            if (jail_player == null)
            {
                return false;
            }

            switch (team)
            {
                case TEAM_CT:
                    {
                        if (config.ct_swap_only)
                        {
                            JailbreakExtras.announce(invoke, TEAM_PREFIX, $"Sorry guards must be swapped to CT by admin");
                            play_sound(invoke, "sounds/ui/counter_beep.vsnd");
                            return false;
                        }

                        int ct_counta = ct_count();
                        int t_counta = t_count();

                        // check CT aint full
                        // i.e at a suitable raito or either team is empty
                        if ((ct_counta * config.bal_guards) > t_counta && ct_counta != 0 && t_counta != 0)
                        {
                            JailbreakExtras.announce(invoke, TEAM_PREFIX, $"Sorry, CT has too many players {config.bal_guards}:1 ratio maximum");
                            play_sound(invoke, "sounds/ui/counter_beep.vsnd");
                            return false;
                        }

                        return true;
                    }

                case TEAM_T:
                    {
                        return true;
                    }

                case TEAM_SPEC:
                    {
                        return true;
                    }

                default:
                    {
                        play_sound(invoke, "sounds/ui/counter_beep.vsnd");
                        return false;
                    }
            }
        }

        [RequiresPermissions("@css/generic")]
        public void swap_guard_cmd(CCSPlayerController? invoke, CommandInfo command)
        {
            if (invoke == null || !is_valid(invoke))
            {
                return;
            }

            if (command.ArgCount != 2)
            {
                invoke.PrintToChat("Expected usage: !swap_guard <player name>");
                return;
            }

            var target = command.GetArgTargetResult(1);

            foreach (CCSPlayerController player in target)
            {
                if (is_valid(player))
                {
                    invoke.PrintToChat($"swapped: {player.PlayerName}");
                    player.SwitchTeam(CsTeam.CounterTerrorist);
                }
            }
        }

        public void ct_guns(CCSPlayerController player, ChatMenuOption option)
        {
            if (player == null || !is_valid_alive(player) || !is_ct(player))
            {
                return;
            }

            strip_weapons(player);

            player.GiveNamedItem("weapon_" + gun_give_name(option.Text));
            player.GiveNamedItem("weapon_deagle");

            if (config.ct_armour)
            {
                player.GiveNamedItem("item_assaultsuit");
            }
        }

        public void cmd_ct_guns(CCSPlayerController? player, CommandInfo command)
        {
            if (player == null || !is_valid(player))
            {
                return;
            }

            if (!is_ct(player))
            {
                JailbreakExtras.announce(player, WARDEN_PREFIX, "You must be a ct to use the gun menu!");
                return;
            }

            if (!config.ct_gun_menu)
            {
                JailbreakExtras.announce(player, WARDEN_PREFIX, "Gun menu is disabled!");
                return;
            }

            gun_menu_internal(player, true, ct_guns);
        }

        public void player_hurt(CCSPlayerController? player, CCSPlayerController? attacker, int damage, int health)
        {
            var jail_player = jail_player_from_player(player);

            if (jail_player != null)
            {
                jail_player.player_hurt(player, attacker, damage, health);
            }
        }

        public void weapon_fire(CCSPlayerController? player, String name)
        {
            // attempt to set rebel
            var jail_player = jail_player_from_player(player);

            if (jail_player != null)
            {
                jail_player.rebel_weapon_fire(player, name);
            }
        }

        // util func to get a jail player
        private JailPlayer? jail_player_from_player(CCSPlayerController? player)
        {
            if (!is_valid(player) || player == null)
            {
                return null;
            }

            var slota = slot(player);

            if (slota == null)
            {
                return null;
            }

            return jail_players[slota.Value];
        }

        private const int INAVLID_SLOT = -3;

        private int warden_slot = INAVLID_SLOT;

        public static readonly String WARDEN_PREFIX = $" {ChatColors.Green}[WARDEN]: {ChatColors.White}";

        private CSTimer.Timer? start_timer = null;

        public JailConfig config = new JailConfig();

        private JailPlayer[] jail_players = new JailPlayer[64];

        public Warday warday = new Warday();
        public Block block = new Block();
        public Mute mute = new Mute();
    };
}