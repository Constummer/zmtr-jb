using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Dictionary<int, List<ulong>> TeamSteamIds = new();
    private static bool TeamActive = false;

    #region TakimYap

    [ConsoleCommand("takimyap")]
    [CommandHelper(0, "<(varsayilan 2) | 2-18 arasi bir deger girmelisiniz, 2serliden baslamak uzere takimlara boler>")]
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
        var chunk = 2;
        if (info.ArgCount == 2)
        {
            if (!int.TryParse(info.GetArg(1), out chunk))
            {
                chunk = 2;
                player!.PrintToChat($" {ChatColors.LightRed}[ZMTR]{ChatColors.Green} Takım sayısı değeri düzgün değil! 2 olarak ilerlendi. !takimboz ile bozup tekrar deneyebilirsiniz.");
            }
            if (chunk < 2)
            {
                player!.PrintToChat($" {ChatColors.LightRed}[ZMTR]{ChatColors.Green} Takım sayısı değeri düzgün değil! en az 2 takım oluşturabilirsiniz. 2 takım oluşturuldu. !takimboz ile bozup tekrar deneyebilirsiniz.");
                chunk = 2;
            }
            if (chunk > 18)
            {
                player!.PrintToChat($" {ChatColors.LightRed}[ZMTR]{ChatColors.Green} Takım sayısı değeri düzgün değil! en fazla 18 takım oluşturabilirsiniz 18 takım oluşturuldu. !takimboz ile bozup tekrar deneyebilirsiniz.");
                chunk = 18;
            }
            if (players.Count() > chunk)
            {
                chunk = players.Count() / 2;
                player!.PrintToChat($" {ChatColors.LightRed}[ZMTR]{ChatColors.Green} Yaşayan oyuncu sayısı düzgün değil! {chunk} olarak ilerlendi. !takimboz ile bozup tekrar deneyebilirsiniz.");
            }
        }
        else if (info.ArgCount > 2)
        {
            chunk = 2;
            player!.PrintToChat($" {ChatColors.LightRed}[ZMTR]{ChatColors.Green} Takım sayısı değeri düzgün değil! 2 olarak ilerlendi. !takimboz ile bozup tekrar deneyebilirsiniz.");
        }
        var chunked = ChunkBy(players.ToList(), 2);
        if (chunked != null)
        {
            for (int i = 0; i < chunked.Count; i++)
            {
                var plist = chunked[i];
                var res = GetTeamColorAndTextByIndex(i);

                if (res.Msg == null)
                    return;
                plist.ForEach(x =>
                {
                    SetColour(x, res.Color);
                    x.PrintToChat($" {ChatColors.LightRed}[ZMTR] {res.Msg} Takıma girdin!");
                    x.PrintToCenter($" {ChatColors.LightRed}[ZMTR] {res.Msg} Takıma girdin!");

                    Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
                    Vector currentSpeed = new Vector(0, 0, 0);
                    QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
                    x.PlayerPawn.Value.Teleport(currentPosition, currentRotation, currentSpeed);
                });
                TeamSteamIds.Add(i, plist.Select(x => x.SteamID).ToList());
            }
        }
        else
        {
            var teamBlue = players.Take(players.Count() / 2);
            var teamRed = players.Skip(players.Count() / 2);
            teamBlue.ToList().ForEach(x =>
            {
                SetColour(x, Color.FromArgb(255, 0, 0, 255));
                x.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.Blue} Mavi Takıma girdin!");
                x.PrintToCenter($" {ChatColors.LightRed}[ZMTR] {ChatColors.Blue} Mavi Takıma girdin!");

                Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
                Vector currentSpeed = new Vector(0, 0, 0);
                QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
                x.PlayerPawn.Value.Teleport(currentPosition, currentRotation, currentSpeed);
            });
            TeamSteamIds.Add(0, teamBlue.Select(x => x.SteamID).ToList());
            teamRed.ToList().ForEach(x =>
            {
                SetColour(x, Color.FromArgb(255, 255, 0, 0));
                x.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.Red} Kırmızı Takıma girdin!");
                x.PrintToCenter($" {ChatColors.LightRed}[ZMTR] {ChatColors.Red} Kırmızı Takıma girdin!");

                Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
                Vector currentSpeed = new Vector(0, 0, 0);
                QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
                x.PlayerPawn.Value.Teleport(currentPosition, currentRotation, currentSpeed);
            });
            TeamSteamIds.Add(1, teamRed.Select(x => x.SteamID).ToList());
        }
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
        TeamSteamIds.Clear();
    }

    private static void TeamYapActive(CCSPlayerController attacker, CCSPlayerController victim, int dmgHealth, int dmgArmor)
    {
        if (TeamActive == true)
        {
            foreach (var item in TeamSteamIds)
            {
                if (item.Value != null)
                {
                    if (item.Value.Contains(attacker.SteamID)
                        && item.Value.Contains(victim.SteamID))
                    {
                        AddHp(victim, dmgHealth, dmgArmor);
                    }
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

    private static (string? Msg, Color Color) GetTeamColorAndTextByIndex(int index)
    {
        return index switch
        {
            0 => ($"{ChatColors.Red} Kırmızı", Color.FromArgb(255, 0, 0)),
            1 => ($"{ChatColors.Blue} Mavi", Color.FromArgb(0, 0, 255)),
            2 => ($"{ChatColors.Darkred} Koyu Kırmızı", Color.FromArgb(139, 0, 0)),
            3 => ($"{ChatColors.Green} Yeşil", Color.FromArgb(0, 128, 0)),
            4 => ($"{ChatColors.LightYellow} Açık Sarı", Color.FromArgb(255, 255, 224)),
            5 => ($"{ChatColors.LightBlue} Açık Mavi", Color.FromArgb(173, 216, 230)),
            6 => ($"{ChatColors.Olive} Zeytin Yeşili", Color.FromArgb(128, 128, 0)),
            7 => ($"{ChatColors.Lime} Lime Yeşili", Color.FromArgb(0, 255, 0)),
            8 => ($"{ChatColors.LightPurple} Açık Mor", Color.FromArgb(221, 160, 221)),
            9 => ($"{ChatColors.Purple} Mor", Color.FromArgb(128, 0, 128)),
            10 => ($"{ChatColors.Grey} Gri", Color.FromArgb(128, 128, 128)),
            11 => ($"{ChatColors.Yellow} Sarı", Color.FromArgb(255, 255, 0)),
            12 => ($"{ChatColors.Gold} Altın", Color.FromArgb(255, 215, 0)),
            13 => ($"{ChatColors.Silver} Gümüş", Color.FromArgb(192, 192, 192)),
            14 => ($"{ChatColors.DarkBlue} Koyu Mavi", Color.FromArgb(0, 0, 139)),
            15 => ($"{ChatColors.BlueGrey} Mavi Gri", Color.FromArgb(112, 128, 144)),
            16 => ($"{ChatColors.Magenta} Bordo", Color.FromArgb(128, 0, 0)),
            17 => ($"{ChatColors.LightRed} Açık Kırmızı", Color.FromArgb(255, 182, 193)),
            18 => ($"{ChatColors.Orange} Turuncu", Color.FromArgb(255, 165, 0)),
            _ => (null, Color.Black),
        };
    }

    #endregion TakimYap
}