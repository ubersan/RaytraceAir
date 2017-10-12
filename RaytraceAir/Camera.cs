namespace RaytraceAir
{
    public class Camera
    {
        public Camera(Vec3 position, Vec3 upDirection, Vec3 viewDirection, double horizontalFoV,  int widthInPixel, int heightInPixel)
        {
            Position = position;
            UpDirection = upDirection;
            ViewDirection = viewDirection;
            RightDirection = ViewDirection.Cross(UpDirection);
            HorizontalFoV = horizontalFoV;
            WidthInPixel = widthInPixel;
            HeightInPixel = heightInPixel;
            Pixels = new Vec3[WidthInPixel, HeightInPixel];

            for (var i = 0; i < WidthInPixel; ++i)
            {
                for (var j = 0; j < HeightInPixel; ++j)
                {
                    Pixels[i, j] = Vec3.Zeros();
                }
            }
        }

        public Vec3 Position { get; }
        public Vec3 UpDirection { get; }
        public Vec3 ViewDirection { get; }
        public Vec3 RightDirection { get; }
        public double HorizontalFoV { get; }
        public int WidthInPixel { get; }
        public int HeightInPixel { get; }
        public Vec3[,] Pixels { get; }
    }
}