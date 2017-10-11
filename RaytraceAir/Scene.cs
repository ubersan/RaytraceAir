using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace RaytraceAir
{
    public class Scene
    {
        private readonly Camera _camera;
        private readonly List<Sphere> _spheres;

        public Scene(Camera camera, List<Sphere> spheres)
        {
            _camera = camera;
            _spheres = spheres;
        }

        public void Trace()
        {
            var scale = Math.Tan(deg2rad(_camera.HorizontalFoV * 0.5));
            var aspectRatio = _camera.WidthInPixel / (double) _camera.HeightInPixel;
            for (var j = 0; j < _camera.HeightInPixel; ++j)
            {
                for (var i = 0; i < _camera.WidthInPixel; ++i)
                {
                    var origin = _camera.Position;

                    var x = (2 * (i + 0.5) / _camera.WidthInPixel - 1) * scale;
                    var y = (1 - 2 * (j + 0.5) / _camera.HeightInPixel) * scale * 1 / aspectRatio;

                    var dir = new Vec3(x, y, -1).Normalized();

                    Sphere hitSphere = null;
                    foreach (var sphere in _spheres)
                    {
                        if (sphere.Intersects(origin, dir))
                        {
                            hitSphere = sphere;
                        }
                    }

                    if (hitSphere != null)
                    {
                        _camera.Pixels[i, j] = Vec3.Ones();
                    }
                    else
                    {
                        _camera.Pixels[i, j] = new Vec3(200, 50, 90);
                    }
                }
            }
        }

        private double deg2rad(double deg)
        {
            return deg * Math.PI / 180;
        }

        public void Render()
        {
            using (var bmp = new Bitmap(_camera.WidthInPixel, _camera.HeightInPixel, PixelFormat.Format24bppRgb))
            {
                var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                    ImageLockMode.ReadWrite,
                    bmp.PixelFormat);

                var bytes = Math.Abs(data.Stride) * bmp.Height;
                var values = new byte[bytes];

                for (var j = 0; j < bmp.Height; ++j)
                {
                    for (var i = 0; i < bmp.Width; ++i)
                    {
                        var px = _camera.Pixels[i, j];
                        var col = new byte[] { (byte)(255*px.X), (byte)(255*px.Y), (byte)(255*px.Z) };
                        Marshal.Copy(col, 0, data.Scan0 + (j * bmp.Width + i) * 3, 3);
                    }
                }

                bmp.UnlockBits(data);
                bmp.Save("bmp.bmp");
            }
        }
    }
}