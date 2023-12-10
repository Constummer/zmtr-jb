using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Timers;
using System.Text.RegularExpressions;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static float Time;
    private static string Text;
    private static bool Countdown_enable;
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
                if (Text != null)
                {
                    var tempText = Text;
                    try
                    {
                        if (changed == false)
                        {
                            Match match = Regex.Match(Text, Pattern, RegexOptions.IgnoreCase);

                            if (match.Success)
                            {
                                Text = Regex.Replace(Text, Pattern, match =>
                                {
                                    return Time + " " + match.Groups[2].Value;
                                }, RegexOptions.IgnoreCase);
                            }

                            Text = Regex.Replace(Text, @"[#*<>]+\s*", "");

                            changed = true;
                        }
                    }
                    catch
                    {
                        Text = tempText;
                    }

                    client.PrintToCenterHtml(
                        $"<font class='fontSize-m' color='#00FF00'>{Text} </font>"
                    );
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
                    Time = time_convert;
                    Text = text;
                    Countdown_enable_text = true;

                    // Yeni bir zamanlayıcı ekle
                    timer_2 = jailbreakExtras.AddTimer(1.0f, () =>
                    {
                        if (Time == 0.0)
                        {
                            Countdown_enable_text = false;
                            timer_2?.Kill();
                            return;
                        }

                        Time = Time - 1.0f;
                    }, TimerFlags.REPEAT);
                }
            }
            catch
            {
            }
        }
    }
}