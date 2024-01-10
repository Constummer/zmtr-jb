using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Hide

    [ConsoleCommand("hide")]
    [CommandHelper(1, "<0/1>")]
    public void Hide(CCSPlayerController? player, CommandInfo info)
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
        int.TryParse(target, out var godOneTwo);
        if (godOneTwo < 0 || godOneTwo > 1)
        {
            player.PrintToChat($"{Prefix}{CC.W} 0 = kapatmak icin, 1 = acmak icin.");
            return;
        }
        GetPlayers(CsTeam.Terrorist)
            .ToList()
            .ForEach(x =>
            {
                switch (godOneTwo)
                {
                    case 0:
                        Config.Additional.ParachuteModelEnabled = true;
                        SetColour(x, DefaultColor);
                        ShowWeapons(x);
                        break;

                    case 1:
                        Config.Additional.ParachuteModelEnabled = false;
                        SetColour(x, Color.FromArgb(0, 0, 0, 0));
                        HideWeapons(x);
                        break;
                }
                RefreshPawn(x);
            });
    }

    //public enum RenderMode_t : byte
    //{
    //    kRenderNormal = 0x0,
    //    kRenderTransColor = 0x1,
    //    kRenderTransTexture = 0x2,
    //    kRenderGlow = 0x3,
    //    kRenderTransAlpha = 0x4,
    //    kRenderTransAdd = 0x5,
    //    kRenderEnvironmental = 0x6,
    //    kRenderTransAddFrameBlend = 0x7,
    //    kRenderTransAlphaAdd = 0x8,
    //    kRenderWorldGlow = 0x9,
    //    kRenderNone = 0xA,
    //    kRenderDevVisualizer = 0xB,
    //    kRenderModeCount = 0xC,
    //}

    #endregion Hide
}