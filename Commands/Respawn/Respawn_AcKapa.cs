using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static bool RespawnAcActive = false;

    #region Respawn

    [ConsoleCommand("respawnac")]
    public void RespawnAc(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye27", "@css/seviye27") == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        GetPlayers()
            .Where(x => x.PawnIsAlive == false)
            .ToList()
            .ForEach(x =>
            {
                CustomRespawn(x);
            });
        RespawnAcAction();
    }

    [ConsoleCommand("respawnkapa")]
    [ConsoleCommand("respawnkapat")]
    public void RespawnKapat(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye27", "@css/seviye27") == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        RespawnKapatAction();
    }

    private static void RespawnAcAction()
    {
        Server.ExecuteCommand("mp_respawn_on_death_ct 1");
        Server.ExecuteCommand("mp_respawn_on_death_t 1");
        Server.ExecuteCommand("mp_ignore_round_win_conditions 1");
        Server.PrintToChatAll($"{Prefix} {CC.W}Respawn açıldı.");
        RespawnAcActive = true;
    }

    private static void RespawnKapatAction()
    {
        Server.ExecuteCommand("mp_respawn_on_death_ct 0");
        Server.ExecuteCommand("mp_respawn_on_death_t 0");
        Server.ExecuteCommand("mp_ignore_round_win_conditions 0");
        Server.PrintToChatAll($"{Prefix} {CC.W}Respawn kapandı.");
        RespawnAcActive = false;
    }

    #endregion Respawn
}