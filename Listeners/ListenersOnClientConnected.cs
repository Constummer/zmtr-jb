using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void ListenersOnClientConnected()
    {
        //var queue = new Queue<ulong>();
        RegisterListener<Listeners.OnClientConnected>(playerSlot =>
        {
            uint finalSlot = (uint)playerSlot + 1;
            CCSPlayerController player = new CCSPlayerController(NativeAPI.GetEntityFromIndex((int)finalSlot));
            if (player == null || player.UserId < 0)
                return;

            if (player?.SteamID != null && player!.SteamID != 0)
            {
                Task.Run(async () =>
                {
                    await GetPlayerMarketData(player!.SteamID);
                });
            }
            //queue.Enqueue(finalSlot);
        });
        //AddTimer(10f, () =>
        //{
        //    Console.WriteLine("1");
        //    if (queue.TryDequeue(out var result))
        //    {
        //        var res = result;
        //        Console.WriteLine("2 = " + res);

        //        var p = Utilities.GetPlayerFromSteamId(res);
        //        Console.WriteLine("3 = " + res);

        //        if (ValidateCallerPlayer(p, false) == true && p.PawnIsAlive == false
        //                 && p.Connected == PlayerConnectedState.PlayerConnected)
        //        {
        //            p.SwitchTeam(CounterStrikeSharp.API.Modules.Utils.CsTeam.Terrorist);
        //            Console.WriteLine("4 = " + res);

        //            CustomRespawn(p);
        //        }
        //    };
        //}, CounterStrikeSharp.API.Modules.Timers.TimerFlags.REPEAT);
    }
}