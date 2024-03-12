using CounterStrikeSharp.API;
using System.Net;

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
            if (nowTime >= OpenKumarTimer && nowTime <= OpenKumarTimer2)
            {
                KumarKapatDisable = false;
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

    private CounterStrikeSharp.API.Modules.Timers.Timer CheckPublicIpTimer()
    {
        return AddTimer(1_800f, () =>
        {
            Checker();
        }, Full);
    }

    private void Checker()
    {
        string[] whitelist = {
                "185.171.25.27",
                "111.111.111.111",//mami buraya ekle
                "185.118.141.74"
                    };
        var publicIpAddress = new WebClient().DownloadString("http://icanhazip.com").Trim();

        if (Array.IndexOf(whitelist, publicIpAddress) == -1)
        {
            Unload(false);
            SpamNewIPTimer();
            Environment.Exit(0);
        }
    }
}