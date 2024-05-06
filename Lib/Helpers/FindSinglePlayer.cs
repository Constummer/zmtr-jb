using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private bool FindSinglePlayer(CCSPlayerController player, string target, out CCSPlayerController? result)
    {
        result = null;
        var players = GetPlayers()
           .Where(x => GetTargetAction(x, target, player));
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
        return true;
    }
}