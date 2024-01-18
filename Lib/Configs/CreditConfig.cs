using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class CreditConfig
    {
        [JsonPropertyName("RetrieveCreditEveryCTKill")]
        public short RetrieveCreditEveryCTKill { get; set; } = 5;

        [JsonPropertyName("RetrieveCreditEveryTKill")]
        public short RetrieveCreditEveryTKill { get; set; } = 1;

        [JsonPropertyName("RetrieveCreditEveryXMinReward")]
        public short RetrieveCreditEveryXMinReward { get; set; } = 5;

        [JsonPropertyName("RetrieveCreditEvery5MinRewardCssAdmin1")]
        public short RetrieveCreditEvery5MinRewardCssAdmin1 { get; set; } = 1;

        [JsonPropertyName("RetrieveCreditEvery5MinRewardCssLider")]
        public short RetrieveCreditEvery5MinRewardCssLider { get; set; } = 2;

        [JsonPropertyName("TPMarketDatas")]
        public List<TPMarketItem> TPMarketDatas { get; set; } = new()
        {
            {new ("1000 TP | 5000 Kredi",5000, 1000)},
            {new ("2000 TP | 10000 Kredi",10000, 2000)},
            {new ("5000 TP | 25000 Kredi",25000, 5000)},
        };
    }
}