using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

public class TeamSave
{
    public void save()
    {
        count = 0;

        // iter over each active player and save the theam they are on
        foreach (CCSPlayerController player in Utilities.GetPlayers())
        {
            int? player_slot = player.slot();

            if (!player.is_valid() || player_slot == null)
            {
                continue;
            }

            int team = player.TeamNum;

            if (Lib.is_active_team(team))
            {
                slots[count] = player_slot.Value;
                teams[count] = team;
                count++;
            }
        }
    }

    public void restore()
    {
        // iter over each player and switch to recorded team
        for (int i = 0; i < count; i++)
        {
            CCSPlayerController? player = Utilities.GetPlayerFromSlot(slots[i]);

            if (player == null || !player.is_valid())
            {
                continue;
            }

            if (Lib.is_active_team(player.TeamNum))
            {
                player.SwitchTeam((CsTeam)teams[i]);
            }
        }

        count = 0;
    }

    private int[] slots = new int[64];
    private int[] teams = new int[64];

    private int count = 0;
};