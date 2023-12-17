using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using CounterStrikeSharp.API.Modules.Utils;
using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class JailConfig : BasePluginConfig
    {
        [JsonPropertyName("username")]
        public String username { get; set; } = "";

        [JsonPropertyName("password")]
        public String password { get; set; } = "";

        [JsonPropertyName("server")]
        public String server { get; set; } = "127.0.0.1";

        [JsonPropertyName("port")]
        public String port { get; set; } = "3306";

        [JsonPropertyName("database")]
        public String database { get; set; } = "cs2_jail";

        [JsonPropertyName("ct_voice_only")]
        public bool ct_voice_only { get; set; } = false;

        [JsonPropertyName("mute_t_allways")]
        public bool mute_t_allways { get; set; } = false;

        [JsonPropertyName("warden_on_voice")]
        public bool warden_on_voice { get; set; } = true;

        [JsonPropertyName("ct_swap_only")]
        public bool ct_swap_only { get; set; } = false;

        [JsonPropertyName("ct_guns")]
        public bool ct_guns { get; set; } = true;

        [JsonPropertyName("ct_gun_menu")]
        public bool ct_gun_menu { get; set; } = true;

        [JsonPropertyName("ct_armour")]
        public bool ct_armour { get; set; } = true;

        // ratio of t to CT
        [JsonPropertyName("bal_guards")]
        public int bal_guards { get; set; } = 0;

        [JsonPropertyName("enable_riot")]
        public bool riot_enable { get; set; } = false;

        [JsonPropertyName("hide_kills")]
        public bool hide_kills { get; set; } = false;

        [JsonPropertyName("lr_knife")]
        public bool lr_knife { get; set; } = true;

        [JsonPropertyName("lr_gun_toss")]
        public bool lr_gun_toss { get; set; } = true;

        [JsonPropertyName("lr_dodgeball")]
        public bool lr_dodgeball { get; set; } = true;

        [JsonPropertyName("lr_no_scope")]
        public bool lr_no_scope { get; set; } = true;

        [JsonPropertyName("lr_shotgun_war")]
        public bool lr_shotgun_war { get; set; } = true;

        [JsonPropertyName("lr_grenade")]
        public bool lr_grenade { get; set; } = true;

        [JsonPropertyName("lr_russian_roulette")]
        public bool lr_russian_roulette { get; set; } = true;

        [JsonPropertyName("lr_scout_knife")]
        public bool lr_scout_knife { get; set; } = true;

        [JsonPropertyName("lr_headshot_only")]
        public bool lr_headshot_only { get; set; } = true;

        [JsonPropertyName("lr_shot_for_shot")]
        public bool lr_shot_for_shot { get; set; } = true;

        [JsonPropertyName("lr_mag_for_mag")]
        public bool lr_mag_for_mag { get; set; } = true;

        [JsonPropertyName("lr_count")]
        public uint lr_count { get; set; } = 2;
    }

    // main plugin file, controls central hooking
    // defers to warden, lr and sd
    public class JailPlugin
    {
        // workaround to query global state!
        public static JailPlugin? global_ctx;

        public static JailbreakExtras global_extras;

        public static JailConfig Config { get; set; } = new JailConfig();

        public static Warden warden = new Warden();
        public static LastRequest lr = new LastRequest();
        public static SpecialDay sd = new SpecialDay();

        // Global event settings, used to filter plugin activits
        // during warday and SD
        internal bool is_event_active = false;

        public static bool is_warden(CCSPlayerController? player)
        {
            if (global_ctx == null)
            {
                return false;
            }

            return warden.is_warden(player);
        }

        public static bool event_active()
        {
            if (global_ctx == null)
            {
                return false;
            }

            return global_ctx.is_event_active;
        }

        public static void start_event()
        {
            if (global_ctx != null)
            {
                global_ctx.is_event_active = true;
            }
        }

        public static void end_event()
        {
            if (global_ctx != null)
            {
                global_ctx.is_event_active = false;
            }
        }

        internal void register_listener()
        {
            global_extras.RegisterListener<Listeners.OnEntitySpawned>(entity =>
            {
                lr.ent_created(entity);
                sd.ent_created(entity);
            });
        }

        internal void register_commands()
        {
            // reg warden comamnds
            global_extras.AddCommand("w", "take warden", warden.take_warden_cmd);
            global_extras.AddCommand("uw", "leave warden", warden.leave_warden_cmd);
            global_extras.AddCommand("rw", "remove warden", warden.remove_warden_cmd);

            global_extras.AddCommand("wub", "warden : disable block", warden.wub_cmd);
            global_extras.AddCommand("wb", "warden : enable block", warden.wb_cmd);

            global_extras.AddCommand("swap_guard", "admin : move a player to ct", warden.swap_guard_cmd);

            global_extras.AddCommand("wd", "warden : start warday", warden.warday_cmd);
            global_extras.AddCommand("wcommands", "warden : show all commands", warden.cmd_info);

            global_extras.AddCommand("guns", "give ct guns", warden.cmd_ct_guns);

            // reg lr commands
            global_extras.AddCommand("lr", "start an lr", lr.lr_cmd);
            global_extras.AddCommand("cancel_lr", "admin : cancel lr", lr.cancel_lr_cmd);
            global_extras.AddCommand("lr_stats", "list lr stats", lr.lr_stats.lr_stats_cmd);

            // reg sd commands
            global_extras.AddCommand("sd", "start a sd", sd.sd_cmd);
            global_extras.AddCommand("sd_ff", "start a ff sd", sd.sd_ff_cmd);
            global_extras.AddCommand("cancel_sd", "cancel an sd", sd.cancel_sd_cmd);

            global_extras.AddCommandListener("jointeam", join_team);

            // debug
            if (Debug.enable)
            {
                global_extras.AddCommand("nuke", "debug : kill every player", Debug.nuke);
                global_extras.AddCommand("force_open", "debug : force open every door and vent", Debug.force_open_cmd);
                global_extras.AddCommand("is_rebel", "debug : print rebel state to console", warden.is_rebel_cmd);
                global_extras.AddCommand("lr_debug", "debug : start an lr without restriction", lr.lr_debug_cmd);
                global_extras.AddCommand("is_blocked", "debug : print block state", warden.block.is_blocked);
                global_extras.AddCommand("test_laser", "test laser", Debug.test_laser);
                global_extras.AddCommand("test_strip", "test weapon strip", Debug.test_strip_cmd);
                global_extras.AddCommand("join_ct_debug", "debug : force join ct", Debug.join_ct_cmd);
                global_extras.AddCommand("hide_weapon_debug", "debug : hide player weapon on back", Debug.hide_weapon_cmd);
                global_extras.AddCommand("rig", "debug : force player to boss on sd", sd.sd_rig_cmd);
                global_extras.AddCommand("is_muted", "debug : print voice flags", Debug.is_muted_cmd);
            }
        }

        public HookResult join_team(CCSPlayerController? invoke, CommandInfo command)
        {
            lr.lr_stats.connect(invoke);

            if (!warden.join_team(invoke, command))
            {
                return HookResult.Stop;
            }

            return HookResult.Continue;
        }

        internal void register_hook()
        {
            global_extras.RegisterEventHandler<EventRoundStart>(OnRoundStart);
            global_extras.RegisterEventHandler<EventRoundEnd>(OnRoundEnd, HookMode.Pre);
            global_extras.RegisterEventHandler<EventWeaponFire>(OnWeaponFire);
            global_extras.RegisterEventHandler<EventPlayerSpawn>(OnPlayerSpawn);
            global_extras.RegisterEventHandler<EventPlayerDisconnect>(OnPlayerDisconnect);
            global_extras.RegisterEventHandler<EventPlayerConnect>(OnPlayerConnect);
            global_extras.RegisterEventHandler<EventTeamchangePending>(OnSwitchTeam);
            global_extras.RegisterEventHandler<EventMapTransition>(OnMapChange);
            global_extras.RegisterEventHandler<EventPlayerDeath>(OnPlayerDeath, HookMode.Pre);
            global_extras.RegisterEventHandler<EventItemEquip>(OnItemEquip);
            global_extras.RegisterEventHandler<EventGrenadeThrown>(OnGrenadeThrown);
            global_extras.RegisterEventHandler<EventPlayerHurt>(OnPlayerHurt);
            global_extras.RegisterEventHandler<EventWeaponZoom>(OnWeaponZoom);
            VirtualFunctions.CBaseEntity_TakeDamageOldFunc.Hook(OnTakeDamage, HookMode.Pre);

            //HookEntityOutput("func_button", "OnPressed", OnButtonPressed);

            //RegisterListener<Listeners.OnClientVoice>(OnClientVoice);

            // TODO: need to hook weapon drop
        }

        internal void OnClientVoice(int slot)
        {
            CCSPlayerController? player = Utilities.GetPlayerFromSlot(slot);

            if (player != null && player.is_valid())
            {
                warden.voice(player);
            }
        }

        // button log
        internal HookResult OnButtonPressed(CEntityIOOutput output, String name, CEntityInstance activator, CEntityInstance caller, CVariant value, float delay)
        {
            CCSPlayerController? player = activator.player();

            // grab player controller from pawn
            CBaseEntity? ent = Utilities.GetEntityFromIndex<CBaseEntity>((int)caller.Index);

            if (player != null && player.is_valid() && ent != null && ent.IsValid)
            {
                Lib.print_console_all($"{player.PlayerName} pressed button '{ent.Entity?.Name}' -> '{output?.Connections?.TargetDesc}'", true);
            }

            return HookResult.Continue;
        }

        internal HookResult OnGrenadeThrown(EventGrenadeThrown @event, GameEventInfo info)
        {
            CCSPlayerController? player = @event.Userid;

            if (player != null && player.is_valid())
            {
                lr.grenade_thrown(player);
                sd.grenade_thrown(player);
            }

            return HookResult.Continue;
        }

        internal HookResult OnWeaponZoom(EventWeaponZoom @event, GameEventInfo info)
        {
            CCSPlayerController? player = @event.Userid;

            if (player != null && player.is_valid())
            {
                lr.weapon_zoom(player);
            }

            return HookResult.Continue;
        }

        internal HookResult OnItemEquip(EventItemEquip @event, GameEventInfo info)
        {
            CCSPlayerController? player = @event.Userid;

            if (player != null && player.is_valid())
            {
                lr.weapon_equip(player, @event.Item);
                sd.weapon_equip(player, @event.Item);
            }

            return HookResult.Continue;
        }

        internal HookResult OnPlayerHurt(EventPlayerHurt @event, GameEventInfo info)
        {
            CCSPlayerController? player = @event.Userid;
            CCSPlayerController? attacker = @event.Attacker;

            int damage = @event.DmgHealth;
            int health = @event.Health;
            int hitgroup = @event.Hitgroup;

            if (player != null && player.is_valid())
            {
                lr.player_hurt(player, attacker, damage, health, hitgroup);
                warden.player_hurt(player, attacker, damage, health);
                sd.player_hurt(player, attacker, damage, health, hitgroup);
            }

            return HookResult.Continue;
        }

        internal HookResult OnTakeDamage(DynamicHook handle)
        {
            CEntityInstance victim = handle.GetParam<CEntityInstance>(0);
            CTakeDamageInfo damage_info = handle.GetParam<CTakeDamageInfo>(1);

            CHandle<CBaseEntity> dealer = damage_info.Attacker;

            // get player and attacker
            CCSPlayerController? player = victim.player();
            CCSPlayerController? attacker = dealer.player();

            sd.take_damage(player, attacker, ref damage_info.Damage);
            lr.take_damage(player, attacker, ref damage_info.Damage);

            return HookResult.Continue;
        }

        internal HookResult OnMapChange(EventMapTransition @event, GameEventInfo info)
        {
            warden.map_start();

            return HookResult.Continue;
        }

        internal HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
        {
            warden.round_start();
            lr.round_start();
            sd.round_start();

            return HookResult.Continue;
        }

        internal HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo info)
        {
            CCSPlayerController? victim = @event.Userid;
            CCSPlayerController? killer = @event.Attacker;

            // hide t killing ct
            if (Config.hide_kills && killer.is_t() && victim.is_ct())
            {
                //@event.Attacker = player;
                // fire event as is to T
                foreach (CCSPlayerController? player in Utilities.GetPlayers())
                {
                    if (player != null && player.is_valid())
                    {
                        if (player.is_t())
                        {
                            // T gets full event
                            @event.Userid = victim;
                            @event.Attacker = killer;

                            @event.FireEventToClient(player);
                        }
                        else
                        {
                            // ct gets a suicide
                            @event.Userid = victim;
                            @event.Attacker = victim;

                            @event.FireEventToClient(player);
                        }
                    }
                }

                info.DontBroadcast = true;
            }

            if (victim != null && victim.is_valid())
            {
                warden.death(victim, killer);
                lr.death(victim);
                sd.death(victim, killer);
            }

            return HookResult.Continue;
        }

        internal HookResult OnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
        {
            CCSPlayerController? player = @event.Userid;

            if (player != null && player.is_valid())
            {
                int? slot = player.slot();

                global_extras.AddTimer(0.5f, () =>
                {
                    if (slot != null)
                    {
                        warden.spawn(Utilities.GetPlayerFromSlot(slot.Value));
                    }
                });
            }

            return HookResult.Continue;
        }

        internal HookResult OnSwitchTeam(EventTeamchangePending @event, GameEventInfo info)
        {
            CCSPlayerController? player = @event.Userid;

            int new_team = @event.Toteam;

            if (player != null && player.is_valid())
            {
                warden.switch_team(player, new_team);
            }

            return HookResult.Continue;
        }

        internal HookResult OnPlayerConnect(EventPlayerConnect @event, GameEventInfo info)
        {
            CCSPlayerController? player = @event.Userid;

            if (player != null && player.is_valid())
            {
                lr.lr_stats.connect(player);
            }

            return HookResult.Continue;
        }

        internal HookResult OnPlayerDisconnect(EventPlayerDisconnect @event, GameEventInfo info)
        {
            CCSPlayerController? player = @event.Userid;

            if (player != null && player.is_valid())
            {
                warden.disconnect(player);
                lr.disconnect(player);
                sd.disconnect(player);
            }

            return HookResult.Continue;
        }

        internal HookResult OnRoundEnd(EventRoundEnd @event, GameEventInfo info)
        {
            warden.round_end();
            lr.round_end();
            sd.round_end();

            return HookResult.Continue;
        }

        internal HookResult OnWeaponFire(EventWeaponFire @event, GameEventInfo info)
        {
            // attempt to get player and weapon
            var player = @event.Userid;
            String name = @event.Weapon;

            warden.weapon_fire(player, name);
            lr.weapon_fire(player, name);

            return HookResult.Continue;
        }
    }
}