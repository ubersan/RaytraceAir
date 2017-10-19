using System;
using System.Collections.Generic;

namespace RaytraceAir
{
    public class Scene
    {
        private readonly Camera _camera;
        private readonly List<SceneObject> _sceneObjects;
        private readonly List<Light> _lights;

        public Scene(Camera camera, List<SceneObject> sceneObjects, List<Light> lights)
        {
            _camera = camera;
            _sceneObjects = sceneObjects;
            _lights = lights;
        }

        public void Render()
        {
            foreach (var pixel in GetPixel())
            {
                if (pixel.I == 990 && pixel.J == 506)
                {
                    var i = 313;
                }

                _camera.Pixels[pixel.I, pixel.J] = new Vec3(0.8, 0.2, 0.3);

                var originPrimaryRay = _camera.Position;
                var dir = (_camera.ViewDirection + pixel.X * _camera.RightDirection + pixel.Y * _camera.UpDirection)
                    .Normalized();

                if (Trace(originPrimaryRay, dir, out var hitSceneObject, out var hitPoint))
                {
                    var originShadowRay = hitPoint + hitSceneObject.Normal(hitPoint) * 1e-12;

                    foreach (var light in _lights)
                    {
                        var lightDist = light.GetDistToLight(hitPoint);
                        var lightDir = light.GetDirToLight(hitPoint);

                        var isIlluminated = TraceShadow(originShadowRay, lightDir, lightDist);
                        if (Math.Abs(isIlluminated - 1d) < 1e-14)
                        {
                            var i = 313;
                        }
                        var contribution = lightDir.Dot(hitSceneObject.Normal(hitPoint));
                        contribution *= 15 * hitSceneObject.Albedo / Math.PI;
                        contribution /= light.GetFalloff(lightDist);

                        _camera.Pixels[pixel.I, pixel.J] = isIlluminated * Vec3.Ones() * Math.Max(0, contribution);
                    }
                }
            }
        }

        private double deg2rad(double deg)
        {
            return deg * Math.PI / 180;
        }

        private bool Trace(Vec3 origin, Vec3 dir, out SceneObject hitSceneObject, out Vec3 hitPoint)
        {
            hitSceneObject = null;
            hitPoint = null;

            var closestT = double.MaxValue;
            foreach (var sceneObject in _sceneObjects)
            {
                if (sceneObject.Intersects(origin, dir, out var t) && t < closestT)
                {
                    hitSceneObject = sceneObject;
                    hitPoint = origin + t * dir;
                    closestT = t;
                }
            }

            return hitSceneObject != null;
        }

        private double TraceShadow(Vec3 origin, Vec3 dir, double distToLight)
        {
            SceneObject shadowSphere = null;
            foreach (var sceneObject in _sceneObjects)
            {
                if (sceneObject.Intersects(origin, dir, out var t) && t < distToLight)
                {
                    shadowSphere = sceneObject;
                    break;
                }
            }

            return shadowSphere != null ? 0d : 1d;
        }

        private IEnumerable<Pixel> GetPixel()
        {
            var scale = Math.Tan(deg2rad(_camera.HorizontalFoV * 0.5));
            var aspectRatio = _camera.WidthInPixel / (double) _camera.HeightInPixel;
            for (var j = 0; j < _camera.HeightInPixel; ++j)
            {
                for (var i = 0; i < _camera.WidthInPixel; ++i)
                {
                    var x = (2 * (i + 0.5) / _camera.WidthInPixel - 1) * scale;
                    var y = (1 - 2 * (j + 0.5) / _camera.HeightInPixel) * scale * 1 / aspectRatio;

                    yield return new Pixel {X = x, Y = y, I = i, J = j};
                }
            }
        }
    }

    public struct Pixel
    {
        public double X;
        public double Y;
        public int I;
        public int J;
    }
}