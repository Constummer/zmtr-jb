using CounterStrikeSharp.API.Modules.Timers;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public static TimerFlags? SOM = (TimerFlags?)null/* TimerFlags.STOP_ON_MAPCHANGE*/;
    //public static TimerFlags? SOM = TimerFlags.STOP_ON_MAPCHANGE;

    public static TimerFlags? Full = TimerFlags.REPEAT /*| TimerFlags.STOP_ON_MAPCHANGE*/;
    //public static TimerFlags? Full = TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE;
}