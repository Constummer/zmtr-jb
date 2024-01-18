using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region SonKalan

    [ConsoleCommand("sonkalan")]
    [ConsoleCommand("sonakalan")]
    [CommandHelper(1, "<nick>")]
    public void SonKalan(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.ArgString.GetArg(0);

        var players = GetPlayers()
                .Where(x => x.PlayerName.ToLower().Contains(target.ToLower()))
                .ToList();
        if (players.Count == 0)
        {
            player.PrintToChat($"{Prefix} {CC.W}Eşleşen oyuncu bulunamadı!");
            return;
        }
        else if (players.Count > 1)
        {
            player.PrintToChat($"{Prefix} {CC.W}Eşleşen birden fazla oyuncu bulundu!");
            return;
        }

        var sk = players.FirstOrDefault();
        if (ValidateCallerPlayer(sk, false) == false)
        {
            return;
        }
        var team = sk.TeamNum;
        GetPlayers((CsTeam)team)
          .Where(x => x.PawnIsAlive && x.SteamID != sk.SteamID)
          .ToList()
          .ForEach(x =>
          {
              if (ValidateCallerPlayer(player, false) == false) return;
              x.CommitSuicide(false, true);
          });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{sk.PlayerName} {CC.W} adlı oyuncuyu son kalan olarak işaretledi.");
    }

    #endregion SonKalan
}