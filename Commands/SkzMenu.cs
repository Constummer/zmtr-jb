using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;
using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    //private Dictionary<string, Vector> SkzCoordinates = new()
    //{
    //
    //};
    //private List<SkzCoordinate> SkzCoordinates = new List<SkzCoordinate>()
    //{
    //     new("Hücre",   new Vector(-535,345,-27) ),
    //     new("KZ",      new Vector(2102,739,-357) ),
    //};
    public class VectorTemp
    {
        public VectorTemp(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        [JsonPropertyName("X")]
        public float X { get; set; }

        [JsonPropertyName("Y")]
        public float Y { get; set; }

        [JsonPropertyName("Z")]
        public float Z { get; set; }
    }

    public class SkzCoordinate
    {
        public SkzCoordinate(string text, VectorTemp coords)
        {
            Text = text;
            Coords = coords;
        }

        [JsonPropertyName("Text")]
        public string Text { get; set; } = "";

        [JsonPropertyName("Coord")]
        public VectorTemp Coords { get; set; } = new VectorTemp(0, 0, 0);

        [JsonIgnore]
        public Vector Coord { get => new Vector(Coords.X, Coords.Y, Coords.Z); }
    }

    private CounterStrikeSharp.API.Modules.Timers.Timer SkzTimer = null;
    private CounterStrikeSharp.API.Modules.Timers.Timer Skz2Timer = null;

    #region SKZ

    [ConsoleCommand("skz")]
    [CommandHelper(1, "<saniye>")]
    public void SkzMenu(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        if (info.ArgCount != 2)
        {
            return;
        }
        var target = info.GetArg(1);
        if (int.TryParse(target, out var value))
        {
            if (value > 120)
            {
                player.PrintToChat("Max 120 sn girebilirsin");
                return;
            }
            else if (value < 4)
            {
                player.PrintToChat("Min 3 sn girebilirsin");
                return;
            }
        }

        if ((Config.Map.SkzCoordinates?.TryGetValue(Server.MapName, out var coords) ?? false) == false || coords == null || coords.Count == 0)
        {
            player.PrintToChat("SKZ konumları girilmemiş bir mapte oynamaktasınız. Admin ile görüşmelisiniz");
            return;
        }

        var skzMenu = new ChatMenu("SKZ Menü");

        foreach (var k in coords)
        {
            skzMenu.AddMenuOption(k.Text, (p, t) =>
            {
                Server.PrintToChatAll($"{Prefix} {CC.W}Mahkûmlar {CC.B}{k.Text} {CC.W} ışınlanıyor");

                var players = GetPlayers(CsTeam.Terrorist)
                    .Where(x => x.PawnIsAlive == true)
                    .ToList();

                players.ForEach(x =>
                {
                    x.PlayerPawn.Value.MoveType = MoveType_t.MOVETYPE_OBSOLETE;
                    x.PlayerPawn.Value.Teleport(k.Coord, x.PlayerPawn.Value.AbsRotation, new Vector(0f, 0f, 0f));
                });
                BasicCountdown.CommandStartTextCountDown(this, $"[ZMTR] SKZ 3 SANİYE SONRA BAŞLIYOR");

                _ = AddTimer(3, () =>
                {
                    GetPlayers()
                    .Where(x => x != null
                         && x.IsValid
                         && x.PawnIsAlive
                         && GetTeam(x) == CsTeam.Terrorist)
                    .ToList()
                    .ForEach(x =>
                    {
                        if (ValidateCallerPlayer(x, false) == false) return;
                        if (TeamActive == false)
                        {
                            if (players.Count < 6)
                            {
                                SetColour(x, DefaultColor);
                            }
                            else
                            {
                                SetColour(x, Color.FromArgb(0, 0, 0, 0));
                            }
                        }
                        x.PlayerPawn.Value!.MoveType = MoveType_t.MOVETYPE_WALK;
                        RefreshPawnTP(x);
                    });
                    FreezeOrUnfreezeSound();
                    BasicCountdown.CommandStartTextCountDown(this, $"mahkûmlar {value} saniye sonra donacak");
                }, SOM);

                _ = AddTimer(value + 3, () =>
                {
                    GetPlayers()
                    .Where(x => x != null
                         && x.IsValid
                         && x.PawnIsAlive
                         && GetTeam(x) == CsTeam.Terrorist)
                    .ToList()
                    .ForEach(x =>
                    {
                        if (ValidateCallerPlayer(x, false) == false) return;
                        if (TeamActive == false)
                        {
                            SetColour(x, Config.Burry.BuryColor);
                        }
                        x.PlayerPawn.Value!.MoveType = MoveType_t.MOVETYPE_OBSOLETE;
                        Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
                        x.PlayerPawn.Value.Teleport(currentPosition, x.PlayerPawn.Value.AbsRotation, new Vector(0, 0, 0));
                    });
                    FreezeOrUnfreezeSound();
                    Server.PrintToChatAll($"{Prefix} {CC.Ol}{value}{CC.W} saniye süren {CC.Ol}SKZ{CC.W} bitti, {CC.G}mahkûmlar {CC.B}dondu{CC.W}.");
                }, SOM);
            });
        }
        ChatMenus.OpenMenu(player, skzMenu);
    }

    #endregion SKZ
}