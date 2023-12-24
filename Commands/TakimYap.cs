using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static List<ulong> TeamOneSteamIds = new List<ulong>();
    private static List<ulong> TeamTwoSteamIds = new List<ulong>();
    private static bool TeamActive = false;

    #region TakimYap

    [ConsoleCommand("takimyap")]
    public void TakimYap(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        TeamActive = true;
        var players = GetPlayers()
              .Where(x => x != null
                   && x.PlayerPawn.IsValid
                   && x.PawnIsAlive
                   && x.IsValid
                   && x?.PlayerPawn?.Value != null
                   && GetTeam(x) == CsTeam.Terrorist);

        var teamOne = players.Take(players.Count() / 2);
        var teamTwo = players.Skip(players.Count() / 2);
        teamOne.ToList().ForEach(x =>
              {
                  SetColour(x, Config.TeamOneColor);

                  Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
                  Vector currentSpeed = new Vector(0, 0, 0);
                  QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
                  x.PlayerPawn.Value.Teleport(currentPosition, currentRotation, currentSpeed);
              });
        TeamOneSteamIds = teamOne.Select(x => x.SteamID).ToList();
        teamTwo.ToList().ForEach(x =>
        {
            SetColour(x, Config.TeamTwoColor);

            Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
            Vector currentSpeed = new Vector(0, 0, 0);
            QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
            x.PlayerPawn.Value.Teleport(currentPosition, currentRotation, currentSpeed);
        });
        TeamTwoSteamIds = teamTwo.Select(x => x.SteamID).ToList();
    }

    [ConsoleCommand("takimboz")]
    public void TakimBoz(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        TeamActive = false;

        var players = GetPlayers()
              .Where(x => x != null
                   && x.PlayerPawn.IsValid
                   && x.PawnIsAlive
                   && x.IsValid
                   && x?.PlayerPawn?.Value != null
                   && GetTeam(x) == CsTeam.Terrorist)
              .ToList();

        players.ForEach(x =>
            {
                SetColour(x, DefaultPlayerColor);

                Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
                Vector currentSpeed = new Vector(0, 0, 0);
                QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
                x.PlayerPawn.Value.Teleport(currentPosition, currentRotation, currentSpeed);
            });
    }

    private static void TeamYapActive(CCSPlayerController attacker, CCSPlayerController victim, int dmgHealth, int dmgArmor)
    {
        if (TeamActive == true)
        {
            if (TeamOneSteamIds != null)
            {
                if (TeamOneSteamIds.Contains(attacker.SteamID)
                    && TeamOneSteamIds.Contains(victim.SteamID))
                {
                    AddHp(victim, dmgHealth, dmgArmor);
                }
            }
            if (TeamTwoSteamIds != null)
            {
                if (TeamTwoSteamIds.Contains(attacker.SteamID)
                    && TeamTwoSteamIds.Contains(victim.SteamID))
                {
                    AddHp(victim, dmgHealth, dmgArmor);
                }
            }
        }
    }

    private static void AddHp(CCSPlayerController player, int health, int armor)
    {
        player.Health += health;
        player.PlayerPawn.Value!.Health += health;
        if (player.PawnArmor != 0)
        {
            player.PawnArmor += armor;
        }
        if (player.PlayerPawn.Value!.ArmorValue != 0)
        {
            player.PlayerPawn.Value!.ArmorValue += armor;
        }
    }

    #endregion TakimYap
}