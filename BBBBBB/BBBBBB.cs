using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace BBBB;

public class BBBB : BasePlugin
{
    public override string ModuleName => "BBBB";

    public override string ModuleVersion => "0.0.1";
    public override string ModuleAuthor => "Constummer";
    public override string ModuleDescription => "BBBB";

    public override void Load(bool hotReload)
    {
        RegisterEventHandler<EventPlayerSpawn>(OnPlayerSpawn);
    }

    private HookResult OnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
    {
        CCSPlayerController? player = @event.Userid;

        if (player != null && IsValid(player))
        {
            int? slot = Slot(player);

            AddTimer(0.5f, () =>
            {
                if (slot != null)
                {
                    Spawn(Utilities.GetPlayerFromSlot(slot.Value));
                }
            });
        }

        return HookResult.Continue;
    }

    public static int? ToSlot(int? user_id)
    {
        if (user_id == null)
        {
            return null;
        }

        return user_id & 0xff;
    }

    public static int? Slot(CCSPlayerController? player)
    {
        if (player == null)
        {
            return null;
        }

        return ToSlot(player.UserId);
    }

    public void Spawn(CCSPlayerController? player)
    {
        if (player == null || !IsValidAlive(player))
        {
            return;
        }

        SetupPlayerGuns(player);

        // mute.spawn(player);
    }

    public void SetupPlayerGuns(CCSPlayerController? player)
    {
        if (player == null || !IsValidAlive(player))
        {
            return;
        }

        // cvars take care of this for us now
        // player.strip_weapons();

        if (IsCt(player))
        {
            if (player == null || !IsValidAlive(player))
            {
                return;
            }
            // if(config.ct_guns)

            player.GiveNamedItem("weapon_deagle");
            if (player == null || !IsValidAlive(player))
            {
                return;
            }
            player.GiveNamedItem("weapon_m4a1");

            // if(config.ct_armour)
            if (player == null || !IsValidAlive(player))
            {
                return;
            }
            player.GiveNamedItem("item_assaultsuit");
        }
    }

    public const int TEAM_CT = 3;

    public static bool IsCt(CCSPlayerController? player) => player != null && IsValid(player) && player.TeamNum == TEAM_CT;

    public static bool IsValidAlive(CCSPlayerController? player)
    {
        return player != null && IsValid(player) && player.PawnIsAlive && GetHealth(player) > 0;
    }

    public static bool IsValid(CCSPlayerController? player)
    {
        return player != null && player.IsValid && player.PlayerPawn.IsValid;
    }

    public static int GetHealth(CCSPlayerController? player)
    {
        CCSPlayerPawn? pawn = Pawn(player);

        if (pawn == null)
        {
            return 100;
        }

        return pawn.Health;
    }

    public static CCSPlayerPawn? Pawn(CCSPlayerController? player)
    {
        if (player == null || !IsValid(player))
        {
            return null;
        }

        CCSPlayerPawn? pawn = player.PlayerPawn.Value;

        return pawn;
    }
}