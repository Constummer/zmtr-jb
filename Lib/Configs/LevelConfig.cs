using System.Text.Json.Serialization;

namespace JailbreakExtras.Lib.Configs
{
    public class LevelConfig
    {
        [JsonIgnore]
        public List<LevelGiftConfig> LevelGifts { get; set; } = new()
        {
            new (-1, 0,   "Seviye 0", null, "[NOOB]", null),
            new (1, 1,       "Seviye 1" , "@css/seviye1","[Seviye 1]","olive tag sadece"),
            new (2, 1000,    "Seviye 2" , "@css/seviye2", "[Seviye 2]","revote"),
            new (3, 2000,    "Seviye 3" , "@css/seviye3", "[Seviye 3]","500 kredi",500),
            new (4, 3000,    "Seviye 4" , "@css/seviye4", "[Seviye 4]","freeze, beacon"),
            new (5, 5000,    "Seviye 5" , "@css/seviye5", "[Seviye 5]","500 kredi",500 ),
            new (6, 6000,    "Seviye 6" , "@css/seviye6", "[Seviye 6]","team, respawn"),
            new (7, 7000,    "Seviye 7" , "@css/seviye7", "[Seviye 7]","500 kredi",500 ),
            new (8, 8000,    "Seviye 8" , "@css/seviye8", "[Seviye 8]","hrespawn, 1up"),
            new (9, 9000,    "Seviye 9" , "@css/seviye9", "[Seviye 9]","bring, weapon"),
            new (10, 10000,    "Seviye 10" , "@css/seviye10", "[Seviye 10]","goto, slap, mute, gag, komal, komiptal"),
            new (11, 15000,    "Seviye 11" , "@css/seviye11", "[Seviye 11]","1000 kredi",1_000 ),
            new (12, 20000,    "Seviye 12" , "@css/seviye12", "[Seviye 12]","1000 kredi",1_000 ),
            new (13, 25000,    "Seviye 13" , "@css/seviye13", "[Seviye 13]","1000 kredi",1_000 ),
            new (14, 30000,    "Seviye 14" , "@css/seviye14", "[Seviye 14]","1000 kredi",1_000 ),
            new (15, 40000,    "Seviye 15" , "@css/seviye15", "[Seviye 15]","1000 kredi",1_000 ),
            new (16, 50000,    "Seviye 16" , "@css/seviye16", "[Seviye 16]","1000 kredi",1_000 ),
            new (17, 60000,    "Seviye 17" , "@css/seviye17", "[Seviye 17]","1000 kredi",1_000 ),
            new (18, 70000,    "Seviye 18" , "@css/seviye18", "[Seviye 18]","1000 kredi",1_000 ),
            new (19, 80000,    "Seviye 19" , "@css/seviye19", "[Seviye 19]","1000 kredi",1_000 ),
            new (20, 100000,    "Seviye 20" , "@css/seviye20", "[Seviye 20]","1000 kredi",1_000 ),
            new (21, 115000,    "Seviye 21" , "@css/seviye21", "[Seviye 21]","bi tane market modeli free"),
            new (22, 130000,    "Seviye 22" , "@css/seviye22", "[Seviye 22]","ffmenu, ffiptal"),
            new (23, 145000,    "Seviye 23" , "@css/seviye23", "[Seviye 23]","23000 kredi",23_000 ),
            new (24, 160000,    "Seviye 24" , "@css/seviye24", "[Seviye 24]","aura"),
            new (25, 175000,    "Seviye 25" , "@css/seviye25", "[Seviye 25]","ffdondur"),
            new (26, 190000,    "Seviye 26" , "@css/seviye26", "[Seviye 26]","vote, cancelvote"),
            new (27, 210000,    "Seviye 27" , "@css/seviye27", "[Seviye 27]","respawnac, respawnkapat"),
            new (28, 225000,    "Seviye 28" , "@css/seviye28", "[Seviye 28]","gravityac, gravitykapa"),
            new (29, 240000,    "Seviye 29" , "@css/seviye29", "[Seviye 29]","bunnyac, bunnykapa"),
            new (30, 300000,    "Seviye 30" , "@css/seviye30", "[Seviye 30]","ban, hook, god, 10k kredi",10_000 ),
        };
    }

    public class LevelGiftConfig
    {
        public LevelGiftConfig(int level, int xp, string levelName, string permission, string clanTag, string extra, int creditReward = 0)
        {
            Level = level;
            Xp = xp;
            LevelName = levelName;
            Permission = permission;
            ClanTag = clanTag;
            Extra = extra;
            CreditReward = creditReward;
        }

        [JsonIgnore]
        public int Level { get; set; } = 1;

        [JsonIgnore]
        public int Xp { get; set; } = 0;

        [JsonIgnore]
        public string LevelName { get; }

        [JsonIgnore]
        public string Permission { get; set; } = "@css/seviye1";

        [JsonIgnore]
        public string ClanTag { get; set; } = "[Seviye 1]";

        [JsonIgnore]
        public string Extra { get; set; } = "Yeşil renkte yazma, !ceza";

        public int CreditReward { get; } = 0;
    }
}