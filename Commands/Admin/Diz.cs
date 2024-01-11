using CounterStrikeSharp.API;
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
        if (!AdminManager.PlayerHasPermissions(player, "@css/lider"))
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
        if (!AdminManager.PlayerHasPermissions(player, "@css/lider"))
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
        player.PrintToChat($"{Prefix}{CC.W} Ateş Ettiğin 2 Nokta Arasına Tüm Mahkûmlar Dizilecek.");
        player.PrintToChat($"{Prefix}{CC.W} Ateş Ettiğin 2 Nokta Arasına Tüm Mahkûmlar Dizilecek.");
        player.PrintToChat($"{Prefix}{CC.W} Ateş Ettiğin 2 Nokta Arasına Tüm Mahkûmlar Dizilecek.");
        player.PrintToChat($"{Prefix}{CC.W} Ateş Ettiğin 2 Nokta Arasına Tüm Mahkûmlar Dizilecek.");
    }

    private void DizAction(EventBulletImpact @event)
    {
        if (DizActive == false)
        {
            return;
        }

        if (DizPlayerId == 0)
        {
            return;
        }
        if (@event.Userid.SteamID != DizPlayerId)
        {
            return;
        }

        var player = GetPlayers().Where(x => x.SteamID == DizPlayerId).FirstOrDefault();
        if (player == null)
        {
            return;
        }

        if (DizStart == null)
        {
            DizStart = new Tuple<float, float>(@event.X, @event.Y);
            return;
        }

        if (DizEnd == null)
        {
            DizEnd = new Tuple<float, float>(@event.X, @event.Y);
        }

        var players = GetPlayers(CsTeam.Terrorist).Where(x => x.PawnIsAlive).ToList();
        if (players.Count > 0)
        {
            var res = InterpolatePoints(DizStart, DizEnd, players.Count);

            if (players.Count == 1)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    var x = players[i];
                    if (ValidateCallerPlayer(x, false) == false)
                    {
                        continue;
                    }

                    x.PlayerPawn.Value.Teleport(
                        new Vector(DizStart.Item1, DizStart.Item2, player.PlayerPawn.Value.AbsOrigin.Z),
                        x.PlayerPawn.Value.AbsRotation ?? ANGLE_ZERO,
                        VEC_ZERO);
                }
            }
            else if (players.Count == 2)
            {
                var first = players[0];
                var second = players[1];
                first.PlayerPawn.Value.Teleport(
                    new Vector(DizStart.Item1, DizStart.Item2, player.PlayerPawn.Value.AbsOrigin.Z),
                    first.PlayerPawn.Value.AbsRotation ?? ANGLE_ZERO,
                    VEC_ZERO);
                second.PlayerPawn.Value.Teleport(
                  new Vector(DizEnd.Item1, DizEnd.Item2, player.PlayerPawn.Value.AbsOrigin.Z),
                  second.PlayerPawn.Value.AbsRotation ?? ANGLE_ZERO,
                  VEC_ZERO);
            }
            else if (res.Count == players.Count)
            {
                for (int i = 0; i < players.Count; i++)
                {
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

                    x.PlayerPawn.Value.Teleport(
                        new Vector(coord.Item1, coord.Item2, player.PlayerPawn.Value.AbsOrigin.Z),
                        x.PlayerPawn.Value.AbsRotation ?? ANGLE_ZERO,
                        VEC_ZERO);
                }
            }
        }

        DizPlayerId = 0;
        DizActive = false;
        DizStart = null;
        DizEnd = null;
    }

    private static List<Tuple<float, float>> InterpolatePoints(Tuple<float, float> start, Tuple<float, float> end, int dataCount)
    {
        List<Tuple<float, float>> points = new List<Tuple<float, float>>();

        for (int i = 0; i < dataCount; i++)
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