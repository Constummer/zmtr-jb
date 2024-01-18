using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region OnTeamCommand

    [ConsoleCommand("swap2")]
    [CommandHelper(0, "<nick-#userid-@me> <nick-#userid-@me>")]
    public void OnSwap2Command(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye6") == false)
        {
            return;
        }
        if (info.ArgCount != 3) return;
        var swap1res = SwapTargetOne(info.GetArg(1), player);
        var swap2res = SwapTargetOne(info.GetArg(2), player);
        if (swap1res.Res && swap2res.Res)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{swap1res.PName} {CC.W} ile {CC.B}{swap2res.PName} {CC.W} swapladı.");
        }
    }

    private (bool Res, string PName) SwapTargetOne(string target, CCSPlayerController? player)
    {
        var targetArgument = GetTargetArgument(target);
        var players = GetPlayers()
             .Where(x =>
             (targetArgument == TargetForArgument.UserIdIndex
             ? GetUserIdIndex(target) == x.UserId : targetArgument == TargetForArgument.Me ? x.SteamID == player.SteamID : false)
                          || x.PlayerName.ToLower().Contains(target.ToLower()))
             .ToList();
        if (players.Count == 0)
        {
            player.PrintToChat($"{Prefix} {CC.W}Eşleşen oyuncu bulunamadı!");
            return (false, null);
        }
        if (players.Count != 1)
        {
            player.PrintToChat($"{Prefix} {CC.W}Birden fazla oyuncu bulundu.");
            return (false, null);
        }
        var x = players.FirstOrDefault();
        Unmuteds = Unmuteds.Where(X => X != x.SteamID).ToList();
        x.VoiceFlags |= VoiceFlags.Muted;

        if (x?.SteamID != null && x!.SteamID != 0)
        {
            if (ValidateCallerPlayer(x, false) == false) return (false, null);
            switch (GetTeam(x))
            {
                case CsTeam.CounterTerrorist:
                    if (ValidateCallerPlayer(x, false) == false) return (false, null);
                    if (x.SteamID == LatestWCommandUser)
                    {
                        RemoveWardenAction(x);
                    }
                    x.ChangeTeam(CsTeam.Terrorist);
                    break;

                case CsTeam.Terrorist:
                    if (CTBanCheck(x) == false)
                    {
                        Server.PrintToChatAll($"{Prefix} {CC.W}{x.PlayerName} CT banı olduğu için CT atılamadı!");
                        return (false, null);
                    }
                    if (ValidateCallerPlayer(x, false) == false) return (false, null);
                    x.ChangeTeam(CsTeam.CounterTerrorist); break;

                default: break;
            }
            if (ActiveGodMode.ContainsKey(x.SteamID))
            {
                ActiveGodMode[x.SteamID] = false;
            }
            else
            {
                ActiveGodMode.TryAdd(x.SteamID, false);
            }
            return (true, x.PlayerName);
        }
        return (false, null);
    }

    #endregion OnTeamCommand
}