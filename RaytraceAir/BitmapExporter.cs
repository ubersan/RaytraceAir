using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace RaytraceAir
{
    public static class BitmapExporter
    {
        public static void Export(Camera camera)
        {
            using (var bmp = new Bitmap(camera.WidthInPixel, camera.HeightInPixel, PixelFormat.Format24bppRgb))
            {
                var data = bmp.LockBits(
                    new Rectangle(0, 0, bmp.Width, bmp.Height),
                    ImageLockMode.WriteOnly,
                    bmp.PixelFormat);

                for (var j = 0; j < bmp.Height; ++j)
                {
                    for (var i = 0; i < bmp.Width; ++i)
                    {
                        var px = camera.Pixels[i, j];
                        var col = new[]
                        {
                            (byte) (255 * Clamp(0, 1, px.X)),
                            (byte) (255 * Clamp(0, 1, px.Y)),
                            (byte) (255 * Clamp(0, 1, px.Z))
                        };
                        Marshal.Copy(col, 0, data.Scan0 + (j * bmp.Width + i) * 3, 3);
                    }
                }

                bmp.UnlockBits(data);
                bmp.Save(@"..\..\..\Output\bmp.bmp");
            }
        }

        private static double Clamp(double lo, double hi, double value)
        {
            return Math.Max(lo, Math.Min(hi, value));
        }
    }
}