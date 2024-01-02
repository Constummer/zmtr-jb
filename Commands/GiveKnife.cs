using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region GK

    [ConsoleCommand("gk", "bicak Verir")]
    public void GiveKnife(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye9"))
        {
            player.PrintToChat($" {CC.LR}[ZMTR]{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        GetPlayers()
               .Where(x => x.PawnIsAlive)
               .ToList()
               .ForEach(x =>
               {
                   x.GiveNamedItem($"weapon_knife");
               });
        Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}@all {CC.W}hedefine {CC.B}knife {CC.W}adlı silahı verdi.");
    }

    #endregion GK
}