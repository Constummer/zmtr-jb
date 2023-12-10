using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region God

    [ConsoleCommand("god", "godmode a player")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me>")]
    public void God(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);

        GetPlayers()
               .Where(x => x.PawnIsAlive
                        && GetTargetAction(x, target, player.PlayerName))
               .ToList()
               .ForEach(x =>
               {
                   if (ActiveGodMode.TryGetValue(x.SteamID, out var god))
                   {
                       if (god)
                       {
                           FakeGod(x, 100);
                       }
                       else
                       {
                           FakeGod(x, int.MaxValue);
                       }

                       ActiveGodMode[x.SteamID] = !god;
                   }
                   else
                   {
                       ActiveGodMode.TryAdd(x.SteamID, true);
                       FakeGod(x, int.MaxValue);
                   }
                   RefreshPawn(x);
               });
    }

    [ConsoleCommand("q", "godmode ct player")]
    public void Q(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GetPlayers(CsTeam.CounterTerrorist)
               .ToList()
               .ForEach(x =>
               {
                   ActiveGodMode[x.SteamID] = true;
                   FakeGod(x, int.MaxValue);
                   RefreshPawn(x);
               });
    }

    [ConsoleCommand("qq", "remove godmode ct player")]
    public void Qq(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GetPlayers(CsTeam.CounterTerrorist)
               .ToList()
               .ForEach(x =>
               {
                   ActiveGodMode.Remove(x.SteamID);
                   FakeGod(x, 100);
                   RefreshPawn(x);
               });
    }

    private static void FakeGod(CCSPlayerController x, int amount)
    {
        x.Pawn.Value!.Health = amount;
        var playerPawnValue = x.PlayerPawn.Value;
        if (playerPawnValue != null)
        {
            playerPawnValue.ArmorValue = amount;
        }
    }

    #endregion God
}