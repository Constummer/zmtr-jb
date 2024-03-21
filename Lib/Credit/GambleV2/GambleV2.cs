using CounterStrikeSharp.API;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private class GambleHistory
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Black { get; set; }
        public RuletOptions Winner { get; set; }
        public int ParticipationCount { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }

    private static SelfAdjustingQueue<GambleHistory> LastGambleDatas { get; set; } = new SelfAdjustingQueue<GambleHistory>(maxSize: 20);
    private static Dictionary<ulong, RuletData> RuletPlayers = new();

    private class RuletData
    {
        public int Credit { get; set; }
        public RuletOptions Option { get; set; }
    }

    internal enum RuletOptions
    {
        None = 0,
        Yesil,
        Siyah,
        Kirmizi
    }
}

internal static class RuletChatShort
{
    internal static string GetShort(this JailbreakExtras.RuletOptions opt) => opt switch
    {
        JailbreakExtras.RuletOptions.Yesil => "Y",
        JailbreakExtras.RuletOptions.Siyah => "S",
        JailbreakExtras.RuletOptions.Kirmizi => "K",
        _ => ""
    };
}