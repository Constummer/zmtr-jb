using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Krediler

    [ConsoleCommand("krediler")]
    public void Krediler(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($" {CC.LR}[ZMTR]{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        GetPlayers()
                .ToList()
                .ForEach(x =>
                {
                    PlayerMarketModel item = null;
                    if (x?.SteamID != null && x!.SteamID != 0)
                    {
                        _ = PlayerMarketModels.TryGetValue(x.SteamID, out item);
                    }
                    player.PrintToConsole($" {CC.LR}[ZMTR] {CC.G}{x.PlayerName} - {CC.B}{(item?.Credit ?? 0)}");
                });

        player.PrintToChat($" {CC.LR}[ZMTR] {CC.G}Konsoluna bak");
    }

    #endregion Krediler
}