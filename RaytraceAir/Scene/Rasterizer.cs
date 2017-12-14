using System;
using System.Collections.Generic;

namespace RaytraceAir
{
    public static class Rasterizer
    {
        public static IEnumerable<Pixel> GetPixels(Camera camera)
        {
            var scale = (float)Math.Tan(DegreeToRadian(camera.HorizontalFoV * 0.5f));
            var aspectRatio = camera.WidthInPixel / (float)camera.HeightInPixel;
            for (var j = 0; j < camera.HeightInPixel; ++j)
            {
                for (var i = 0; i < camera.WidthInPixel; ++i)
                {
                    var x = (2 * (i + 0.5f) / camera.WidthInPixel - 1) * scale;
                    var y = (1 - 2 * (j + 0.5f) / camera.HeightInPixel) * scale / aspectRatio;

                    yield return new Pixel { X = x, Y = y, I = i, J = j };
                }
            }
        }

        private static float DegreeToRadian(float degree)
        {
            return degree * (float)Math.PI / 180f;
        }

        public struct Pixel
        {
            public float X;
            public float Y;
            public int I;
            public int J;
        }
    }
}