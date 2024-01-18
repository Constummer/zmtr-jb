using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public static Dictionary<int, List<ulong>> TeamSteamIds = new();
    private static bool TeamActive = false;

    #region TakimYap

    [ConsoleCommand("takimyap")]
    [CommandHelper(1, "<2-4 arasi bir deger girmelisiniz>")]
    public void TakimYap(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 2)
        {
            player.PrintToChat($"{Prefix}{CC.W} 2-4 arası bir değer girmelisiniz, 2 derseniz 64 kişiyi 2ye bölerek 32-32 yapar gibi ");
            return;
        }
        if (int.TryParse(info.ArgString.GetArg(0), out var chunk) == false)
        {
            player.PrintToChat($"{Prefix}{CC.W} 2-4 arası bir değer girmelisiniz, 2 derseniz 64 kişiyi 2ye bölerek 32-32 yapar gibi ");
            return;
        }
        else
        {
            if (chunk < 2 || chunk > 4)
            {
                player.PrintToChat($"{Prefix}{CC.W} 2-4 arası bir değer girmelisiniz, 2 derseniz 64 kişiyi 2ye bölerek 32-32 yapar gibi ");
                return;
            }
        }

        TakimYapAction(chunk);
    }

    [ConsoleCommand("takimboz")]
    [ConsoleCommand("takimiptal")]
    [ConsoleCommand("takimsil")]
    public void TakimBoz(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        TakimBozAction();
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}{CC.W} Takımları sildi");
    }

    private static void TakimBozAction()
    {
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
            SetColour(x, DefaultColor);

            Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
            Vector currentSpeed = new Vector(0, 0, 0);
            QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
            x.PlayerPawn.Value.Teleport(currentPosition, currentRotation, currentSpeed);
        });
        TeamSteamIds.Clear();
    }

    private static (string? Msg, Color Color, int Index) FindTeam(ulong steamID)
    {
        foreach (var team in TeamSteamIds)
        {
            if (team.Value.Contains(steamID))
            {
                var res = GetTeamColorAndTextByIndex(team.Key);
                return (res.Msg, res.Color, team.Key);
            }
        }
        return (null, Color.Black, -1);
    }

    private void TakimYapAction(int chunk, bool additionalMsg = false)
    {
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

        var chunked = ChunkBy(players.ToList(), chunk);

        List<string> olusanTakimlar = new List<string>();
        for (int i = 0; i < chunked.Count; i++)
        {
            var plist = chunked[i];
            (string? Msg, Color Color) res = GetTeamColorAndTextByIndex(i);

            if (res.Msg == null)
                return;
            plist.ForEach(x =>
            {
                SetColour(x, res.Color);
                x.PrintToChat($"{Prefix} {CC.P}{res.Msg} {CC.W}Takıma girdin!{(additionalMsg ? " Takım oyunu başlamak üzere" : "")}");
                x.PrintToCenter($"{Prefix} {CC.P}{res.Msg} {CC.W}Takıma girdin!{(additionalMsg ? " Takım oyunu başlamak üzere" : "")}");

                Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
                Vector currentSpeed = new Vector(0, 0, 0);
                QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
                x.PlayerPawn.Value.Teleport(currentPosition, currentRotation, currentSpeed);
            });
            TeamSteamIds.Add(i, plist.Select(x => x.SteamID).ToList());
            olusanTakimlar.Add(res.Msg);
        }

        Server.PrintToChatAll($"{Prefix}{CC.G} Oluşturulan takımlar = {(string.Join(",", olusanTakimlar))}");
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
            2 => ($"{CC.G} Yeşil", Color.FromArgb(0, 255, 0)),
            3 => ($"{CC.Gr} Gri", Color.FromArgb(0, 0, 0)),

            //4 => ($"{CC.LY} Açık Sarı", Color.FromArgb(255, 255, 224)),
            //5 => ($"{CC.LB} Açık Mavi", Color.FromArgb(173, 216, 230)),
            //6 => ($"{CC.Ol} Koyu Yeşil", Color.FromArgb(128, 128, 0)),
            //7 => ($"{CC.L} Açık Yeşil", Color.FromArgb(0, 255, 0)),
            //8 => ($"{CC.LP} Açık Mor", Color.FromArgb(221, 160, 221)),
            //9 => ($"{CC.Go} Altın", Color.FromArgb(255, 215, 0)),
            //10 => ($"{CC.P} Mor", Color.FromArgb(128, 0, 128)),

            //11 => ($"{CC.Y} Sarı", Color.FromArgb(255, 255, 0)),
            //12 => ($"{CC.DR} Koyu Kırmızı", Color.FromArgb(139, 0, 0)),
            //13 => ($"{CC.S} Gümüş", Color.FromArgb(192, 192, 192)),
            //14 => ($"{CC.DB} Koyu Mavi", Color.FromArgb(0, 0, 139)),
            //15 => ($"{CC.BG} Mavi Gri", Color.FromArgb(112, 128, 144)),
            //16 => ($"{CC.Gr} Gri", Color.FromArgb(128, 128, 128)),
            //17 => ($"{CC.LR} Açık Kırmızı", Color.FromArgb(255, 182, 193)),
            //18 => ($"{CC.Or} Turuncu", Color.FromArgb(255, 165, 0)),
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
        else if (color == CC.Gr)
            return 3;
        //else if (color == CC.LY)
        //    return 4;
        //else if (color == CC.LB)
        //    return 5;
        //else if (color == CC.Ol)
        //    return 6;
        //else if (color == CC.L)
        //    return 7;
        //else if (color == CC.LP)
        //    return 8;
        //else if (color == CC.P)
        //    return 9;
        //else if (color == CC.Gr)
        //    return 10;
        //else if (color == CC.Y)
        //    return 11;
        //else if (color == CC.Go)
        //    return 12;
        //else if (color == CC.S)
        //    return 13;
        //else if (color == CC.DB)
        //    return 14;
        //else if (color == CC.BG)
        //    return 15;
        //else if (color == CC.M)
        //    return 16;
        //else if (color == CC.LR)
        //    return 17;
        //else if (color == CC.Or)
        //    return 18;
        else
            return -1;
    }

    private static Dictionary<int, int> GetTeamPlayerCounts()
    {
        Dictionary<int, int> playerCount = new();

        GetPlayers()
                .Select(x => x.SteamID)
                .ToList()
                .ForEach(x =>
                {
                    var team = FindTeam(x);
                    if (team.Index == -1)
                        return;
                    if (playerCount.ContainsKey(team.Index) == false)
                    {
                        playerCount.Add(team.Index, 1);
                    }
                    else
                    {
                        playerCount[team.Index]++;
                    }
                });
        return playerCount;
    }

    #endregion TakimYap
}