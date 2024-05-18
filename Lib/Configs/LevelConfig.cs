using System.Text.Json.Serialization;

namespace JailbreakExtras.Lib.Configs
{
    public class LevelConfig
    {
        [JsonIgnore]
        public List<LevelGiftConfig> LevelGifts { get; set; } = new()
        {
            new (-1, 0,   "Seviye 0", null, "[NOOB]", null),
            new (1, 1,       "Seviye 1" , JailbreakExtras.Perm_Seviye1,"[Seviye 1]","olive tag sadece"),
            new (2, 1000,    "Seviye 2" , JailbreakExtras.Perm_Seviye2, "[Seviye 2]","revote"),
            new (3, 2000,    "Seviye 3" , JailbreakExtras.Perm_Seviye3, "[Seviye 3]","500 kredi",500),
            new (4, 3000,    "Seviye 4" , JailbreakExtras.Perm_Seviye4, "[Seviye 4]","freeze, beacon"),
            new (5, 5000,    "Seviye 5" , JailbreakExtras.Perm_Seviye5, "[Seviye 5]","500 kredi",500 ),
            new (6, 6000,    "Seviye 6" , JailbreakExtras.Perm_Seviye6, "[Seviye 6]","team, respawn"),
            new (7, 7000,    "Seviye 7" , JailbreakExtras.Perm_Seviye7, "[Seviye 7]","500 kredi",500 ),
            new (8, 8000,    "Seviye 8" , JailbreakExtras.Perm_Seviye8, "[Seviye 8]","hrespawn, 1up"),
            new (9, 9000,    "Seviye 9" , JailbreakExtras.Perm_Seviye9, "[Seviye 9]","bring, weapon"),
            new (10, 10000,    "Seviye 10" , JailbreakExtras.Perm_Seviye10, "[Seviye 10]","goto, slap, mute, gag, komal, komiptal"),
            new (11, 15000,    "Seviye 11" , JailbreakExtras.Perm_Seviye11, "[Seviye 11]","1000 kredi",1_000 ),
            new (12, 20000,    "Seviye 12" , JailbreakExtras.Perm_Seviye12, "[Seviye 12]","1000 kredi",1_000 ),
            new (13, 25000,    "Seviye 13" , JailbreakExtras.Perm_Seviye13, "[Seviye 13]","1000 kredi",1_000 ),
            new (14, 30000,    "Seviye 14" , JailbreakExtras.Perm_Seviye14, "[Seviye 14]","1000 kredi",1_000 ),
            new (15, 40000,    "Seviye 15" , JailbreakExtras.Perm_Seviye15, "[Seviye 15]","1000 kredi",1_000 ),
            new (16, 50000,    "Seviye 16" , JailbreakExtras.Perm_Seviye16, "[Seviye 16]","1000 kredi",1_000 ),
            new (17, 60000,    "Seviye 17" , JailbreakExtras.Perm_Seviye17, "[Seviye 17]","1000 kredi",1_000 ),
            new (18, 70000,    "Seviye 18" , JailbreakExtras.Perm_Seviye18, "[Seviye 18]","1000 kredi",1_000 ),
            new (19, 80000,    "Seviye 19" , JailbreakExtras.Perm_Seviye19, "[Seviye 19]","1000 kredi",1_000 ),
            new (20, 100000,    "Seviye 20" , JailbreakExtras.Perm_Seviye20, "[Seviye 20]","1000 kredi",1_000 ),
            new (21, 115000,    "Seviye 21" , JailbreakExtras.Perm_Seviye21, "[Seviye 21]","bi tane market modeli free"),
            new (22, 130000,    "Seviye 22" , JailbreakExtras.Perm_Seviye22, "[Seviye 22]","ffmenu, ffiptal"),
            new (23, 145000,    "Seviye 23" , JailbreakExtras.Perm_Seviye23, "[Seviye 23]","23000 kredi",23_000 ),
            new (24, 160000,    "Seviye 24" , JailbreakExtras.Perm_Seviye24, "[Seviye 24]","aura"),
            new (25, 175000,    "Seviye 25" , JailbreakExtras.Perm_Seviye25, "[Seviye 25]","ffdondur"),
            new (26, 190000,    "Seviye 26" , JailbreakExtras.Perm_Seviye26, "[Seviye 26]","vote, cancelvote"),
            new (27, 210000,    "Seviye 27" , JailbreakExtras.Perm_Seviye27, "[Seviye 27]","respawnac, respawnkapat"),
            new (28, 225000,    "Seviye 28" , JailbreakExtras.Perm_Seviye28, "[Seviye 28]","gravityac, gravitykapa"),
            new (29, 240000,    "Seviye 29" , JailbreakExtras.Perm_Seviye29, "[Seviye 29]","bunnyac, bunnykapa"),
            new (30, 300000,    "Seviye 30" , JailbreakExtras.Perm_Seviye30, "[Seviye 30]","ban, hook, god, 10k kredi",10_000 ),
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
        public string Permission { get; set; } = JailbreakExtras.Perm_Seviye1;

        [JsonIgnore]
        public string ClanTag { get; set; } = "[Seviye 1]";

        [JsonIgnore]
        public string Extra { get; set; } = "Yeşil renkte yazma, !ceza";

        public int CreditReward { get; } = 0;
    }
}