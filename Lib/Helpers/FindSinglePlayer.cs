using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private bool FindSinglePlayer(CCSPlayerController player, string target,
        out CCSPlayerController result, Predicate<CCSPlayerController>? predicate = null)
    {
        result = null;
        var players = GetPlayers()
           .Where(x => GetTargetAction(x, target, player));
        if (predicate != null)
        {
            players = players.Where(x => predicate(x));
        }
        if (players.Count() == 0)
        {
            player.PrintToChat($"{Prefix} {CC.W}Eşleşen oyuncu bulunamadı!");
            return false;
        }
        if (players.Count() != 1)
        {
            player.PrintToChat($"{Prefix} {CC.W}Birden fazla oyuncu bulundu.");
            return false;
        }
        result = players.FirstOrDefault();
        if (ValidateCallerPlayer(result, false) == false)
        {
            return false;
        }
        return true;
    }
}