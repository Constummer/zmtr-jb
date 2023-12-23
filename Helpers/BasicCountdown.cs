using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;
using System.Text.RegularExpressions;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static float CountdownTime;
    private static string CountdownText = "";
    private static bool Countdown_enable_text;
    private static CounterStrikeSharp.API.Modules.Timers.Timer? timer_1;
    private static CounterStrikeSharp.API.Modules.Timers.Timer? timer_2;
    private static string Pattern = @"(\d+)\s*(saniye|sn|second|se|s.)";

    internal static class BasicCountdown
    {
        internal static bool CountdownEnableTextHandler(bool changed, CCSPlayerController client)
        {
            if (Countdown_enable_text)
            {
                if (CountdownText != null)
                {
                    var tempText = CountdownText;
                    try
                    {
                        if (changed == false)
                        {
                            Match match = Regex.Match(CountdownText, Pattern, RegexOptions.IgnoreCase);

                            if (match.Success)
                            {
                                CountdownText = Regex.Replace(CountdownText, Pattern, match =>
                                {
                                    return CountdownTime + " " + match.Groups[2].Value;
                                }, RegexOptions.IgnoreCase);
                            }

                            CountdownText = Regex.Replace(CountdownText, @"[#*<>]+\s*", "");

                            changed = true;
                        }
                    }
                    catch
                    {
                        CountdownText = tempText;
                    }

                    client.PrintToCenter($"{ChatColors.Green}{CountdownText}");
                    client.PrintToCenterHtml($"{ChatColors.Green}{CountdownText}");
                }
            }

            return changed;
        }

        internal static void CommandStartTextCountDown(JailbreakExtras jailbreakExtras, string text)
        {
            try
            {
                Match match = Regex.Match(text, Pattern, RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    // Mevcut bir zamanlayıcı varsa sonlandır
                    timer_2?.Kill();
                    var TimeSec = match.Groups[1].Value;

                    var time_convert = Convert.ToInt32(TimeSec);
                    CountdownTime = time_convert;
                    CountdownText = text;
                    Countdown_enable_text = true;

                    // Yeni bir zamanlayıcı ekle
                    timer_2 = jailbreakExtras.AddTimer(1.0f, () =>
                    {
                        if (CountdownTime == 0.0)
                        {
                            Countdown_enable_text = false;
                            timer_2?.Kill();
                            return;
                        }

                        CountdownTime = CountdownTime - 1.0f;
                    }, TimerFlags.REPEAT);
                }
            }
            catch
            {
            }
        }
    }
}