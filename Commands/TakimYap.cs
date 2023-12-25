using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static List<ulong> TeamBlueSteamIds = new List<ulong>();
    private static List<ulong> TeamRedSteamIds = new List<ulong>();
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
                   && GetTeam(x) == CsTeam.Terrorist
                   && ValidateCallerPlayer(x, false));

        var teamBlue = players.Take(players.Count() / 2);
        var teamRed = players.Skip(players.Count() / 2);
        teamBlue.ToList().ForEach(x =>
              {
                  SetColour(x, Color.FromArgb(255, 0, 0, 255));
                  x.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.Blue} Kırmızı Takıma girdin!");
                  x.PrintToCenter($" {ChatColors.LightRed}[ZMTR] {ChatColors.Blue} Kırmızı Takıma girdin!");

                  Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
                  Vector currentSpeed = new Vector(0, 0, 0);
                  QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
                  x.PlayerPawn.Value.Teleport(currentPosition, currentRotation, currentSpeed);
              });
        TeamBlueSteamIds = teamBlue.Select(x => x.SteamID).ToList();
        teamRed.ToList().ForEach(x =>
        {
            SetColour(x, Color.FromArgb(255, 255, 0, 0));
            x.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.Red} Mavi Takıma girdin!");
            x.PrintToCenter($" {ChatColors.LightRed}[ZMTR] {ChatColors.Red} Mavi Takıma girdin!");

            Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
            Vector currentSpeed = new Vector(0, 0, 0);
            QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
            x.PlayerPawn.Value.Teleport(currentPosition, currentRotation, currentSpeed);
        });
        TeamRedSteamIds = teamRed.Select(x => x.SteamID).ToList();
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
            if (TeamBlueSteamIds != null)
            {
                if (TeamBlueSteamIds.Contains(attacker.SteamID)
                    && TeamBlueSteamIds.Contains(victim.SteamID))
                {
                    AddHp(victim, dmgHealth, dmgArmor);
                }
            }
            if (TeamRedSteamIds != null)
            {
                if (TeamRedSteamIds.Contains(attacker.SteamID)
                    && TeamRedSteamIds.Contains(victim.SteamID))
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