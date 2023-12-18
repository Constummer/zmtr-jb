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

    private static Warden warden = new Warden();
    private static LastRequest lr = new LastRequest();
    private static SpecialDay sd = new SpecialDay();
    private static bool is_event_active = false;

    // main plugin file, controls central hooking
    // defers to warden, lr and sd

    // workaround to query global state!

    // Global event settings, used to filter plugin activits
    // during warday and SD

    internal void OnClientVoice(int slot)
    {
        CCSPlayerController? player = Utilities.GetPlayerFromSlot(slot);

        if (player != null && is_valid(player))
        {
            warden.voice(player);
        }
    }

    // button log
    internal HookResult OnButtonPressed(CEntityIOOutput output, String name, CEntityInstance activator, CEntityInstance caller, CVariant value, float delay)
    {
        CCSPlayerController? playera = player(activator);

        // grab player controller from pawn
        CBaseEntity? ent = Utilities.GetEntityFromIndex<CBaseEntity>((int)caller.Index);

        if (playera != null && is_valid(playera) && ent != null && ent.IsValid)
        {
            print_console_all($"{playera.PlayerName} pressed button '{ent.Entity?.Name}' -> '{output?.Connections?.TargetDesc}'", true);
        }

        return HookResult.Continue;
    }

    internal HookResult OnGrenadeThrown(EventGrenadeThrown @event, GameEventInfo info)
    {
        CCSPlayerController? player = @event.Userid;

        if (player != null && is_valid(player))
        {
            lr.grenade_thrown(player);
            sd.grenade_thrown(player);
        }

        return HookResult.Continue;
    }

    internal HookResult OnWeaponZoom(EventWeaponZoom @event, GameEventInfo info)
    {
        CCSPlayerController? player = @event.Userid;

        if (player != null && is_valid(player))
        {
            lr.weapon_zoom(player);
        }

        return HookResult.Continue;
    }

    internal HookResult OnItemEquip(EventItemEquip @event, GameEventInfo info)
    {
        CCSPlayerController? player = @event.Userid;

        if (player != null && is_valid(player))
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

        if (player != null && is_valid(player))
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
        CCSPlayerController? playerp = player(victim);
        CCSPlayerController? attacker = player(dealer);

        sd.take_damage(playerp, attacker, ref damage_info.Damage);
        lr.take_damage(playerp, attacker, ref damage_info.Damage);

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
        if (Config.hide_kills && is_t(killer) && is_ct(victim))
        {
            //@event.Attacker = player;
            // fire event as is to T
            foreach (CCSPlayerController? player in Utilities.GetPlayers())
            {
                if (player != null && is_valid(player))
                {
                    if (is_t(player))
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

        if (victim != null && is_valid(victim))
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

        if (player != null && is_valid(player))
        {
            int? slota = slot(player);

            AddTimer(0.5f, () =>
            {
                if (slota != null)
                {
                    warden.spawn(Utilities.GetPlayerFromSlot(slota.Value));
                }
            });
        }

        return HookResult.Continue;
    }

    internal HookResult OnSwitchTeam(EventTeamchangePending @event, GameEventInfo info)
    {
        CCSPlayerController? player = @event.Userid;

        int new_team = @event.Toteam;

        if (player != null && is_valid(player))
        {
            warden.switch_team(player, new_team);
        }

        return HookResult.Continue;
    }

    internal HookResult OnPlayerConnect(EventPlayerConnect @event, GameEventInfo info)
    {
        CCSPlayerController? player = @event.Userid;

        if (player != null && is_valid(player))
        {
            lr.lr_stats.connect(player);
        }

        return HookResult.Continue;
    }

    internal HookResult OnPlayerDisconnect(EventPlayerDisconnect @event, GameEventInfo info)
    {
        CCSPlayerController? player = @event.Userid;

        if (player != null && is_valid(player))
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

    internal void register_listener()
    {
        RegisterListener<Listeners.OnEntitySpawned>(entity =>
        {
            lr.ent_created(entity);
            sd.ent_created(entity);
        });
    }

    internal void register_commands()
    {
        // reg warden comamnds
        AddCommand("w", "take warden", warden.take_warden_cmd);
        AddCommand("uw", "leave warden", warden.leave_warden_cmd);
        AddCommand("rw", "remove warden", warden.remove_warden_cmd);

        AddCommand("wub", "warden : disable block", warden.wub_cmd);
        AddCommand("wb", "warden : enable block", warden.wb_cmd);

        AddCommand("swap_guard", "admin : move a player to ct", warden.swap_guard_cmd);

        AddCommand("wd", "warden : start warday", warden.warday_cmd);
        AddCommand("wcommands", "warden : show all commands", warden.cmd_info);

        AddCommand("guns", "give ct guns", warden.cmd_ct_guns);

        // reg lr commands
        AddCommand("lr", "start an lr", lr.lr_cmd);
        AddCommand("cancel_lr", "admin : cancel lr", lr.cancel_lr_cmd);
        AddCommand("lr_stats", "list lr stats", lr.lr_stats.lr_stats_cmd);

        // reg sd commands
        AddCommand("sd", "start a sd", sd.sd_cmd);
        AddCommand("sd_ff", "start a ff sd", sd.sd_ff_cmd);
        AddCommand("cancel_sd", "cancel an sd", sd.cancel_sd_cmd);

        AddCommandListener("jointeam", join_team);

        // debug
        if (Debug.enable)
        {
            AddCommand("nuke", "debug : kill every player", Debug.nuke);
            AddCommand("force_open", "debug : force open every door and vent", Debug.force_open_cmd);
            AddCommand("is_rebel", "debug : print rebel state to console", warden.is_rebel_cmd);
            AddCommand("lr_debug", "debug : start an lr without restriction", lr.lr_debug_cmd);
            AddCommand("is_blocked", "debug : print block state", warden.block.is_blocked);
            AddCommand("test_laser", "test laser", Debug.test_laser);
            AddCommand("test_strip", "test weapon strip", Debug.test_strip_cmd);
            AddCommand("join_ct_debug", "debug : force join ct", Debug.join_ct_cmd);
            AddCommand("hide_weapon_debug", "debug : hide player weapon on back", Debug.hide_weapon_cmd);
            AddCommand("rig", "debug : force player to boss on sd", sd.sd_rig_cmd);
            AddCommand("is_muted", "debug : print voice flags", Debug.is_muted_cmd);
        }
    }

    internal void register_hook()
    {
        RegisterEventHandler<EventRoundStart>(OnRoundStart);
        RegisterEventHandler<EventRoundEnd>(OnRoundEnd, HookMode.Pre);
        RegisterEventHandler<EventWeaponFire>(OnWeaponFire);
        RegisterEventHandler<EventPlayerSpawn>(OnPlayerSpawn);
        RegisterEventHandler<EventPlayerDisconnect>(OnPlayerDisconnect);
        RegisterEventHandler<EventPlayerConnect>(OnPlayerConnect);
        RegisterEventHandler<EventTeamchangePending>(OnSwitchTeam);
        RegisterEventHandler<EventMapTransition>(OnMapChange);
        RegisterEventHandler<EventPlayerDeath>(OnPlayerDeath, HookMode.Pre);
        RegisterEventHandler<EventItemEquip>(OnItemEquip);
        RegisterEventHandler<EventGrenadeThrown>(OnGrenadeThrown);
        RegisterEventHandler<EventPlayerHurt>(OnPlayerHurt);
        RegisterEventHandler<EventWeaponZoom>(OnWeaponZoom);
        VirtualFunctions.CBaseEntity_TakeDamageOldFunc.Hook(OnTakeDamage, HookMode.Pre);

        //HookEntityOutput("func_button", "OnPressed", OnButtonPressed);

        //RegisterListener<Listeners.OnClientVoice>(OnClientVoice);

        // TODO: need to hook weapon drop
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

    public static bool is_warden(CCSPlayerController? player)
    {
        return warden.is_warden(player);
    }

    public static bool event_active()
    {
        return is_event_active;
    }

    public static void start_event()
    {
        is_event_active = true;
    }

    public static void end_event()
    {
        is_event_active = false;
    }
}