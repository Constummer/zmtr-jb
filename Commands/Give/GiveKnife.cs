using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

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
                   x.GiveNamedItem($"weapon_knife");
               });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}@all {CC.W}hedefine {CC.B}knife {CC.W}adlı silahı verdi.");
    }

    [ConsoleCommand("gdd", "bicak Verir")]
    public void gdd(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false) return;
        LogManagerCommand(player.SteamID, info.GetCommandString);

        GetPlayers()
               .Where(x => x.PawnIsAlive)
               .ToList()
               .ForEach(x =>
               {
                   RemoveAllButKnife(x);
               });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}@all {CC.W}hedefine {CC.B}gdd.");
    }

    #endregion GK
}