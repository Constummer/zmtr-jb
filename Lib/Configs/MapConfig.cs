﻿using System.Text.Json.Serialization;
using static JailbreakExtras.JailbreakExtras;

namespace JailbreakExtras
{
    public class MapConfig
    {
        [JsonPropertyName("MapCellCoords")]
        public List<CoordinateTemplate> MapCellCoords { get; set; } = new List<CoordinateTemplate>()
        {
            new("jb_zmtr_v1" ,new VectorTemp(-535, 345, -27)),
            new("jb_zmtr_v2" ,new VectorTemp(-535, 345, -27))
        };

        [JsonPropertyName("ParamCoords")]
        public List<CoordinateTemplate> ParamCoords { get; set; } = new List<CoordinateTemplate>()
        {
            new ("jb_zmtr_v1" ,new VectorTemp(4497, 4261, -1880)),
            new ("jb_zmtr_v2" ,new VectorTemp(-535, 345, -27))
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
            {"jb_zmtr_v2", new List<CoordinateTemplate>()
                {
                    new("Hucre",   new VectorTemp(-535,345,-27) ),
                    new("KZ",      new VectorTemp(2102,739,-357) ),
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
            {"jb_zmtr_v2",new Dictionary<string, string>()
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
             {"jb_zmtr_v2","kapi"},
        };

        [JsonPropertyName("CoinCoords")]
        public List<CoordinateTemplate> CoinCoords { get; set; } = new List<CoordinateTemplate>()
        {
            new ("jb_zmtr_v1" ,new VectorTemp(-718, -765, 24)),
            new ("jb_zmtr_v2" ,new VectorTemp(62, -408, 171))
        };

        [JsonPropertyName("KzCellCoords")]
        public Dictionary<string, List<CoordinateTemplate>> KzCellCoords { get; set; } = new Dictionary<string, List<CoordinateTemplate>>()
        {
             {"jb_zmtr_v1",
                new()
                {
                    new("LeftBottom",new VectorTemp(1969,1317,-200)),
                    new("RightTop",new VectorTemp( 2222,1505,-13))
                }
             },
             {"jb_zmtr_v2",
                new()
                {
                    new("LeftBottom",new VectorTemp(2364,-50,107)),
                    new("RightTop",new VectorTemp(2620,140,232))
                }
             },
        };
    }

    //public class MapConfig
    //{
    //    [JsonPropertyName("MapCellCoords")]
    //    public List<SkzCoordinate> MapCellCoords { get; set; } = new List<SkzCoordinate>()
    //    {
    //        new("jb_zmtr_v1" ,new VectorTemp(-535, 345, -27))
    //    };

    //    [JsonPropertyName("SkzCoordinates")]
    //    public List<SkzDatas> SkzCoordinates { get; set; } = new()
    //    {
    //       new("jb_zmtr_v1", new List<SkzCoordinate>()
    //            {
    //                new("Hücre",   new VectorTemp(-535,345,-27) ),
    //                new("KZ",      new VectorTemp(2102,739,-357) ),
    //            }
    //        ),
    //        new("jb_zmtr_v2", new List<SkzCoordinate>()
    //            {
    //                new("Hücre",   new VectorTemp(-535,345,-27) ),
    //                new("KZ",      new VectorTemp(2102,739,-357) ),
    //            }
    //        ),
    //    };
    //}

    //public class SkzDatas
    //{
    //    [JsonPropertyName("MapCellCoords")]
    //    public string Text =
    //    public List<SkzCoordinate> SkzCoordinates;

    //    public SkzDatas(string v, List<SkzCoordinate> skzCoordinates)
    //    {
    //        Text = v;
    //        SkzCoordinates = skzCoordinates;
    //    }
    //}
}