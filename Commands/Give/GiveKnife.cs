using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region GK

    [ConsoleCommand("gk", "bicak Verir")]
    public void GiveKnife(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false) return;
        LogManagerCommand(player.SteamID, info.GetCommandString);

        GetPlayers()
               .Where(x => x.PawnIsAlive)
               .ToList()
               .ForEach(x =>
               {
                   switch (GetTeam(x))
                   {
                       case CsTeam.Terrorist:
                           x.GiveNamedItem($"weapon_knife_t");
                           break;

                       default:
                           x.GiveNamedItem($"weapon_knife");
                           break;
                   }
               });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}@all {CC.W}hedefine {CC.B}knife {CC.W}adlı silahı verdi.");
    }

    #endregion GK
}