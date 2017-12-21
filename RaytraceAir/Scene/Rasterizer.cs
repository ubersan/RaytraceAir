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
            for (var row = 0; row < camera.HeightInPixel; ++row)
            {
                for (var column = 0; column < camera.WidthInPixel; ++column)
                {
                    var x = (2 * (column + 0.5f) / camera.WidthInPixel - 1) * scale;
                    var y = (1 - 2 * (row + 0.5f) / camera.HeightInPixel) * scale / aspectRatio;

                    yield return new Pixel { X = x, Y = y, Column = column, Row = row };
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
            public int Column;
            public int Row;
        }
    }
}