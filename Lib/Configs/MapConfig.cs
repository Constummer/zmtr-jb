using System.Text.Json.Serialization;

namespace JailbreakExtras
{
    public class MapConfig
    {
        [JsonPropertyName("MapWorkshopIds")]
        public Dictionary<string, long> MapWorkshopIds { get; set; } = new Dictionary<string, long>()
        {
            {"jb_zmtr_v2remake", 3176846225 },
            {"jb_zmtr_triplex", 3170722498},
            {"jb_zmtr_ramazan", 3183895434},
            //{"jb_zmtr_minecraft_party ", 3168967727},
            //{"jb_zmtr_uzay ", 3164210735},
        };

        [JsonPropertyName("MapCellCoords")]
        public List<CoordinateTemplate> MapCellCoords { get; set; } = new List<CoordinateTemplate>()
        {
            new("jb_zmtr_v1" ,new VectorTemp(-535, 345, -27)),
        };

        [JsonPropertyName("ParamCoords")]
        public List<CoordinateTemplate> ParamCoords { get; set; } = new List<CoordinateTemplate>()
        {
            new ("jb_zmtr_v1" ,new VectorTemp(4497, 4261, -1880)),
        };

        [JsonPropertyName("SkzCoordinates")]
        public Dictionary<string, List<CoordinateTemplate>> SkzCoordinates { get; set; } = new Dictionary<string, List<CoordinateTemplate>>()
        {
            {"jb_zmtr_v1", new List<CoordinateTemplate>()
                {
                    new("Hucre",   new VectorTemp(-535,345,-27) ),
                    new("KZ",      new VectorTemp(2102,739,-357) ),
                }
            },
            {"jb_zmtr_v2remake", new List<CoordinateTemplate>()
                {
                    new("Hucre",   new VectorTemp(-535,345,-27) ),
                    new("KZ",      new VectorTemp(2496,-627,-150) ),
                    new("KZ Sag Kulvar",  new VectorTemp(2841,-531,-109) ),
                    new("KZ Sol Kulvar",  new VectorTemp(2139,-532,-109) ),
                }
            },
            {"jb_zmtr_uzay", new List<CoordinateTemplate>()
                {
                    new("Hucre",   new VectorTemp(-1860,-153,-269) ),
                    new("KZ",      new VectorTemp(-1320,-2900,-543) ),
                    new("KZ Sag Kulvar",  new VectorTemp(-1185,-3252,-501) ),
                    new("KZ Sol Kulvar",  new VectorTemp(-1183,-2549,-501) ),
                }
            },
            {"jb_zmtr_minecraft_party", new List<CoordinateTemplate>()
                {
                    new("KZ",      new VectorTemp(-2788,2582,675) ),
                    new("KZ Sag Kulvar",  new VectorTemp(-2434,2720,716) ),
                    new("KZ Sol Kulvar",  new VectorTemp(-3142,2720,716) ),
                }
            },
            {"jb_zmtr_triplex", new List<CoordinateTemplate>()
                {
                    new("Hucre",   new VectorTemp(210,3115,-88) ),
                    new("KZ",      new VectorTemp(-472,-668,-380) ),
                    new("KZ Sag Kulvar",  new VectorTemp(-606,-315,-337) ),
                    new("KZ Sol Kulvar",  new VectorTemp(-606,-1019,-337) ),
                }
            },
            {"jb_zmtr_ramazan", new List<CoordinateTemplate>()
                {
                    new("Hucre",   new VectorTemp(-2478,-18,61) ),
                    new("KZ",      new VectorTemp(1714,589,-151) ),
                    new("KZ Sag Kulvar",  new VectorTemp(2064,817,-106) ),
                    new("KZ Sol Kulvar",  new VectorTemp(1356,817,-106) ),
                }
            },
        };

        [JsonPropertyName("KapiAcKapaList")]
        public Dictionary<string, Dictionary<string, string>> KapiAcKapaList { get; set; } = new Dictionary<string, Dictionary<string, string>>()
        {
             {"jb_zmtr_v1",new Dictionary<string, string>()
                 {
                    {"kacak", "func_door" },
                    {"kapi2", "func_door" },
                 }
            },
        };

        [JsonPropertyName("ForceOpenDoor")]
        public Dictionary<string, string> ForceOpenDoor { get; set; } = new Dictionary<string, string>()
        {
             {"jb_zmtr_v1","kapi2"},
        };

        [JsonPropertyName("CoinCoords")]
        public List<CoordinateTemplate> CoinCoords { get; set; } = new List<CoordinateTemplate>()
        {
            new ("jb_zmtr_v1" ,new VectorTemp(-718, -765, 24)),
            new ("jb_zmtr_v2remake" ,new VectorTemp(62, -408, 171)),
            new ("jb_zmtr_uzay" ,new VectorTemp(-581, -3488, -544)),
            new ("jb_zmtr_minecraft_party" ,new VectorTemp(1,1,1)),
            new ("jb_zmtr_triplex" ,new VectorTemp(368, 3618, -60)),
            new ("jb_zmtr_ramazan" ,new VectorTemp(-874, 1582, 60))
        };

        [JsonPropertyName("KzCellCoords")]
        public Dictionary<string, List<CoordinateTemplate>> KzCellCoords { get; set; } = new Dictionary<string, List<CoordinateTemplate>>()
        {
             {"jb_zmtr_v1",
                new()
                {
                    new("LeftBottom", new VectorTemp(1969,1316,-210)),
                    new("RightTop",   new VectorTemp(2222,1505,-13))
                }
             },
             {"jb_zmtr_uzay",
                new()
                {
                    new("LeftBottom", new VectorTemp(-702,-3025,-288)),
                    new("RightTop",   new VectorTemp(-514,-2773,-104))
                }
             },
             {"jb_zmtr_v2remake",
                new()
                {
                    new("LeftBottom", new VectorTemp(2364,-50,107)),
                    new("RightTop",   new VectorTemp(2620,140,232))
                }
             },
             {"jb_zmtr_minecraft_party",
                new()
                {
                    new("LeftBottom", new VectorTemp(-2899,3200,926)),
                    new("RightTop",   new VectorTemp(-2676,3375,1056))
                }
             },
             { "jb_zmtr_triplex",
                new()
                {
                    new("LeftBottom", new VectorTemp(-1087,-795,-124)),
                    new("RightTop",   new VectorTemp(-1274,-540,30))
                }
             },
             { "jb_zmtr_ramazan",
                new()
                {
                    new("LeftBottom", new VectorTemp(1583,1295,106)),
                    new("RightTop",   new VectorTemp(1814,1590,279))
                }
             },
            };
    }
}