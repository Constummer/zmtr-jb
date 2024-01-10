using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Diz

    private bool DizActive = false;
    private ulong DizPlayerId = 0;
    private Tuple<float, float> DizStart = null;
    private Tuple<float, float> DizEnd = null;

    [ConsoleCommand("dizkapa")]
    public void DizKapa(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }

        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        DizPlayerId = 0;
        DizActive = false;
        player.PrintToChat($"{Prefix}{CC.W} oyuncu dizme kapandi.");
    }

    [ConsoleCommand("diz")]
    public void Diz(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }

        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        DizPlayerId = player.SteamID;
        DizActive = true;
        player.PrintToChat($"{Prefix}{CC.W} ATEŞ ETTİĞİN 2 NOKTA ARASINA TÜM T'LER DİZİLECEK.");
        player.PrintToChat($"{Prefix}{CC.W} ATEŞ ETTİĞİN 2 NOKTA ARASINA TÜM T'LER DİZİLECEK.");
        player.PrintToChat($"{Prefix}{CC.W} ATEŞ ETTİĞİN 2 NOKTA ARASINA TÜM T'LER DİZİLECEK.");
        player.PrintToChat($"{Prefix}{CC.W} ATEŞ ETTİĞİN 2 NOKTA ARASINA TÜM T'LER DİZİLECEK.");
        player.PrintToChat($"{Prefix}{CC.W} ATEŞ ETTİĞİN 2 NOKTA ARASINA TÜM T'LER DİZİLECEK.");
        player.PrintToChat($"{Prefix}{CC.W} ATEŞ ETTİĞİN 2 NOKTA ARASINA TÜM T'LER DİZİLECEK.");
        player.PrintToChat($"{Prefix}{CC.W} ATEŞ ETTİĞİN 2 NOKTA ARASINA TÜM T'LER DİZİLECEK.");
    }

    private void DizAction(EventBulletImpact @event)
    {
        Logger.LogInformation("0");

        if (DizActive == false)
        {
            return;
        }
        Logger.LogInformation("1");
        if (DizPlayerId == 0)
        {
            return;
        }
        Logger.LogInformation("2");

        var player = GetPlayers().Where(x => x.SteamID == DizPlayerId).FirstOrDefault();
        if (player == null)
        {
            return;
        }
        Logger.LogInformation("3");

        if (DizStart == null)
        {
            player.PrintToChat($"{Prefix}{CC.W} ATEŞ ETTİĞİN İLK NOKTA ALINDI.");
            player.PrintToChat($"{Prefix}{CC.W} ATEŞ ETTİĞİN İLK NOKTA ALINDI.");
            player.PrintToChat($"{Prefix}{CC.W} ATEŞ ETTİĞİN İLK NOKTA ALINDI.");
            DizStart = new Tuple<float, float>(@event.X, @event.Y);
            return;
        }
        Logger.LogInformation("4");

        if (DizEnd == null)
        {
            player.PrintToChat($"{Prefix}{CC.W} ATEŞ ETTİĞİN İKİNCİ NOKTA ALINDI.");
            player.PrintToChat($"{Prefix}{CC.W} ATEŞ ETTİĞİN İKİNCİ NOKTA ALINDI.");
            player.PrintToChat($"{Prefix}{CC.W} ATEŞ ETTİĞİN İKİNCİ NOKTA ALINDI.");
            player.PrintToChat($"{Prefix}{CC.W} OYUNCULAR DİZİLİYOR.");
            DizEnd = new Tuple<float, float>(@event.X, @event.Y);
        }
        Logger.LogInformation("5");

        var players = GetPlayers(CsTeam.Terrorist).Where(x => x.PawnIsAlive).ToList();
        if (players.Count > 0)
        {
            Logger.LogInformation("7");

            var res = InterpolatePoints(DizStart, DizEnd, players.Count);
            Logger.LogInformation("8");
            Logger.LogInformation($"res.Count={res.Count}");
            Logger.LogInformation($"players.Count={players.Count}");

            if (res.Count == players.Count)
            {
                Logger.LogInformation("9");

                for (int i = 0; i < players.Count; i++)
                {
                    Logger.LogInformation("10");

                    var x = players[i];
                    var coord = res[i];
                    if (ValidateCallerPlayer(x, false) == false)
                    {
                        continue;
                    }
                    if (coord == null)
                    {
                        continue;
                    }
                    Logger.LogInformation("11");

                    x.PlayerPawn.Value.Teleport(
                        new Vector(coord.Item1, coord.Item2, player.PlayerPawn.Value.AbsOrigin.Z),
                        x.PlayerPawn.Value.AbsRotation ?? ANGLE_ZERO,
                        VEC_ZERO);
                }
            }
        }
        Logger.LogInformation("6");

        DizPlayerId = 0;
        DizActive = false;
        DizStart = null;
        DizEnd = null;
    }

    private static List<Tuple<float, float>> InterpolatePoints(Tuple<float, float> start, Tuple<float, float> end, int dataCount)
    {
        List<Tuple<float, float>> points = new List<Tuple<float, float>>();

        for (int i = 0; i <= dataCount; i++)
        {
            float t = (float)i / dataCount;
            float interpolatedX = Interpolate(start.Item1, end.Item1, t);
            float interpolatedY = Interpolate(start.Item2, end.Item2, t);

            points.Add(Tuple.Create(interpolatedX, interpolatedY));
        }

        return points;
    }

    private static float Interpolate(float start, float end, float t)
    {
        return start + (end - start) * t;
    }

    #endregion Diz
}