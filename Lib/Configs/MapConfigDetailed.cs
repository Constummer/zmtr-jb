using System.Text.Json.Serialization;

namespace JailbreakExtras
{
    public class MapConfigDetailed
    {
        public MapConfigDetailed()
        {
        }

        public MapConfigDetailed(VectorTemp mapCellCoords, VectorTemp paramCoords, List<CoordinateTemplate> skzCoordinates, Dictionary<string, string> kapiAcKapaList, string forceOpenDoor, VectorTemp coinCoords, List<CoordinateTemplate> kzCellCoords)
        {
            MapCellCoords = mapCellCoords;
            ParamCoords = paramCoords;
            SkzCoordinates = skzCoordinates;
            KapiAcKapaList = kapiAcKapaList;
            ForceOpenDoor = forceOpenDoor;
            CoinCoords = coinCoords;
            KzCellCoords = kzCellCoords;
        }

        [JsonPropertyName("MapCellCoords")]
        public VectorTemp MapCellCoords { get; set; } = new VectorTemp(-535, 345, -27);

        [JsonPropertyName("ParamCoords")]
        public VectorTemp ParamCoords { get; set; } = new VectorTemp(4497, 4261, -1880);

        [JsonPropertyName("SkzCoordinates")]
        public List<CoordinateTemplate> SkzCoordinates { get; set; } = new()
        {
            new("Hucre",   new VectorTemp(-2478,-18,61) ),
            new("KZ",      new VectorTemp(1714,589,-151) ),
            new("KZ Sag Kulvar",  new VectorTemp(2064,817,-106) ),
            new("KZ Sol Kulvar",  new VectorTemp(1356,817,-106) ),
        };

        [JsonPropertyName("KapiAcKapaList")]
        public Dictionary<string, string> KapiAcKapaList { get; set; } = new()
        {
           {"kacak", "func_door" },
           {"kapi2", "func_door" },
        };

        [JsonPropertyName("ForceOpenDoor")]
        public string ForceOpenDoor { get; set; } = "kapi2";

        [JsonPropertyName("CoinCoords")]
        public VectorTemp CoinCoords { get; set; } = new VectorTemp(-718, -765, 24);

        [JsonPropertyName("KzCellCoords")]
        public List<CoordinateTemplate> KzCellCoords { get; set; } = new List<CoordinateTemplate>()
        {
            new ("LeftBottom", new VectorTemp(1969,1316,-210)),
            new ("RightTop",   new VectorTemp(2222,1505,-13))
        };
    }
}