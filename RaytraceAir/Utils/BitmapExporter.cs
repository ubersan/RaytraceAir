using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace RaytraceAir
{
    public static class BitmapExporter
    {
        // TODO: It makes no sense to export the camera. One would expect to export data instead.
        public static string Export(Camera camera, string targetFolder, string filename)
        {
            using (var bmp = new Bitmap(camera.WidthInPixel, camera.HeightInPixel, PixelFormat.Format24bppRgb))
            {
                var data = bmp.LockBits(
                    new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
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

                var myImageCodecInfo = GetEncoderInfo("image/jpeg");
                var myEncoder = Encoder.Quality;
                var myEncoderParameters = new EncoderParameters(1);
                var myEncoderParameter = new EncoderParameter(myEncoder, 100L);
                myEncoderParameters.Param[0] = myEncoderParameter;

                Directory.CreateDirectory(targetFolder);
                var renderedImagePath = Path.Combine(targetFolder, $"{filename}.jpg");
                bmp.Save(renderedImagePath, myImageCodecInfo, myEncoderParameters);

                return renderedImagePath;
            }
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            var encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        // TODO: Code copy in  Scene
        private static double Clamp(double lo, double hi, double value)
        {
            return Math.Max(lo, Math.Min(hi, value));
        }
    }
}