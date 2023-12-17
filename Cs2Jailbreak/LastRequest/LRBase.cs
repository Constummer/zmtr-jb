// base lr class
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CSTimer = CounterStrikeSharp.API.Modules.Timers;

// base LR impl
public abstract class LRBase
{
    private enum LrState
    {
        PENDING,
        ACTIVE,
    }

    protected LRBase(LastRequest lr_manager, LastRequest.LRType lr_type, int lr_slot, int actor_slot, String lr_choice)
    {
        state = LrState.PENDING;
        slot = lr_slot;
        player_slot = actor_slot;
        choice = lr_choice;
        lr_name = LastRequest.LR_NAME[(int)lr_type];
        type = lr_type;

        // while lr is pending damage is off
        restrict_damage = true;
        manager = lr_manager;

        // make sure we cant get guns during startup
        weapon_restrict = "knife";
    }

    public virtual void start()
    {
        var player = Utilities.GetPlayerFromSlot(player_slot);

        // player is not alive cancel the lr
        if (player == null || !player.is_valid_alive())
        {
            manager.end_lr(slot);
            return;
        }

        init_player(player);
    }

    public void cleanup()
    {
        // clean up timer
        Lib.kill_timer(ref timer);

        countdown.kill();

        // reset alive player
        CCSPlayerController? player = Utilities.GetPlayerFromSlot(player_slot);

        if (player == null || !player.is_valid_alive())
        {
            return;
        }

        // make sure our weapons dont get taken off
        weapon_restrict = "";

        // restore hp
        player.set_health(100);

        // restore weapons
        player.strip_weapons();

        // reset gravity
        player.set_gravity(1.0f);

        player.set_velocity(1.0f);

        if (player.is_ct())
        {
            player.GiveNamedItem("item_assaultsuit");
            player.GiveNamedItem("weapon_deagle");
            player.GiveNamedItem("weapon_m4a1");
        }
    }

    public void lose()
    {
        if (partner == null)
        {
            return;
        }

        CCSPlayerController? player = Utilities.GetPlayerFromSlot(player_slot);
        CCSPlayerController? winner = Utilities.GetPlayerFromSlot(partner.player_slot);

        if (player == null || !player.is_valid() || winner == null || !winner.is_valid())
        {
            manager.end_lr(slot);
            return;
        }

        manager.lr_stats.win(winner, type);
        manager.lr_stats.loss(player, type);

        manager.end_lr(slot);
    }

    // NOTE: this is called once for a pair on the starting slot
    public virtual void pair_activate()
    {
    }

    public void activate()
    {
        // this is a timer callback set it to null
        timer = null;

        // check this was built correctly
        // TODO: is there a static way to ensure this is made properly or no?
        if (partner == null)
        {
            manager.end_lr(slot);
            return;
        }

        CCSPlayerController? player = Utilities.GetPlayerFromSlot(player_slot);

        if (player != null && player.is_valid_alive())
        {
            player.announce(LastRequest.LR_PREFIX, "Fight!");
        }

        // renable damage
        // NOTE: start_lr can override this if it so pleases
        restrict_damage = false;

        start();

        state = LrState.ACTIVE;

        // make partner lr active if pending
        if (partner.state == LrState.PENDING)
        {
            partner.activate();
        }
    }

    // player setup -> NOTE: hp and gun stripping is done for us
    public abstract void init_player(CCSPlayerController player);

    // what events might we want access to?
    public virtual void weapon_fire(String name)
    { }

    public virtual void ent_created(CEntityInstance entity)
    { }

    public virtual bool take_damage()
    {
        return !restrict_damage;
    }

    public virtual void player_hurt(int health, int damage, int hitgroup)
    {
    }

    public virtual bool weapon_drop(String name)
    {
        return !restrict_drop;
    }

    public virtual bool weapon_equip(String name)
    {
        //Server.PrintToChatAll($"{name} : {weapon_restrict}");
        return weapon_restrict == "" || name.Contains(weapon_restrict);
    }

    public (CCSPlayerController? winner, CCSPlayerController? loser, LRBase? winner_lr) pick_rand_player()
    {
        Random rnd = new Random((int)DateTime.Now.Ticks);

        CCSPlayerController? winner = null;
        CCSPlayerController? loser = null;
        LRBase? winner_lr = null;

        if (rnd.Next(0, 2) == 0)
        {
            if (partner != null)
            {
                winner = Utilities.GetPlayerFromSlot(player_slot);
                loser = Utilities.GetPlayerFromSlot(partner.player_slot);
                winner_lr = this;
            }
        }
        else
        {
            if (partner != null)
            {
                winner = Utilities.GetPlayerFromSlot(partner.player_slot);
                loser = Utilities.GetPlayerFromSlot(player_slot);
                winner_lr = partner;
            }
        }

        return (winner, loser, winner_lr);
    }

    public static void print_countdown(LRBase lr, int delay)
    {
        if (lr.partner == null)
        {
            return;
        }

        CCSPlayerController? t_player = Utilities.GetPlayerFromSlot(lr.player_slot);
        CCSPlayerController? ct_player = Utilities.GetPlayerFromSlot(lr.partner.player_slot);

        if (t_player == null || !t_player.is_valid() || ct_player == null || !ct_player.is_valid())
        {
            return;
        }

        t_player.PrintToCenter($"Starting {lr.lr_name} against {ct_player.PlayerName} in {delay} seconds");
        ct_player.PrintToCenter($"Starting {lr.lr_name} against {t_player.PlayerName} in {delay} seconds");
    }

    public void countdown_start()
    {
        countdown.start(lr_name, 5, this, print_countdown, manager.activate_lr);
    }

    public virtual void ent_created(String name)
    { }

    public virtual void grenade_thrown()
    { }

    public virtual void weapon_zoom()
    { }

    public String lr_name = "";

    // player and lr info
    public readonly int player_slot;

    public readonly int slot;

    private LastRequest manager;

    // what weapon are we allowed to use?
    public String weapon_restrict = "";

    public bool restrict_damage = true;

    public bool restrict_drop = true;

    private LrState state;

    private LastRequest.LRType type;

    // who are we playing against, set up in create_pair
    public LRBase? partner;

    // custom choice
    protected String choice = "";

    // countdown for start
    public Countdown<LRBase> countdown = new Countdown<LRBase>();

    // managed timer
    private CSTimer.Timer? timer = null;
};