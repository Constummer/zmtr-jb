using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static bool HideActive = false;

    [ConsoleCommand("hidet")]
    [ConsoleCommand("thide")]
    [CommandHelper(1, "<0/1>")]
    public void Hidet(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var target = info.ArgString.GetArg(0);
        int.TryParse(target, out var godOneTwo);
        if (godOneTwo < 0 || godOneTwo > 1)
        {
            player.PrintToChat($"{Prefix}{CC.W} 0 = kapatmak icin, 1 = acmak icin.");
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        GetPlayers(CsTeam.Terrorist)
            .ToList()
            .ForEach(x =>
            {
                switch (godOneTwo)
                {
                    case 0:
                        if (ValidateCallerPlayer(x, false) == false) return;
                        ShowPlayer(x);
                        break;

                    case 1:
                        if (ValidateCallerPlayer(x, false) == false) return;
                        HidePlayer(x);
                        break;
                }
            });
        switch (godOneTwo)
        {
            case 0:
                HideActive = false;
                Config.Additional.ParachuteModelEnabled = true;
                Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.W} {T_PluralCamel} için hide kapadı.");
                if (Config.Additional.HideMsg)
                {
                    Server.PrintToChatAll($"{Prefix} {CC.R} Kişisel hide geldi. {CC.B}!hide {CC.R} yazarak açıp kapatabilirsiniz..");
                }
                Server.ExecuteCommand($"sv_teamid_overhead_maxdist 2000");
                break;

            case 1:
                HideActive = true;
                Config.Additional.ParachuteModelEnabled = false;
                Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.W} {T_PluralCamel} için hide açtı.");
                if (Config.Additional.HideMsg)
                {
                    Server.PrintToChatAll($"{Prefix} {CC.R} Kişisel hide geldi. {CC.B}!hide {CC.R} yazarak açıp kapatabilirsiniz..");
                }
                Server.ExecuteCommand($"sv_teamid_overhead_maxdist 1");
                break;
        }
    }

    private void ShowPlayer(CCSPlayerController x)
    {
        SetColour(x, DefaultColor);
        ShowWeapons(x);
        ReAddAllParticleAures();
    }

    private void HidePlayer(CCSPlayerController x)
    {
        SetColour(x, Color.FromArgb(0, 0, 0, 0));
        HideWeapons(x);
        RemoveAllParticleAures();
    }
}