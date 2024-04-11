using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("TelliSefer")]
    [ConsoleCommand("TelliogluSeferoglu")]
    [ConsoleCommand("TelliogluSeferogluBasla")]
    [ConsoleCommand("TelliogluVsSeferoglu")]
    [ConsoleCommand("TelliogluVsSeferogluBasla")]
    public void TelliogluSeferoglu(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        PatronuKoruActive = !PatronuKoruActive;
        switch (PatronuKoruActive)
        {
            case true:
                Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.Go} TELLİ OĞULLARI VS SEFER OĞULLARI{CC.W} etkinliğini başlattı");
                break;

            case false:
                Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.Go} TELLİ OĞULLARI VS SEFER OĞULLARI{CC.W} etkinliğini bitirdi");
                break;
        }
    }

    [ConsoleCommand("TelliogluVsSeferogluMapAc")]
    public void TelliogluVsSeferogluMapAc(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        Server.ExecuteCommand($"host_workshop_map 3178317288");
    }

    private static void TelliogluVsSeferogluRoundStart()
    {
        HookDisabled = true;
        Server.ExecuteCommand($"sv_enablebunnyhopping 0;sv_autobunnyhopping 0");
        Server.ExecuteCommand("mp_autoteambalance 1");

        Server.ExecuteCommand("mp_maxrounds 30");
        Server.ExecuteCommand("mp_halftime 1");
        Server.ExecuteCommand("mp_freezetime 5");
        Server.ExecuteCommand("mp_free_armor 1");
        Server.ExecuteCommand("mp_roundtime 3");
        Server.ExecuteCommand("sv_alltalk 1");
        Server.ExecuteCommand("sv_deadtalk 1");
        Server.ExecuteCommand("mp_forcecamera 0");
        Server.ExecuteCommand("sv_voiceenable 1");
        Model0Action();
    }
}