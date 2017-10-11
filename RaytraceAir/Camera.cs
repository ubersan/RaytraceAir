using System.Collections;
using System.Collections.Generic;

namespace RaytraceAir
{
    public class Camera
    {
        public Camera(Vec3 position, Vec3 upDirection, Vec3 viewDirection, double horizontalFoV,  int widthInPixel, int heightInPixel)
        {
            Position = position;
            UpDirection = upDirection;
            ViewDirection = viewDirection;
            HorizontalFoV = horizontalFoV;
            WidthInPixel = widthInPixel;
            HeightInPixel = heightInPixel;
            Pixels = new Vec3[WidthInPixel, HeightInPixel];
        }

        public Vec3 Position { get; }
        public Vec3 UpDirection { get; }
        public Vec3 ViewDirection { get; }
        public double HorizontalFoV { get; }
        public int WidthInPixel { get; }
        public int HeightInPixel { get; }
        public Vec3[,] Pixels { get; }
    }
}