﻿using System.Text.Json.Serialization;

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

        [JsonPropertyName("RetrieveCreditEvery5MinRewardCssPremium")]
        public short RetrieveCreditEvery5MinRewardCssPremium { get; set; } = 3;

        [JsonPropertyName("HaftalikWCredit")]
        public short HaftalikWCredit { get; set; } = 3000;

        [JsonPropertyName("HaftalikKaCredit")]
        public short HaftalikKaCredit { get; set; } = 3000;

        [JsonPropertyName("HaftalikIsCredit")]
        public short HaftalikIsCredit { get; set; } = 3000;

        [JsonPropertyName("HaftalikTopCredit")]
        public Dictionary<string, short> HaftalikTopCredit { get; set; } = new() {
                    {"1",   2500},
                    {"2",   2250},
                    {"3",   2000},
                    {"4",   1750},
                    {"5",   1500},
                    {"6",   1250},
                    {"7",   1000},
                    {"8",   750},
                    {"9",   500},
                    {"10",  2500}};

        [JsonPropertyName("TPMarketDatas")]
        public List<TPMarketItem> TPMarketDatas { get; set; } = new()
        {
            {new ("1000 TP | 5000 Kredi",5000, 1000)},
            {new ("2000 TP | 10000 Kredi",10000, 2000)},
            {new ("5000 TP | 25000 Kredi",25000, 5000)},
        };
    }
}