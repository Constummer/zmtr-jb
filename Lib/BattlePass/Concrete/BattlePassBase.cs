using Newtonsoft.Json;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public partial class BattlePassBase
    {
        public BattlePassBase(int level, int time, int credit, int tP, string? other = null)
        {
            Level = level;
            Time = time * 60;
            Credit = credit;
            TP = tP;
            Other = other;
        }

        [JsonIgnore]
        public ulong SteamId { get; set; } = 0;

        [JsonIgnore]
        public int Level { get; set; } = 0;

        [JsonIgnore]
        public int Time { get; set; } = 0;

        [JsonIgnore]
        public int Credit { get; set; } = 0;

        [JsonIgnore]
        public int TP { get; set; } = 0;

        [JsonIgnore]
        public string Other { get; set; } = null;

        public int CurrentTime { get; set; } = 0;
        public bool Completed { get; set; } = false;
    }
}