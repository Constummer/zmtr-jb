using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class TeamSave
    {
        public void save()
        {
            count = 0;

            // iter over each active player and save the theam they are on
            foreach (CCSPlayerController player in Utilities.GetPlayers())
            {
                int? player_slot = slot(player);

                if (!is_valid(player) || player_slot == null)
                {
                    continue;
                }

                int team = player.TeamNum;

                if (is_active_team(team))
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

                if (player == null || !is_valid(player))
                {
                    continue;
                }

                if (is_active_team(player.TeamNum))
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
}