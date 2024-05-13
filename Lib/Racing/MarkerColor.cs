using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static void Racing_GetColorOfMarker(List<object> markers)
    {
        int totalCount = markers.Count;

        for (int i = 0; i < markers.Count; i++)
        {
            Color interpolatedColor = InterpolateColor(Color.Red, Color.Green, (float)i / (totalCount - 1));

            int r = Clamp(interpolatedColor.R, 0, 255);
            int g = Clamp(interpolatedColor.G, 0, 255);
            int b = Clamp(interpolatedColor.B, 0, 255);

            Console.WriteLine($"Item {markers[i]} - R: {r}, G: {g}, B: {b}");
        }

        static Color InterpolateColor(Color color1, Color color2, float ratio)
        {
            int r = (int)(color1.R + (color2.R - color1.R) * ratio);
            int g = (int)(color1.G + (color2.G - color1.G) * ratio);
            int b = (int)(color1.B + (color2.B - color1.B) * ratio);
            return Color.FromArgb(r, g, b);
        }

        static int Clamp(int value, int min, int max)
        {
            return Math.Max(min, Math.Min(max, value));
        }
    }
}