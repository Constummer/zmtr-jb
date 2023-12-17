using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

public class Warday
{
    private void gun_callback(int unused)
    {
        // if warday is no longer active dont allow guns

        if (warday_active)
        {
            // give T guns
            foreach (CCSPlayerController player in Utilities.GetPlayers())
            {
                if (player.is_valid() && player.TeamNum == Lib.TEAM_T)
                {
                    player.event_gun_menu();
                }
            }

            Lib.announce(WARDAY_PREFIX, "Weapons live!");
        }
    }

    public bool start_warday(String location)
    {
        if (round_counter >= ROUND_LIMIT)
        {
            // must wait again to start a warday
            round_counter = 0;

            warday_active = true;
            JailPlugin.start_event();

            foreach (CCSPlayerController player in Utilities.GetPlayers())
            {
                if (player.is_valid() && player.TeamNum == Lib.TEAM_CT)
                {
                    player.event_gun_menu();
                }
            }

            countdown.start($"warday at {location}", 20, 0, null, gun_callback);
            return true;
        }

        return false;
    }

    public void round_end()
    {
        countdown.kill();
    }

    public void round_start()
    {
        // one less round till a warday can be called
        round_counter++;

        countdown.kill();

        warday_active = false;
        JailPlugin.end_event();
    }

    public void map_start()
    {
        // give a warday on map start
        round_counter = ROUND_LIMIT;
    }

    private String WARDAY_PREFIX = $" {ChatColors.Green} [Warday]: {ChatColors.White}";

    private bool warday_active = false;

    public int round_counter = ROUND_LIMIT;

    public const int ROUND_LIMIT = 3;

    private Countdown<int> countdown = new Countdown<int>();
};