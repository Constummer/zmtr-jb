using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static bool TelliSeferActive { get; set; } = false;

    [ConsoleCommand("TelliSefer")]
    [ConsoleCommand("TelliogluSeferoglu")]
    [ConsoleCommand("TelliogluSeferogluBasla")]
    [ConsoleCommand("TelliogluVsSeferoglu")]
    [ConsoleCommand("TelliogluVsSeferogluBasla")]
    public void TelliogluSeferoglu(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        TelliSeferActive = !TelliSeferActive;
        switch (TelliSeferActive)
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
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        Server.ExecuteCommand($"host_workshop_map {Config.Map.TelliSeferWorkshopId}");
    }

    private static void TelliogluVsSeferogluRoundStart()
    {
        HookDisabled = true;
        foreach (var item in _Config.Map.TelliSeferCodes)
        {
            Server.ExecuteCommand(item);
        }
        Global?.AddTimer(1f, () =>
        {
            foreach (var item in _Config.Map.TelliSeferCodes)
            {
                Server.ExecuteCommand(item);
            }
            Model0Action();
        });

        Global?.AddTimer(8f, () =>
        {
            GetPlayers()
             .Where(x => x.PawnIsAlive)
             .ToList()
             .ForEach(x =>
             {
                 SetHp(x, 45);

                 RefreshPawn(x);
             });
        });
    }
}