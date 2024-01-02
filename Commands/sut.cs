using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Sut

    [ConsoleCommand("sut")]
    [ConsoleCommand("sutol")]
    public void Sut(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (GetTeam(player) != CsTeam.Terrorist)
        {
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR]{ChatColors.White} Sadece T ler bu komutu kullanabilir.");
            return;
        }
        GetPlayers()
        .Where(x => x.PawnIsAlive && x.SteamID == player!.SteamID)
        .ToList()
        .ForEach(x =>
        {
            RemoveWeapons(x, false);
            Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı oyuncu, {ChatColors.LightPurple}süt oldu.");
        });
    }

    #endregion Sut
}