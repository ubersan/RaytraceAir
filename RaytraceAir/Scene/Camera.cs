using System.Numerics;

namespace RaytraceAir
{
    public class Camera
    {
        private Camera(Vector3 position, Vector3 upDirection, Vector3 viewDirection, float horizontalFoV,  int widthInPixel, int heightInPixel)
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
        public Vector3[,] Pixels { get; set; }

        public int NumberOfPixels => WidthInPixel * HeightInPixel;

        public static Camera CreateBasicCamera(bool lowPixels = false)
        {
            return new Camera(
                position: new Vector3(0, 3, 10),
                upDirection: Vector3.Normalize(new Vector3(0, 10, -1.8f)),
                viewDirection: Vector3.Normalize(new Vector3(0, -1.8f, -10)),
                horizontalFoV: 30,
                widthInPixel: lowPixels ? 480 : 1920,
                heightInPixel: lowPixels ? 270: 1080);
        }

        public static Camera CreateStraightCamera(bool lowPixels = false)
        {
            return new Camera(
                position: new Vector3(0, 0, 30),
                upDirection: Vector3.Normalize(new Vector3(0, 1, 0)),
                viewDirection: Vector3.Normalize(new Vector3(0, 0, -1)),
                horizontalFoV: 30,
                widthInPixel: lowPixels ? 480 : 1920,
                heightInPixel: lowPixels ? 270 : 1080);
        }

        public static Camera CreateStraightTopCamera(bool lowPixels = false)
        {
            return new Camera(
                position: new Vector3(0, 5, 30),
                upDirection: Vector3.Normalize(new Vector3(0, 10, -1.8f)),
                viewDirection: Vector3.Normalize(new Vector3(0, -1.8f, -10)),
                horizontalFoV: 30,
                widthInPixel: lowPixels ? 480 : 1920,
                heightInPixel: lowPixels ? 270 : 1080);
        }

        public static Camera CreateTopCamera(bool lowPixels = false)
        {
            return new Camera(
                position: new Vector3(0, 30, 0),
                upDirection: Vector3.Normalize(new Vector3(0, 0, -1)),
                viewDirection: Vector3.Normalize(new Vector3(0, -1, 0)),
                horizontalFoV: 30,
                widthInPixel: lowPixels ? 480 : 1920,
                heightInPixel: lowPixels ? 270 : 1080);
        }
    }
}