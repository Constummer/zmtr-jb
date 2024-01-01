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
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR]{ChatColors.White} 0 = kapatmak icin, 1 = acmak icin.");
            return;
        }
        GetPlayers(CsTeam.Terrorist)
            .ToList()
            .ForEach(x =>
            {
                switch (godOneTwo)
                {
                    case 0:
                        SetColour(x, DefaultPlayerColor);
                        break;

                    case 1:
                        SetColour(x, Color.FromArgb(0, 0, 0, 0));
                        break;
                }
                RefreshPawn(x);
            });
    }

    #endregion Hide
}