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
                var originPrimaryRay = _camera.Position;
                var dir = (_camera.ViewDirection + pixel.X * _camera.RightDirection + pixel.Y * _camera.UpDirection).Normalized();

                _camera.Pixels[pixel.I, pixel.J] = CastRay(originPrimaryRay, dir, 0);
            }
        }

        private Vec3 CastRay(Vec3 origin, Vec3 dir, int depth)
        {
            if (depth == 5)
            {
                return Vec3.Background;
            }

            var color = Vec3.Zeros;

            if (Trace(origin, dir, out var hitSceneObject, out var hitPoint))
            {
                var originShadowRay = hitPoint + hitSceneObject.Normal(hitPoint) * 1e-12;

                foreach (var light in _lights)
                {
                    var lightDist = light.GetDistToLight(hitPoint);
                    var lightDir = light.GetDirToLight(hitPoint);

                    var isIlluminated = TraceShadow(originShadowRay, lightDir, lightDist);

                    var contribution = lightDir.Dot(hitSceneObject.Normal(hitPoint));
                    contribution *= 4000 * hitSceneObject.Albedo / Math.PI;
                    contribution /= light.GetFalloff(lightDist);

                    // TODO: BRDF und MIRROR
                    color += isIlluminated * hitSceneObject.Color * light.Color * Math.Max(0, contribution);

                    if (hitSceneObject.Material == Material.Mirror && isIlluminated > 0)
                    {
                        var reflectionDir = GetReflectionDir(dir, hitSceneObject.Normal(hitPoint)).Normalized();
                        color += 0.8 * CastRay(originShadowRay, reflectionDir, ++depth);
                    }

                    if (hitSceneObject.Material == Material.Transparent && isIlluminated > 0)
                    {
                        var hitNormal = hitSceneObject.Normal(hitPoint);
                        var kr = Fresnel(dir, hitNormal, 1.5);
                        var outside = dir.Dot(hitNormal) < 0;
                        var bias = 1e-12 * hitNormal;
                        var refractionColor = Vec3.Zeros;
                        if (kr < 1)
                        {
                            var refractionDir = GetRefractionDir(dir, hitNormal, 1.5).Normalized();
                            var refractionorig = outside ? hitPoint - bias : hitPoint + bias;
                            refractionColor = CastRay(refractionorig, refractionDir, ++depth);
                        }
                        var reflectionDir = GetReflectionDir(dir, hitNormal).Normalized();
                        var reflectionOrig = outside ? hitPoint + bias : hitPoint - bias;
                        var reflectionColor = CastRay(reflectionOrig, reflectionDir, ++depth);

                        color += reflectionColor * kr + refractionColor * (1 - kr);
                    }
                }
            }
            else
            {
                return Vec3.Background;
            }

            return color;
        }

        private double deg2rad(double deg)
        {
            return deg * Math.PI / 180;
        }

        private Vec3 GetReflectionDir(Vec3 viewDir, Vec3 normal)
        {
            return viewDir - 2 * viewDir.Dot(normal) * normal;
        }

        private Vec3 GetRefractionDir(Vec3 viewDir, Vec3 normal, double ior)
        {
            var cosi = Clamp(-1, 1, viewDir.Dot(normal));
            double etai = 1, etat = ior;
            var n = normal;
            if (cosi < 0)
            {
                cosi = -cosi;
            }
            {
                // swap etai,  etat
                var t = etai;
                etai = etat;
                etat = t;
                n = -normal;
            }
            var eta = etai / etat;
            var k = 1 - eta * eta * (1 - cosi * cosi);
            return k < 0 ? Vec3.Zeros : eta * viewDir + (eta * cosi - Math.Sqrt(k)) * n;
        }

        private double Fresnel(Vec3 viewDir, Vec3 normal, double ior)
        {
            var cosi = Clamp(-1, 1, viewDir.Dot(normal));
            double etai = 1, etat = ior;
            if (cosi > 0)
            {
                var t = etai;
                etai = etat;
                etat = t;
            }
            var sint = etai / etat * Math.Sqrt(Math.Max(0, 1 - cosi * cosi));
            if (sint >= 1)
            {
                return 1;
            }
            else
            {
                var cost = Math.Sqrt(Math.Max(0, 1 - sint * sint));
                cosi = Math.Abs(cosi);
                var Rs = ((etat * cosi) - (etai * cost)) / ((etat * cosi) + (etai * cost));
                var Rp = ((etai  * cosi) - (etat  * cost)) / ((etai * cosi) + (etat *  cost));
                return (Rs * Rs + Rp * Rp) / 2;
            }
        }

        private static double Clamp(double lo, double hi, double value)
        {
            return Math.Max(lo, Math.Min(hi, value));
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