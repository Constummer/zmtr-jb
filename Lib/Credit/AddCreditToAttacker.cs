using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void AddCreditToAttacker(CCSPlayerController? attacker, CsTeam victimTeamNo)
    {
        if (ValidateCallerPlayer(attacker, false) == false)
        {
            return;
        }
        if (attacker?.SteamID != null && attacker.SteamID != 0)
        {
            var amount = victimTeamNo switch
            {
                CsTeam.Terrorist => Config.Credit.RetrieveCreditEveryTKill,
                CsTeam.CounterTerrorist => Config.Credit.RetrieveCreditEveryCTKill,
                _ => 0
            };
            if (amount <= 0)
            {
                return;
            }
            if (PlayerMarketModels.TryGetValue(attacker!.SteamID, out var item))
            {
                item.Credit += amount;
            }
            else
            {
                item = new(attacker!.SteamID);
                item.Credit = amount;
            }
            PlayerMarketModels[attacker!.SteamID] = item;

            var teamShortName = victimTeamNo switch
            {
                CsTeam.Terrorist => "T",
                CsTeam.CounterTerrorist => "CT",
                _ => ""
            };
            attacker!.PrintToChat($"{Prefix} {CC.LB}{teamShortName} Oldurdugun için, {amount} kredi kazandın!");
        }
    }
}