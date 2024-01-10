using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Gonder

    [ConsoleCommand("gondert")]
    [CommandHelper(1, "<Hedef-player-ismi>")]
    public void GonderT(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var res = GonderGetVector(player!, info);
        if (res.Vector == null)
        {
            return;
        }

        GetPlayers(CsTeam.Terrorist)
              .Where(x => x.PawnIsAlive)
              .ToList()
              .ForEach(x =>
              {
                  x.PlayerPawn.Value.Teleport(new Vector(res.Vector.X, res.Vector.Y + 1, res.Vector.Z), new QAngle(0f, 0f, 0f), new Vector(0f, 0f, 0f));
              });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} tüm {CC.G}@t'yi {CC.R}{res.PlayerName}{CC.W} adlı oyunun yanına ışınladı.");
    }

    [ConsoleCommand("gonderct")]
    [CommandHelper(1, "<Hedef-player-ismi>")]
    public void GonderCt(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var res = GonderGetVector(player!, info);
        if (res.Vector == null)
        {
            return;
        }

        GetPlayers(CsTeam.CounterTerrorist)
              .Where(x => x.PawnIsAlive)
              .ToList()
              .ForEach(x =>
              {
                  x.PlayerPawn.Value.Teleport(new Vector(res.Vector.X, res.Vector.Y + 1, res.Vector.Z), new QAngle(0f, 0f, 0f), new Vector(0f, 0f, 0f));
              });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} tüm {CC.G}@ct'yi {CC.R}{res.PlayerName}{CC.W} adlı oyunun yanına ışınladı.");
    }

    [ConsoleCommand("gonderall")]
    [CommandHelper(1, "<Hedef-player-ismi>")]
    public void GonderAll(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var res = GonderGetVector(player!, info);
        if (res.Vector == null)
        {
            return;
        }

        GetPlayers()
              .Where(x => x.PawnIsAlive)
              .ToList()
              .ForEach(x =>
              {
                  x.PlayerPawn.Value.Teleport(new Vector(res.Vector.X, res.Vector.Y + 1, res.Vector.Z), new QAngle(0f, 0f, 0f), new Vector(0f, 0f, 0f));
              });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} tüm {CC.G}herkesi {CC.R}{res.PlayerName}{CC.W} adlı oyunun yanına ışınladı.");
    }

    private (Vector? Vector, string PlayerName) GonderGetVector(CCSPlayerController player, CommandInfo info)
    {
        if (info.ArgCount != 2) return (null, null);
        var target = info.GetArg(1);

        var players = GetPlayers()
               .Where(x => x.PlayerName.ToLower().Contains(target.ToLower()))
               .ToList();
        if (players.Count == 0)
        {
            player.PrintToChat($"{Prefix} {CC.W}Eşleşen oyuncu bulunamadı!");
            return (null, null);
        }
        if (players.Count != 1)
        {
            player.PrintToChat($"{Prefix} {CC.W}Birden fazla oyuncu bulundu.");
            return (null, null);
        }
        var x = players.FirstOrDefault();

        if (x?.SteamID != null && x!.SteamID != 0)
        {
            return (x.PlayerPawn.Value.AbsOrigin, x.PlayerName);
        }
        return (null, null);
    }

    #endregion Gonder
}