namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class Cylinder
    {
        private double centerX;
        private double centerY;
        private double centerZ;
        private double radius;
        private double height;

        public Cylinder(double centerX, double centerY, double centerZ, double radius, double height)
        {
            this.centerX = centerX;
            this.centerY = centerY;
            this.centerZ = centerZ;
            this.radius = radius;
            this.height = height;
        }

        public bool IsPointInside(double x, double y, double z)
        {
            double distanceXY = Math.Pow(x - centerX, 2) + Math.Pow(y - centerY, 2);
            bool insideXY = distanceXY <= Math.Pow(radius, 2);
            bool insideZ = z >= centerZ - height / 2 && z <= centerZ + height / 2;

            return insideXY && insideZ;
        }
    }
}