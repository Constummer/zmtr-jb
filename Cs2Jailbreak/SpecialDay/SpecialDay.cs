using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public enum SDState
    {
        INACTIVE,
        STARTED,
        ACTIVE
    };

    // TODO: this will be done after lr and warden
    // are in a decent state as its just an extra, and should
    // not take too long to port from css
    public class SpecialDay
    {
        public void end_sd(bool forced = false)
        {
            if (active_sd != null)
            {
                JailPlugin.end_event();
                active_sd.end_common();
                active_sd = null;

                countdown.kill();

                // restore all players if from a cancel
                if (forced)
                {
                    Lib.announce(SPECIALDAY_PREFIX, "Special day cancelled");
                }

                team_save.restore();
            }
        }

        public void round_end()
        {
            end_sd();
        }

        public void round_start()
        {
            end_sd();
        }

        public void setup_sd(CCSPlayerController? invoke, ChatMenuOption option)
        {
            if (invoke == null || !invoke.is_valid())
            {
                return;
            }

            if (active_sd != null)
            {
                invoke.announce(SPECIALDAY_PREFIX, "You cannot call two SD's at once");
                return;
            }

            String name = option.Text;

            switch (name)
            {
                case "Friendly fire":
                    {
                        active_sd = new SDFriendlyFire();
                        type = SDType.FREIENDLY_FIRE;
                        break;
                    }

                case "Juggernaut":
                    {
                        active_sd = new SDJuggernaut();
                        type = SDType.JUGGERNAUT;
                        break;
                    }

                case "Tank":
                    {
                        active_sd = new SDTank();
                        type = SDType.TANK;
                        break;
                    }

                case "Scout knife":
                    {
                        active_sd = new SDScoutKnife();
                        type = SDType.SCOUT_KNIFE;
                        break;
                    }

                case "Headshot only":
                    {
                        active_sd = new SDHeadshotOnly();
                        type = SDType.HEADSHOT_ONLY;
                        break;
                    }

                case "Knife warday":
                    {
                        active_sd = new SDKnifeWarday();
                        type = SDType.KNIFE_WARDAY;
                        break;
                    }

                case "Hide and seek":
                    {
                        active_sd = new SDHideAndSeek();
                        type = SDType.HIDE_AND_SEEK;
                        break;
                    }

                case "Dodgeball":
                    {
                        active_sd = new SDDodgeball();
                        type = SDType.DODGEBALL;
                        break;
                    }

                case "Spectre":
                    {
                        active_sd = new SDSpectre();
                        type = SDType.SPECTRE;
                        break;
                    }

                case "Grenade":
                    {
                        active_sd = new SDGrenade();
                        type = SDType.GRENADE;
                        break;
                    }
            }

            // 1up all dead players
            foreach (CCSPlayerController player in Utilities.GetPlayers())
            {
                if (player.is_valid() && !player.is_valid_alive())
                {
                    player.Respawn();
                }
            }

            // call the intiail sd setup
            if (active_sd != null)
            {
                JailPlugin.start_event();
                active_sd.setup_common();
            }

            // start the countdown for enable
            if (JailPlugin.global_ctx != null)
            {
                countdown.start($"{name} specialday", 15, 0, null, start_sd);
            }

            team_save.save();
        }

        public void weapon_equip(CCSPlayerController? player, String name)
        {
            if (player == null || !player.is_valid_alive())
            {
                return;
            }

            if (active_sd != null)
            {
                // weapon equip not valid drop the weapons
                if (!active_sd.weapon_equip(player, name))
                {
                    active_sd.setup_player(player);
                }
            }
        }

        public void disconnect(CCSPlayerController? player)
        {
            if (player == null || !player.is_valid())
            {
                return;
            }

            if (active_sd != null)
            {
                active_sd.disconnect(player);
            }
        }

        public void grenade_thrown(CCSPlayerController? player)
        {
            if (active_sd != null)
            {
                active_sd.grenade_thrown(player);
            }
        }

        public void ent_created(CEntityInstance entity)
        {
            if (active_sd != null)
            {
                active_sd.ent_created(entity);
            }
        }

        public void death(CCSPlayerController? player, CCSPlayerController? attacker)
        {
            if (active_sd != null)
            {
                active_sd.death(player, attacker);
            }
        }

        public void player_hurt(CCSPlayerController? player, CCSPlayerController? attacker, int damage, int health, int hitgroup)
        {
            if (active_sd != null && player != null && player.is_valid())
            {
                active_sd.player_hurt(player, damage, health, hitgroup);
            }
        }

        public void start_sd(int unused)
        {
            if (active_sd != null)
            {
                // force ff active
                if (override_ff)
                {
                    Lib.announce(SPECIALDAY_PREFIX, "Friendly fire enabled!");
                    Lib.enable_friendly_fire();
                }

                active_sd.start_common();
            }
        }

        public void take_damage(CCSPlayerController? player, CCSPlayerController? attacker, ref float damage)
        {
            if (active_sd == null || player == null || !player.is_valid())
            {
                return;
            }

            if (active_sd.restrict_damage)
            {
                damage = 0.0f;
            }
        }

        [RequiresPermissions("@css/generic")]
        public void cancel_sd_cmd(CCSPlayerController? player, CommandInfo command)
        {
            end_sd(true);
        }

        public void sd_cmd_internal(CCSPlayerController? player)
        {
            if (player == null || !player.is_valid())
            {
                return;
            }

            var sd_menu = new ChatMenu("SD Menu");

            // Build the basic LR menu
            for (int s = 0; s < SD_NAME.Length - 1; s++)
            {
                sd_menu.AddMenuOption(SD_NAME[s], setup_sd);
            }

            ChatMenus.OpenMenu(player, sd_menu);
        }

        [RequiresPermissions("@jail/debug")]
        public void sd_rig_cmd(CCSPlayerController? player, CommandInfo command)
        {
            if (player == null || !player.is_valid())
            {
                return;
            }

            if (active_sd != null && active_sd.state == SDState.STARTED)
            {
                player.PrintToChat($"Rigged sd boss to {player.PlayerName}");
                active_sd.rigged = player;
            }
        }

        [RequiresPermissions("@css/generic")]
        public void sd_cmd(CCSPlayerController? player, CommandInfo command)
        {
            override_ff = false;

            sd_cmd_internal(player);
        }

        [RequiresPermissions("@css/generic")]
        public void sd_ff_cmd(CCSPlayerController? player, CommandInfo command)
        {
            override_ff = true;

            sd_cmd_internal(player);
        }

        public enum SDType
        {
            FREIENDLY_FIRE,
            JUGGERNAUT,
            TANK,
            SPECTRE,
            DODGEBALL,
            GRENADE,
            SCOUT_KNIFE,
            HIDE_AND_SEEK,
            HEADSHOT_ONLY,
            KNIFE_WARDAY,
            NONE
        };

        public static readonly String SPECIALDAY_PREFIX = $"  {ChatColors.Green}[Special day]: {ChatColors.White}";

        private static String[] SD_NAME = {
        "Friendly fire",
        "Juggernaut",
        "Tank",
        "Spectre",
        "Dodgeball",
        "Grenade",
        "Scout knife",
        "Hide and seek",
        "Headshot only",
        "Knife warday",
        "None"
    };

        private SDBase? active_sd = null;

        private bool override_ff = false;

        private Countdown<int> countdown = new Countdown<int>();

        private SDType type = SDType.NONE;

        private TeamSave team_save = new TeamSave();
    };
}