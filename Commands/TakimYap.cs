using CounterStrikeSharp.API;
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
        TeamSteamIds.Clear();

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
                player!.PrintToChat($" {CC.LR}[ZMTR]{CC.G} Takım sayısı değeri düzgün değil! 2 olarak ilerlendi. !takimboz ile bozup tekrar deneyebilirsiniz.");
            }
            if (chunk < 1)
            {
                player!.PrintToChat($" {CC.LR}[ZMTR]{CC.G} Takım sayısı değeri düzgün değil! en az 2 takım oluşturabilirsiniz. 2 takım oluşturuldu. !takimboz ile bozup tekrar deneyebilirsiniz.");
                chunk = 2;
            }
            if (chunk > 18)
            {
                player!.PrintToChat($" {CC.LR}[ZMTR]{CC.G} Takım sayısı değeri düzgün değil! en fazla 18 takım oluşturabilirsiniz 18 takım oluşturuldu. !takimboz ile bozup tekrar deneyebilirsiniz.");
                chunk = 18;
            }
            if (players.Count() > chunk)
            {
                chunk = players.Count() / 2;
                player!.PrintToChat($" {CC.LR}[ZMTR]{CC.G} Yaşayan oyuncu sayısı düzgün değil! {chunk} olarak ilerlendi. !takimboz ile bozup tekrar deneyebilirsiniz.");
            }
        }
        else if (info.ArgCount > 2)
        {
            chunk = 2;
            player!.PrintToChat($" {CC.LR}[ZMTR]{CC.G} Takım sayısı değeri düzgün değil! 2 olarak ilerlendi. !takimboz ile bozup tekrar deneyebilirsiniz.");
        }
        var chunked = ChunkBy(players.ToList(), chunk);
        if (chunked != null)
        {
            List<string> olusanTakimlar = new List<string>();
            for (int i = 0; i < chunked.Count; i++)
            {
                Console.WriteLine(chunked.Count);
                Console.WriteLine(chunked.Count);
                Console.WriteLine(chunked.Count);
                var plist = chunked[i];
                (string? Msg, Color Color) res = GetTeamColorAndTextByIndex(i);

                if (res.Msg == null)
                    return;
                plist.ForEach(x =>
                {
                    SetColour(x, res.Color);
                    x.PrintToChat($" {CC.LR}[ZMTR] {CC.P}{res.Msg} {CC.W}Takıma girdin!");
                    x.PrintToCenter($" {CC.LR}[ZMTR] {CC.P}{res.Msg} {CC.W}Takıma girdin!");

                    Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
                    Vector currentSpeed = new Vector(0, 0, 0);
                    QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
                    x.PlayerPawn.Value.Teleport(currentPosition, currentRotation, currentSpeed);
                });
                TeamSteamIds.Add(i, plist.Select(x => x.SteamID).ToList());
                olusanTakimlar.Add(res.Msg);
            }
            Server.PrintToChatAll($" {CC.LR}[ZMTR]{CC.G} Oluşturulan takımlar = {(string.Join(",", olusanTakimlar))}");
        }
        else
        {
            var teamBlue = players.Take(players.Count() / 2);
            var teamRed = players.Skip(players.Count() / 2);
            teamBlue.ToList().ForEach(x =>
            {
                SetColour(x, Color.FromArgb(255, 0, 0, 255));
                x.PrintToChat($" {CC.LR}[ZMTR] {CC.B}Mavi Takıma girdin!");
                x.PrintToCenter($" {CC.LR}[ZMTR] {CC.B}Mavi Takıma girdin!");

                Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
                Vector currentSpeed = new Vector(0, 0, 0);
                QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
                x.PlayerPawn.Value.Teleport(currentPosition, currentRotation, currentSpeed);
            });
            TeamSteamIds.Add(0, teamBlue.Select(x => x.SteamID).ToList());
            teamRed.ToList().ForEach(x =>
            {
                SetColour(x, Color.FromArgb(255, 255, 0, 0));
                x.PrintToChat($" {CC.LR}[ZMTR] {CC.R}Kırmızı Takıma girdin!");
                x.PrintToCenter($" {CC.LR}[ZMTR] {CC.R}Kırmızı Takıma girdin!");

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
            0 => ($"{CC.R} Kırmızı", Color.FromArgb(255, 0, 0)),
            1 => ($"{CC.B} Mavi", Color.FromArgb(0, 0, 255)),
            2 => ($"{CC.DR} Koyu Kırmızı", Color.FromArgb(139, 0, 0)),
            3 => ($"{CC.G} Yeşil", Color.FromArgb(0, 128, 0)),
            4 => ($"{CC.LY} Açık Sarı", Color.FromArgb(255, 255, 224)),
            5 => ($"{CC.LB} Açık Mavi", Color.FromArgb(173, 216, 230)),
            6 => ($"{CC.Ol} Koyu Yeşil", Color.FromArgb(128, 128, 0)),
            7 => ($"{CC.L} Açık Yeşil", Color.FromArgb(0, 255, 0)),
            8 => ($"{CC.LP} Açık Mor", Color.FromArgb(221, 160, 221)),
            9 => ($"{CC.P} Mor", Color.FromArgb(128, 0, 128)),
            10 => ($"{CC.Gr} Gri", Color.FromArgb(128, 128, 128)),
            11 => ($"{CC.Y} Sarı", Color.FromArgb(255, 255, 0)),
            12 => ($"{CC.Go} Altın", Color.FromArgb(255, 215, 0)),
            13 => ($"{CC.S} Gümüş", Color.FromArgb(192, 192, 192)),
            14 => ($"{CC.DB} Koyu Mavi", Color.FromArgb(0, 0, 139)),
            15 => ($"{CC.BG} Mavi Gri", Color.FromArgb(112, 128, 144)),
            16 => ($"{CC.M} Bordo", Color.FromArgb(128, 0, 0)),
            17 => ($"{CC.LR} Açık Kırmızı", Color.FromArgb(255, 182, 193)),
            18 => ($"{CC.Or} Turuncu", Color.FromArgb(255, 165, 0)),
            _ => (null, Color.Black),
        };
    }

    public static int GetTeamIndexByColor(char color)
    {
        if (color == CC.R)
            return 0;
        else if (color == CC.B)
            return 1;
        else if (color == CC.DR)
            return 2;
        else if (color == CC.G)
            return 3;
        else if (color == CC.LY)
            return 4;
        else if (color == CC.LB)
            return 5;
        else if (color == CC.Ol)
            return 6;
        else if (color == CC.L)
            return 7;
        else if (color == CC.LP)
            return 8;
        else if (color == CC.P)
            return 9;
        else if (color == CC.Gr)
            return 10;
        else if (color == CC.Y)
            return 11;
        else if (color == CC.Go)
            return 12;
        else if (color == CC.S)
            return 13;
        else if (color == CC.DB)
            return 14;
        else if (color == CC.BG)
            return 15;
        else if (color == CC.M)
            return 16;
        else if (color == CC.LR)
            return 17;
        else if (color == CC.Or)
            return 18;
        else
            return -1;
    }

    #endregion TakimYap
}