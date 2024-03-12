using CounterStrikeSharp.API;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private class GambleHistory
    {
        private int Red { get; set; }
        private int Green { get; set; }
        private int Black { get; set; }
        private RuletOptions Winner { get; set; }
        private int ParticipationCount { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
    private static SelfAdjustingQueue<GambleHistory> LastGambleDatas { get; set; } = new SelfAdjustingQueue<GambleHistory>(maxSize: 20);
}