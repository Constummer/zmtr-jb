using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void ListenersOnClientVoice()
    {
        RegisterListener((Listeners.OnClientVoice)((playerSlot) =>
        {
            //CCSPlayerController? player = Utilities.GetPlayerFromSlot(playerSlot);
            Console.WriteLine(playerSlot);
        }));
    }
}