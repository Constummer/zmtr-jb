namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static bool Racing_IsInsideRacePoint(double player_x, double player_y, double player_z,
                                          double marker_x, double marker_y, double marker_z)
    {
        Cylinder cylinder = new Cylinder(marker_x, marker_y, marker_z, 100, 140);

        if (cylinder.IsPointInside(marker_x, marker_y, marker_z))
        {
            Console.WriteLine($"Point ({marker_x},{marker_y},{marker_z}) is inside the cylinder.");
            return true;
        }
        else
        {
            Console.WriteLine($"Point ({marker_x},{marker_y},{marker_z}) is outside the cylinder.");
            return false;
        }
    }
}