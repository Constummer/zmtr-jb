using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private CounterStrikeSharp.API.Modules.Timers.Timer SkzTimer = null;
    private CounterStrikeSharp.API.Modules.Timers.Timer Skz2Timer = null;

    #region SKZ

    [ConsoleCommand("skz")]
    [CommandHelper(1, "<saniye>")]
    public void Skz(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        if (info.ArgCount != 2)
        {
            return;
        }
        var target = info.ArgString.GetArg(0);
        if (int.TryParse(target, out var value))
        {
            if (value > 120)
            {
                player.PrintToChat("Max 120 sn girebilirsin");
                return;
            }
            else if (value < 1)
            {
                player.PrintToChat("Min 1 sn girebilirsin");
                return;
            }
        }

        if ((Config.Map.SkzCoordinates?.TryGetValue(Server.MapName, out var coords) ?? false) == false || coords == null || coords.Count == 0)
        {
            player.PrintToChat("SKZ konumları girilmemiş bir mapte oynamaktasınız. Admin ile görüşmelisiniz");
            return;
        }

        var abs = player.PlayerPawn.Value.AbsOrigin;
        var skzMenu = new ChatMenu("SKZ Menü");
        var coordsFound = new List<CoordinateTemplate>();
        foreach (var item in coords)
        {
            coordsFound.Add(item);
        }
        coordsFound.Add(new("Bulunduğun Konum", new(abs.X, abs.Y, abs.Z)));

        foreach (var k in coordsFound)
        {
            skzMenu.AddMenuOption(k.Text, (p, t) =>
            {
                Server.PrintToChatAll($"{Prefix} {CC.W}Mahkûmlar {CC.B}{k.Text} {CC.W} ışınlanıyor");

                SkzTimer?.Kill();
                Skz2Timer?.Kill();
                SkzV2FailedSteamIds?.Clear();
                var players = GetPlayers(CsTeam.Terrorist)
                    .Where(x => x.PawnIsAlive == true)
                    .ToList();

                players.ForEach(x =>
                {
                    SetMoveType(x, MoveType_t.MOVETYPE_OBSOLETE);
                    x.PlayerPawn.Value.Teleport(k.Coord, x.Pawn.Value.AbsRotation, new Vector(0f, 0f, 0f));
                });
                BasicCountdown.CommandStartTextCountDown(this, $"SKZ 3 SANİYE SONRA BAŞLIYOR");

                SkzTimer = AddTimer(3, () =>
                {
                    if (players.Count < 6)
                    {
                        Config.Additional.ParachuteModelEnabled = true;
                    }
                    else
                    {
                        Config.Additional.ParachuteModelEnabled = false;
                    }
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
                                ShowWeapons(x);
                            }
                            else
                            {
                                SetColour(x, Color.FromArgb(0, 0, 0, 0));
                                HideWeapons(x);
                            }
                        }
                        SetMoveType(x, MoveType_t.MOVETYPE_WALK);
                        RefreshPawnTP(x);
                    });
                    FreezeOrUnfreezeSound();
                    BasicCountdown.CommandStartTextCountDown(this, $"Mahkûmlar {value} saniye sonra donacak");
                }, SOM);

                Skz2Timer = AddTimer(value + 3, () =>
                {
                    var greenColor = 0;
                    var redColor = 0;
                    SkzV2FailedSteamIds?.Clear();
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
                            var res = PaintPlayersBasedOnTheirPos(x);
                            switch (res)
                            {
                                case 1:
                                    greenColor = greenColor + 1;
                                    break;

                                case -1:
                                    redColor = redColor + 1;
                                    break;
                            }
                        }
                        SetMoveType(x, MoveType_t.MOVETYPE_OBSOLETE);
                        RefreshPawnTP(x);
                    });

                    Config.Additional.ParachuteModelEnabled = true;

                    FreezeOrUnfreezeSound();
                    Server.PrintToChatAll($"{Prefix} {CC.Ol}{value}{CC.W} saniye süren {CC.Ol}SKZ{CC.W} bitti, {CC.G}mahkûmlar {CC.B}dondu{CC.W}.");
                    Server.PrintToChatAll($"{Prefix} {CC.G}{greenColor}{CC.W} adet kz'yi yapan {CC.R}{redColor}{CC.W} adet kz'yi yapamayan var{CC.W}.");

                    var str = $"{Prefix}<br>" +
                        $"<font color='#00FF00'>{greenColor}</font> adet kz'yi yapan<br>" +
                        $"<font color='#FF0000'>{redColor}</font> adet kz'yi yapamayan var.";
                    PrintToCenterHtmlAll(str);
                    PrintToCenterHtmlAll(str);
                    PrintToCenterHtmlAll(str);
                    PrintToCenterHtmlAll(str);
                    PrintToCenterHtmlAll(str);
                    PrintToCenterHtmlAll(str);
                    PrintToCenterHtmlAll(str);
                    PrintToCenterHtmlAll(str);
                    PrintToCenterHtmlAll(str);
                }, SOM);
            });
        }
        ChatMenus.OpenMenu(player, skzMenu);
    }

    #endregion SKZ
}