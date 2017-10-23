using System.Numerics;

namespace RaytraceAir
{
    public class Camera
    {
        public Camera(Vector3 position, Vector3 upDirection, Vector3 viewDirection, float horizontalFoV,  int widthInPixel, int heightInPixel)
        {
            Position = position;
            UpDirection = upDirection;
            ViewDirection = viewDirection;
            RightDirection = Vector3.Normalize(Vector3.Cross(ViewDirection, UpDirection));
            HorizontalFoV = horizontalFoV;
            WidthInPixel = widthInPixel;
            HeightInPixel = heightInPixel;
            Pixels = new Vector3[WidthInPixel, HeightInPixel];

            for (var i = 0; i < WidthInPixel; ++i)
            {
                for (var j = 0; j < HeightInPixel; ++j)
                {
                    Pixels[i, j] = Vector3.Zero;
                }
            }
        }

        public Vector3 Position { get; }
        public Vector3 UpDirection { get; }
        public Vector3 ViewDirection { get; }
        public Vector3 RightDirection { get; }
        public float HorizontalFoV { get; }
        public int WidthInPixel { get; }
        public int HeightInPixel { get; }
        public Vector3[,] Pixels { get; }
    }
}