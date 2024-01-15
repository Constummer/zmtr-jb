﻿using System.Text.Json.Serialization;
using static JailbreakExtras.JailbreakExtras;

namespace JailbreakExtras
{
    public class MapConfig
    {
        [JsonPropertyName("MapCellCoords")]
        public List<SkzCoordinate> MapCellCoords { get; set; } = new List<SkzCoordinate>()
        {
            new("jb_zmtr_v1" ,new VectorTemp(-535, 345, -27)),
            new("jb_zmtr_v2_vsvs" ,new VectorTemp(-535, 345, -27))
        };

        [JsonPropertyName("SkzCoordinates")]
        public Dictionary<string, List<SkzCoordinate>> SkzCoordinates { get; set; } = new Dictionary<string, List<SkzCoordinate>>()
        {
            {"jb_zmtr_v1", new List<SkzCoordinate>()
                {
                    new("Hucre",   new VectorTemp(-535,345,-27) ),
                    new("KZ",      new VectorTemp(2102,739,-357) ),
                }
            },
            {"jb_zmtr_v2_vsvsvs", new List<SkzCoordinate>()
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
            {"jb_zmtr_v2_vsvsvs",new Dictionary<string, string>()
                 {
                    {"kacak", "func_door" },
                    {"kapi2", "func_door" },
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
    //        new("jb_zmtr_v2_vsvsvs", new List<SkzCoordinate>()
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