using System;
using System.Collections.Generic;
using System.Numerics;

namespace RaytraceAir
{
    public class Scene
    {
        private readonly Camera _camera;
        private readonly List<SceneObject> _sceneObjects;
        private readonly List<Light> _lights;
        private readonly Vector3 _background = new Vector3(0.8f, 0.2f, 0.3f);

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
                var dir = Vector3.Normalize(_camera.ViewDirection + pixel.X * _camera.RightDirection + pixel.Y * _camera.UpDirection);

                _camera.Pixels[pixel.I, pixel.J] = CastRay(originPrimaryRay, dir, 0);
            }
        }

        private Vector3 CastRay(Vector3 origin, Vector3 dir, int depth)
        {
            if (depth == 5)
            {
                return _background;
            }

            var color = Vector3.Zero;

            if (Trace(origin, dir, out var hitSceneObject, out var hitPoint))
            {
                var originShadowRay = hitPoint + hitSceneObject.Normal(hitPoint) * 1e-4f;

                foreach (var light in _lights)
                {
                    var lightDist = light.GetDistToLight(hitPoint);
                    var lightDir = light.GetDirToLight(hitPoint);

                    var isIlluminated = TraceShadow(originShadowRay, lightDir, lightDist);

                    var contribution = Vector3.Dot(lightDir, hitSceneObject.Normal(hitPoint));
                    contribution *= 4000 * hitSceneObject.Albedo / (float)Math.PI;
                    contribution /= light.GetFalloff(lightDist);

                    // TODO: BRDF und MIRROR
                    color += isIlluminated * hitSceneObject.Color * light.Color * Math.Max(0, contribution);

                    if (hitSceneObject.Material == Material.Mirror && isIlluminated > 0)
                    {
                        var reflectionDir = Vector3.Normalize(GetReflectionDir(dir, hitSceneObject.Normal(hitPoint)));
                        color += 0.8f * CastRay(originShadowRay, reflectionDir, ++depth);
                    }

                    if (hitSceneObject.Material == Material.Transparent && isIlluminated > 0)
                    {
                        var hitNormal = hitSceneObject.Normal(hitPoint);
                        var kr = Fresnel(dir, hitNormal, 1.5f);
                        var outside = Vector3.Dot(dir, hitNormal) < 0;
                        var bias = 1e-4f * hitNormal;
                        var refractionColor = Vector3.Zero;
                        if (kr < 1)
                        {
                            var refractionDir = Vector3.Normalize(GetRefractionDir(dir, hitNormal, 1.5f));
                            var refractionorig = outside ? hitPoint - bias : hitPoint + bias;
                            refractionColor = CastRay(refractionorig, refractionDir, ++depth);
                        }
                        var reflectionDir = Vector3.Normalize(GetReflectionDir(dir, hitNormal));
                        var reflectionOrig = outside ? hitPoint + bias : hitPoint - bias;
                        var reflectionColor = CastRay(reflectionOrig, reflectionDir, ++depth);

                        color += reflectionColor * kr + refractionColor * (1 - kr);
                    }
                }
            }
            else
            {
                return _background;
            }

            return color;
        }

        private float deg2rad(float deg)
        {
            return deg * (float)Math.PI / 180;
        }

        private Vector3 GetReflectionDir(Vector3 viewDir, Vector3 normal)
        {
            return viewDir - 2f * Vector3.Dot(viewDir, normal) * normal;
        }

        private Vector3 GetRefractionDir(Vector3 viewDir, Vector3 normal, float ior)
        {
            var cosi = Clamp(-1, 1, Vector3.Dot(viewDir, normal));
            float etai = 1, etat = ior;

            Vector3 n;
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
            return k < 0 ? Vector3.Zero : eta * viewDir + (eta * cosi - (float)Math.Sqrt(k)) * n;
        }

        private float Fresnel(Vector3 viewDir, Vector3 normal, float ior)
        {
            var cosi = Clamp(-1, 1, Vector3.Dot(viewDir, normal));
            float etai = 1, etat = ior;

            if (cosi > 0)
            {
                var t = etai;
                etai = etat;
                etat = t;
            }

            var sint = etai / etat * (float)Math.Sqrt(Math.Max(0, 1 - cosi * cosi));
            if (sint >= 1)
            {
                return 1;
            }

            var cost = (float)Math.Sqrt(Math.Max(0, 1 - sint * sint));
            cosi = Math.Abs(cosi);
            var rs = (etat * cosi - etai * cost) / (etat * cosi + etai * cost);
            var rp = (etai  * cosi - etat  * cost) / (etai * cosi + etat *  cost);

            return (rs * rs + rp * rp) / 2;
        }

        private static float Clamp(float lo, float hi, float value)
        {
            return Math.Max(lo, Math.Min(hi, value));
        }

        private bool Trace(Vector3 origin, Vector3 dir, out SceneObject hitSceneObject, out Vector3 hitPoint)
        {
            hitSceneObject = null;
            hitPoint = Vector3.Zero;

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

        private float TraceShadow(Vector3 origin, Vector3 dir, float distToLight)
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

            return shadowSphere != null ? 0f : 1f;
        }

        private IEnumerable<Pixel> GetPixel()
        {
            var scale = (float)Math.Tan(deg2rad(_camera.HorizontalFoV * 0.5f));
            var aspectRatio = _camera.WidthInPixel / (float) _camera.HeightInPixel;
            for (var j = 0; j < _camera.HeightInPixel; ++j)
            {
                for (var i = 0; i < _camera.WidthInPixel; ++i)
                {
                    var x = (2 * (i + 0.5f) / _camera.WidthInPixel - 1) * scale;
                    var y = (1 - 2 * (j + 0.5f) / _camera.HeightInPixel) * scale * 1 / aspectRatio;

                    yield return new Pixel {X = x, Y = y, I = i, J = j};
                }
            }
        }
    }

    public struct Pixel
    {
        public float X;
        public float Y;
        public int I;
        public int J;
    }
}