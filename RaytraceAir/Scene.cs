using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace RaytraceAir
{
    public class Scene
    {
        private readonly Camera _camera;
        private readonly List<Sphere> _spheres;
        private readonly List<Vec3> _lights;

        public Scene(Camera camera, List<Sphere> spheres, List<Vec3> lights)
        {
            _camera = camera;
            _spheres = spheres;
            _lights = lights;
        }

        public void Render()
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

                    var dir = (_camera.ViewDirection + x*_camera.RightDirection + y*_camera.UpDirection).Normalized();

                    if (Trace(origin, dir, out var hitSphere, out var hitPoint))
                    {
                        var lightDir = (_lights[0] - hitPoint).Normalized();
                        var start = hitPoint + hitSphere.Normal(hitPoint) * 1e-6;

                        if (TraceShadow(start, lightDir))
                        {
                            _camera.Pixels[i, j] = Vec3.Zeros();
                        }
                        else
                        {
                            _camera.Pixels[i, j] = Vec3.Ones() * lightDir.Dot(hitSphere.Normal(hitPoint));
                        }
                    }
                    else
                    {
                        _camera.Pixels[i, j] = new Vec3(0.8, 0.2, 0.3);
                    }
                }
            }
        }

        private double deg2rad(double deg)
        {
            return deg * Math.PI / 180;
        }

        private bool Trace(Vec3 origin, Vec3 dir, out Sphere hitSphere, out Vec3 hitPoint)
        {
            hitSphere = null;
            hitPoint = Vec3.Zeros();

            var closestT = double.MaxValue;
            foreach (var sphere in _spheres)
            {
                if (sphere.Intersects(origin, dir, out var t) && t < closestT)
                {
                    hitSphere = sphere;
                    hitPoint = origin + t * dir;
                    closestT = t;
                }
            }

            return hitSphere != null;
        }

        private bool TraceShadow(Vec3 origin,  Vec3 dir)
        {
            Sphere shadowSphere = null;
            foreach (var sphere in _spheres)
            {
                if (sphere.Intersects(origin, dir, out var _))
                {
                    shadowSphere = sphere;
                    break;
                }
            }

            return shadowSphere != null;
        }

        public void Export()
        {
            using (var bmp = new Bitmap(_camera.WidthInPixel, _camera.HeightInPixel, PixelFormat.Format24bppRgb))
            {
                var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                    ImageLockMode.ReadWrite,
                    bmp.PixelFormat);

                for (var j = 0; j < bmp.Height; ++j)
                {
                    for (var i = 0; i < bmp.Width; ++i)
                    {
                        var px = _camera.Pixels[i, j];
                        var col = new[] { (byte)(255*px.X), (byte)(255*px.Y), (byte)(255*px.Z) };
                        Marshal.Copy(col, 0, data.Scan0 + (j * bmp.Width + i) * 3, 3);
                    }
                }

                bmp.UnlockBits(data);
                bmp.Save("bmp.bmp");
            }
        }
    }
}