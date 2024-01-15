using CounterStrikeSharp.API;
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
                        if (ValidateCallerPlayer(x, false) == false) return;
                        SetColour(x, DefaultColor);
                        ShowWeapons(x);

                        break;

                    case 1:
                        if (ValidateCallerPlayer(x, false) == false) return;
                        SetColour(x, Color.FromArgb(0, 0, 0, 0));
                        HideWeapons(x);
                        break;
                }
            });
        switch (godOneTwo)
        {
            case 0:
                Config.Additional.ParachuteModelEnabled = true;
                Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.W} Mahkûmlar için hide kapadı.");
                Server.ExecuteCommand($"sv_teamid_overhead_maxdist 2000");
                break;

            case 1:
                Config.Additional.ParachuteModelEnabled = false;
                Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.W} Mahkûmlar için hide açtı.");
                Server.ExecuteCommand($"sv_teamid_overhead_maxdist 1");
                break;
        }
    }

    #endregion Hide
}