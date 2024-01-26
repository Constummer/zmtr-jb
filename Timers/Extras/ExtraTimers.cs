using CounterStrikeSharp.API;
using System.Diagnostics;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private CounterStrikeSharp.API.Modules.Timers.Timer DailyRestartTimer()
    {
        return AddTimer(300f, () =>
        {
            var nowTime = TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(3));

            if (nowTime >= MaintenanceGameTimer && nowTime <= MaintenanceTimer2)
            {
                KumarKapatDisable = true;
                Server.PrintToChatAll($"{Prefix} BAKIM GEREĞİ KUMAR - PIYANGO KAPATILMISTIR, SAAT 7.00 DA RES GELECEKTIR !!!!");
                Server.PrintToChatAll($"{Prefix} BAKIM GEREĞİ KUMAR - PIYANGO KAPATILMISTIR, SAAT 7.00 DA RES GELECEKTIR !!!!");
                Server.PrintToChatAll($"{Prefix} BAKIM GEREĞİ KUMAR - PIYANGO KAPATILMISTIR, SAAT 7.00 DA RES GELECEKTIR !!!!");
                Server.PrintToChatAll($"{Prefix} BAKIM GEREĞİ KUMAR - PIYANGO KAPATILMISTIR, SAAT 7.00 DA RES GELECEKTIR !!!!");
                Server.PrintToChatAll($"{Prefix} BAKIM GEREĞİ KUMAR - PIYANGO KAPATILMISTIR, SAAT 7.00 DA RES GELECEKTIR !!!!");
            }
            if (nowTime >= MaintenanceTimer && nowTime <= MaintenanceTimer2)
            {
                Server.PrintToChatAll($"{Prefix} BAKIM GEREĞİ SERVERE 07.00 DA RES GELECEKTİR !!!!");
                Server.PrintToChatAll($"{Prefix} BAKIM GEREĞİ SERVERE 07.00 DA RES GELECEKTİR !!!!");
                Server.PrintToChatAll($"{Prefix} BAKIM GEREĞİ SERVERE 07.00 DA RES GELECEKTİR !!!!");
                Server.PrintToChatAll($"{Prefix} BAKIM GEREĞİ SERVERE 07.00 DA RES GELECEKTİR !!!!");
                Server.PrintToChatAll($"{Prefix} BAKIM GEREĞİ SERVERE 07.00 DA RES GELECEKTİR !!!!");
            }
        }, Full);
    }

    private CounterStrikeSharp.API.Modules.Timers.Timer DcWardenNotifyTimer()
    {
        return AddTimer(300f, () =>
        {
            SendDcNotifyOnWardenChange();
        }, Full);
    }
}