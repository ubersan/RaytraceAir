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
        private readonly List<Vec3> _lights;

        public Scene(Camera camera, List<Sphere> spheres, List<Vec3> lights)
        {
            _camera = camera;
            _spheres = spheres;
            _lights = lights;
        }

        public void Trace()
        {
            var scale = Math.Tan(deg2rad(_camera.HorizontalFoV * 0.5));
            var aspectRatio = _camera.WidthInPixel / (double) _camera.HeightInPixel;
            var angle = Math.Acos(new Vec3(0, 0, -1).Dot(_camera.ViewDirection)/(new Vec3(0, 0, -1).Norm*_camera.ViewDirection.Norm))*180/Math.PI;
            for (var j = 0; j < _camera.HeightInPixel; ++j)
            {
                for (var i = 0; i < _camera.WidthInPixel; ++i)
                {
                    var origin = _camera.Position;

                    var x = (2 * (i + 0.5) / _camera.WidthInPixel - 1) * scale;
                    var y = (1 - 2 * (j + 0.5) / _camera.HeightInPixel) * scale * 1 / aspectRatio;

                    var dir = new Vec3(x, y, -1).Normalized();

                    var a = deg2rad(angle);
                    var newDir = new Vec3(
                        Math.Cos(a)*dir.X - Math.Sin(a)*dir.Z,
                        dir.Y,
                        -Math.Sin(a)*dir.X + Math.Cos(a)*dir.Z)
                    .Normalized();

                    Sphere hitSphere = null;
                    Vec3 hitPoint = null;
                    foreach (var sphere in _spheres)
                    {
                        if (sphere.Intersects(origin, newDir,  out var t))
                        {
                            hitSphere = sphere;
                            hitPoint = origin + t * newDir;
                        }
                    }

                    if (hitSphere != null)
                    {
                        var lightDir = (_lights[0] - hitPoint).Normalized();
                        var start = hitPoint + hitSphere.Normal(hitPoint) * 1e-6;

                        Sphere shadowSphere = null;
                        foreach (var sphere in _spheres)
                        {
                            if (sphere.Intersects(start, lightDir, out var _))
                            {
                                shadowSphere = sphere;
                                _camera.Pixels[i, j] = Vec3.Zeros();
                                break;
                            }
                        }
                        
                        if (shadowSphere == null)
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

        public void Render()
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