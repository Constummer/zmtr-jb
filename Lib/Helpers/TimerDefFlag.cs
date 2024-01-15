using CounterStrikeSharp.API.Modules.Timers;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public const TimerFlags SOM = TimerFlags.STOP_ON_MAPCHANGE;
    public const TimerFlags Full = TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE;
}