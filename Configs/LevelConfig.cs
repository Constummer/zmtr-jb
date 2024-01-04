using System.Text.Json.Serialization;

namespace JailbreakExtras
{
    public class LevelConfig
    {
        [JsonPropertyName("Level")]
        public List<LevelGiftConfig> LevelGifts { get; set; } = new()
        {
            new (-1, 0,   null, null, null, null),
            new (1, 1,       "Seviye 1" , "@css/seviye1","[S1]","olive tag sadece"),
            new (2, 1000,    "Seviye 2" , "@css/seviye2", "[S2]","revote"),
            new (3, 2000,    "Seviye 3" , "@css/seviye3", "[S3]","500 kredi"),
            new (4, 3000,    "Seviye 4" , "@css/seviye4", "[S4]","freeze, beacon"),
            new (5, 5000,    "Seviye 5" , "@css/seviye5", "[S5]","500 kredi"),
            new (6, 6000,    "Seviye 6" , "@css/seviye6", "[S6]","team, respawn"),
            new (7, 7000,    "Seviye 7" , "@css/seviye7", "[S7]","500 kredi"),
            new (8, 8000,    "Seviye 8" , "@css/seviye8", "[S8]","hrespawn, 1up"),
            new (9, 9000,    "Seviye 9" , "@css/seviye9", "[S9]","bring, weapon"),
            new (10, 10000,    "Seviye 10" , "@css/seviye10", "[S10]","goto, slap, mute, gag, komal, komiptal"),
            new (11, 15000,    "Seviye 11" , "@css/seviye11", "[S11]","1000 kredi"),
            new (12, 20000,    "Seviye 12" , "@css/seviye12", "[S12]","1000 kredi"),
            new (13, 25000,    "Seviye 13" , "@css/seviye13", "[S13]","1000 kredi"),
            new (14, 30000,    "Seviye 14" , "@css/seviye14", "[S14]","1000 kredi"),
            new (15, 40000,    "Seviye 15" , "@css/seviye15", "[S15]","1000 kredi"),
            new (16, 50000,    "Seviye 16" , "@css/seviye16", "[S16]","1000 kredi"),
            new (17, 60000,    "Seviye 17" , "@css/seviye17", "[S17]","1000 kredi"),
            new (18, 70000,    "Seviye 18" , "@css/seviye18", "[S18]","1000 kredi"),
            new (19, 80000,    "Seviye 19" , "@css/seviye19", "[S19]","1000 kredi"),
            new (20, 100000,    "Seviye 20" , "@css/seviye20", "[S20]","1000 kredi"),
            new (21, 115000,    "Seviye 21" , "@css/seviye21", "[S21]","bi tane market modeli free"),
            new (22, 130000,    "Seviye 22" , "@css/seviye22", "[S22]","ffmenu, ffiptal"),
            new (23, 145000,    "Seviye 23" , "@css/seviye23", "[S23]","23000 kredi"),
            new (24, 160000,    "Seviye 24" , "@css/seviye24", "[S24]","aura"),
            new (25, 175000,    "Seviye 25" , "@css/seviye25", "[S25]","ffdondur"),
            new (26, 190000,    "Seviye 26" , "@css/seviye26", "[S26]","vote, cancelvote"),
            new (27, 210000,    "Seviye 27" , "@css/seviye27", "[S27]","respawnac, respawnkapat"),
            new (28, 225000,    "Seviye 28" , "@css/seviye28", "[S28]","gravityac, gravitykapa"),
            new (29, 240000,    "Seviye 29" , "@css/seviye29", "[S29]","bunnyac, bunnykapa"),
            new (30, 300000,    "Seviye 30" , "@css/seviye30", "[S30]","ban, hook, god, 10k kredi"),
        };
    }

    public class LevelGiftConfig
    {
        public LevelGiftConfig(int level, int xp, string levelName, string permission, string clanTag, string extra)
        {
            Level = level;
            Xp = xp;
            LevelName = levelName;
            Permission = permission;
            ClanTag = clanTag;
            Extra = extra;
        }

        [JsonPropertyName("Level")]
        public int Level { get; set; } = 1;

        [JsonPropertyName("Xp")]
        public int Xp { get; set; } = 0;

        [JsonPropertyName("LevelName")]
        public string LevelName { get; }

        [JsonPropertyName("Permission")]
        public string Permission { get; set; } = "@css/seviye1";

        [JsonPropertyName("ClanTag")]
        public string ClanTag { get; set; } = "[S1]";

        [JsonPropertyName("Extra")]
        public string Extra { get; set; } = "Yeşil renkte yazma, !ceza";
    }
}