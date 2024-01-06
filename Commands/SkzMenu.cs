using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private Dictionary<string, Vector> skzCoordinates = new()
    {
        {"Hücre",   new Vector(-535,345,-27) },
        {"KZ",      new Vector(2102,812,-357) },
    };

    private Vector Hucre = new Vector(-535, 345, -27);

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
        }
        var skzMenu = new ChatMenu("SKZ Menü");
        foreach (var k in skzCoordinates.ToList())
        {
            skzMenu.AddMenuOption(k.Key, (p, t) =>
            {
                Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.W}Mahkûmlar {CC.B}{k.Key} {CC.W} ışınlanıyor");
                BasicCountdown.CommandStartTextCountDown(this, $"[ZMTR] Mahkûmların donmasina {value} saniye");

                GetPlayers(CsTeam.Terrorist)
                    .Where(x => x.PawnIsAlive == true)
                    .ToList()
                    .ForEach(x =>
                    {
                        if (TeamActive == false)
                        {
                            SetColour(x, DefaultPlayerColor);
                        }
                        x.PlayerPawn.Value.MoveType = MoveType_t.MOVETYPE_WALK;

                        x.PlayerPawn.Value.Teleport(k.Value, new QAngle(0f, 0f, 0f), new Vector(0f, 0f, 0f));
                    });

                _ = AddTimer(value, () =>
                {
                    GetPlayers()
                    .Where(x => x != null
                         && x.PlayerPawn.IsValid
                         && x.PawnIsAlive
                         && x.IsValid
                         && x?.PlayerPawn?.Value != null
                         && GetTeam(x) == CsTeam.Terrorist)
                    .ToList()
                    .ForEach(x =>
                    {
                        if (TeamActive == false)
                        {
                            SetColour(x, Config.Burry.BuryColor);
                        }
                        x.PlayerPawn.Value!.MoveType = MoveType_t.MOVETYPE_OBSOLETE;
                        Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
                        Vector currentSpeed = new Vector(0, 0, 0);
                        QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
                        x.PlayerPawn.Value.Teleport(currentPosition, currentRotation, currentSpeed);
                    });
                    FreezeOrUnfreezeSound();
                    Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}mahkûmları {CC.B}dondurdu{CC.W}.");
                });
            });
        }
        ChatMenus.OpenMenu(player, skzMenu);
    }

    #endregion SKZ
}