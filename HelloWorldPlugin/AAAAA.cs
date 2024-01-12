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
        RegisterEventHandler((GameEventHandler<EventPlayerSpawn>)((@event, _) =>
        {
            if (@event == null) return HookResult.Continue;
            if (@event.Userid == null) return HookResult.Continue;
            if (@event.Userid.IsValid == false) return HookResult.Continue;
            if (@event.Userid.UserId < 0) return HookResult.Continue;
            if (@event.Userid.SteamID < 0) return HookResult.Continue;
            var player = @event.Userid;
            if (IsValid(player))
            {
                if ((CsTeam)player.TeamNum == CsTeam.CounterTerrorist)
                {
                    if (IsValid(player))
                    {
                        player.GiveNamedItem("item_assaultsuit");
                        player.GiveNamedItem("weapon_deagle");
                        player.GiveNamedItem("weapon_m4a1");
                    }
                }
                if (IsValid(player))
                {
                    SetColour(@event.Userid, DefaultColor);
                }
            }
            return HookResult.Continue;
        }));
        RegisterEventHandler<EventPlayerDeath>((@event, info) =>
        {
            if (@event == null) return HookResult.Continue;
            if (@event.Userid == null) return HookResult.Continue;
            if (@event.Userid.IsValid == false) return HookResult.Continue;
            if (@event.Userid.UserId < 0) return HookResult.Continue;
            if (@event.Userid.SteamID < 0) return HookResult.Continue;
            if (IsValid(@event.Userid))
            {
                SetColour(@event.Userid, Color.FromArgb(0, 0, 0, 0));
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

    private static bool IsValid(CCSPlayerController? player)
    {
        return player != null && player.IsValid && player.PlayerPawn.IsValid;
    }
}