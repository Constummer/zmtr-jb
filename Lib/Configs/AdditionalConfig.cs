﻿using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class AdditionalConfig
    {
        [JsonPropertyName("KomPermName")]
        public string KomPermName { get; set; } = Perm_Komutcu;

        [JsonPropertyName("KomWeeklyCredit")]
        public int KomWeeklyCredit { get; set; } = 1000;

        [JsonPropertyName("BayramCredit")]
        public int BayramCredit { get; set; } = 500;

        [JsonPropertyName("BayramCreditStart")]
        public DateTime BayramCreditStart { get; set; } = new DateTime(2024, 10, 10);

        [JsonPropertyName("BayramCreditEnd")]
        public DateTime BayramCreditEnd { get; set; } = new DateTime(2024, 10, 12);

        [JsonPropertyName("ParachuteEnabled")]
        public bool ParachuteEnabled { get; set; } = true;

        [JsonPropertyName("ParachuteModelEnabled")]
        public bool ParachuteModelEnabled { get; set; } = true;

        [JsonPropertyName("HideFootsOnConnect")]
        public bool HideFootsOnConnect { get; set; } = true;

        [JsonPropertyName("CustomRespawnEnabled")]
        public bool CustomRespawnEnabled { get; set; } = true;

        [JsonPropertyName("GrappleSpeed")]
        public float GrappleSpeed { get; set; } = 1500.0f;

        [JsonPropertyName("UnlimitedReserver")]
        public bool UnlimitedReserver { get; set; } = false;

        [JsonPropertyName("NoBlockActive")]
        public bool NoBlockActive { get; set; } = true;

        [JsonPropertyName("BattlePassActive")]
        public bool BattlePassActive { get; set; } = true;

        [JsonPropertyName("BattlePassPremiumActive")]
        public bool BattlePassPremiumActive { get; set; } = true;

        [JsonPropertyName("TimeRewardActive")]
        public bool TimeRewardActive { get; set; } = true;

        [JsonPropertyName("WelcomeActive")]
        public bool WelcomeActive { get; set; } = true;

        [JsonPropertyName("HideMsg")]
        public bool HideMsg { get; set; } = true;

        [JsonPropertyName("CitMaxCount")]
        public int CitMaxCount { get; set; } = 128;

        [JsonPropertyName("PluginExceptionWebHook")]
        public string PluginExceptionWebHook { get; set; } = "https://discord.com/api/webhooks/1241871721234038874/fh81QDnbz0o8Pes8DHASNoQpgvGC9kbr0Wuh9hhhyeGNGl0MW4nlmnH-4CyIUlehnyr3";

        [JsonPropertyName("RoundEndStartCommands")]
        public List<string> RoundEndStartCommands { get; set; } = new(){
            "mp_respawn_on_death_t 0",
            "mp_respawn_on_death_ct 0",
            "sv_enablebunnyhopping 1",
            "sv_autobunnyhopping 1",
            "sv_maxspeed 320",
            "mp_teammates_are_enemies 0",
            "sv_gravity 800"
        };
    }
}