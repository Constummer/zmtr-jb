using System.Text.Json.Serialization;

namespace JailbreakExtras
{
    public class VectorTemp
    {
        public VectorTemp()
        {
        }

        public VectorTemp(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public VectorTemp(double x, double y, double z)
        {
            X = (float)x;
            Y = (float)y;
            Z = (float)z;
        }

        public VectorTemp(float? x, float? y, float? z)
        {
            X = x ?? 0;
            Y = y ?? 0;
            Z = z ?? 0;
        }

        [JsonPropertyName("X")]
        public float X { get; set; }

        [JsonPropertyName("Y")]
        public float Y { get; set; }

        [JsonPropertyName("Z")]
        public float Z { get; set; }
    }
}