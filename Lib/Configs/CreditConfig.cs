using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class CreditConfig
    {
        [JsonPropertyName("SaveCreditTimerEveryXSecond")]
        public short SaveCreditTimerEveryXSecond { get; set; } = 60;

        [JsonPropertyName("RetrieveCreditEveryCTKill")]
        public short RetrieveCreditEveryCTKill { get; set; } = 5;

        [JsonPropertyName("RetrieveCreditEveryTKill")]
        public short RetrieveCreditEveryTKill { get; set; } = 1;

        [JsonPropertyName("RetrieveCreditEveryXMin")]
        public short RetrieveCreditEveryXMin { get; set; } = 5 * 60;

        [JsonPropertyName("RetrieveCreditEveryXMinReward")]
        public short RetrieveCreditEveryXMinReward { get; set; } = 5;

        [JsonPropertyName("RetrieveCreditEvery5MinRewardCssAdmin1")]
        public short RetrieveCreditEvery5MinRewardCssAdmin1 { get; set; } = 1;

        [JsonPropertyName("RetrieveCreditEvery5MinRewardCssLider")]
        public short RetrieveCreditEvery5MinRewardCssLider { get; set; } = 2;

        [JsonIgnore]
        public Dictionary<string, TPMarketItem> TPMarketDatas { get; set; } = new()
        {
            {"1000 TP | 5000 Kredi",new (5000, 1000)},
            {"2000 TP | 10000 Kredi",new (10000, 2000)},
            {"5000 TP | 25000 Kredi",new (25000, 5000)},
        };
    }
}