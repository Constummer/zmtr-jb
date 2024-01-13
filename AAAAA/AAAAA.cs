using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using System.Drawing;
using System.Numerics;

namespace AAAAA;

public class AAAAA : BasePlugin
{
    public override string ModuleName => "AAAAA";

    public override string ModuleVersion => "0.0.1";
    public override string ModuleAuthor => "Constummer";
    public override string ModuleDescription => "AAAAA";
    private static Color DefaultColor = Color.FromArgb(255, 255, 255, 255);

    public override void Load(bool hotReload)
    {
        RegisterEventHandler<EventPlayerSpawn>((@event, _) =>
        {
            CCSPlayerController? player = @event.Userid;

            if (player != null && IsValid(player))
            {
                if (IsValid(player))
                {
                    SetColour(player, DefaultColor);
                }
            }
            return HookResult.Continue;
        });
        RegisterEventHandler<EventPlayerDeath>((@event, info) =>
        {
            CCSPlayerController? player = @event.Userid;

            if (player != null && IsValid(player))
            {
                SetColour(player, Color.FromArgb(0, 0, 0, 0));
            }
            return HookResult.Continue;
        }, HookMode.Post);
    }

    private static void SetColour(CCSPlayerController? player, Color colour)
    {
        if (player == null || !IsValid(player))
        {
            return;
        }

        CCSPlayerPawn? pawn = player.PlayerPawn.Value;

        if (pawn != null)
        {
            pawn.RenderMode = RenderMode_t.kRenderTransColor;
            pawn.Render = colour;
        }
    }

    public static bool IsValid(CCSPlayerController? player)
    {
        return player != null && player.IsValid && player.PlayerPawn.IsValid;
    }
}