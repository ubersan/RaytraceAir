using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace RaytraceAir
{
    public static class RayTracer
    {
        private static readonly Vector3 _background = Vector3.Zero;
        private const int MaxRecursionDepth = 5;
        private const int MaxLightSamples = 5;

        public static void SetSceneObjects(IEnumerable<SceneObject> sceneObjects)
        {
            SceneObjects = sceneObjects;
        }

        private static IEnumerable<SceneObject> SceneObjects { get; set; }

        private  static IEnumerable<SceneObject> SceneObjectsWithoutLights => SceneObjects.Where(sceneObject => sceneObject.Material != Material.Light);
        private static IEnumerable<SceneObject> Lights => SceneObjects.Where(sceneObject => sceneObject.Material == Material.Light);

        public static Vector3 CastRay(Vector3 origin, Vector3 dir, int depth)
        {
            if (depth == MaxRecursionDepth)
            {
                return _background;
            }

            var color = Vector3.Zero;

            if (Trace(origin, dir, out var hitSceneObject, out var hitPoint))
            {
                if (hitSceneObject.Material == Material.Light)
                {
                    // TODO: What if light is not emitted into this direction...
                    color = hitSceneObject.Color;
                }
                else
                {
                    var originShadowRay = hitPoint + hitSceneObject.Normal(hitPoint) * 1e-4f;
                    foreach (var light in Lights)
                    {
                        // TODO: Works only for 1 light
                        var lightSamples = light.GetSamples(hitPoint, MaxLightSamples).ToList();
                        foreach ((var lightDir, var lightDist) in lightSamples)
                        {
                            var isIlluminated = TraceShadow(originShadowRay, lightDir, lightDist) && light.EmitsLightInto(lightDir);

                            if (!isIlluminated)
                            {
                                // no color contribution in this sample
                                continue;
                            }

                            color += HitObjectColorContribution(hitSceneObject, hitPoint, light, lightDir, lightDist);

                            if (hitSceneObject.Material == Material.Mirror)
                            {
                                var normal = hitSceneObject.Normal(hitPoint);
                                color += MirrorColorContribution(originShadowRay, dir, normal, depth);
                            }
                            else if (hitSceneObject.Material == Material.Transparent)

                            {
                                var hitNormal = hitSceneObject.Normal(hitPoint);
                                color += TranparentColorContribution(originShadowRay, dir, hitPoint, hitNormal, depth);
                            }
                        }

                        color /= lightSamples.Count;
                    }
                }
            }
            else
            {
                return _background;
            }

            return color;
        }

        private static Vector3 TranparentColorContribution(Vector3 originShadowRay, Vector3 dir, Vector3 hitPoint, Vector3 hitNormal, int depth)
        {
            var kr = Fresnel(dir, hitNormal, 1.5f);
            var outside = Vector3.Dot(dir, hitNormal) < 0;
            var bias = 1e-4f * hitNormal;
            var refractionColor = Vector3.Zero;
            if (kr < 1)
            {
                var refractionDir = Vector3.Normalize(GetRefractionDir(dir, hitNormal, 1.5f));
                var refractionorig = outside ? hitPoint - bias : hitPoint + bias;
                refractionColor = CastRay(refractionorig, refractionDir, depth + 1);
            }
            var reflectionDir = Vector3.Normalize(GetReflectionDir(dir, hitNormal));
            var reflectionOrig = outside ? hitPoint + bias : hitPoint - bias;
            var reflectionColor = CastRay(reflectionOrig, reflectionDir, depth + 1);

            return reflectionColor * kr + refractionColor * (1 - kr);
        }

        private static Vector3 MirrorColorContribution(Vector3 originShadowRay, Vector3 dir, Vector3 normal, int depth)
        {
            var reflectionDir = Vector3.Normalize(GetReflectionDir(dir, normal));
            return 0.8f * CastRay(originShadowRay, reflectionDir, depth + 1);
        }

        private static Vector3 HitObjectColorContribution(SceneObject hitSceneObject, Vector3 hitPoint, SceneObject light, Vector3 lightDir, float lightDist)
        {
            var contribution = Vector3.Dot(lightDir, hitSceneObject.Normal(hitPoint));
            contribution *= 4000 * hitSceneObject.Albedo / (float)Math.PI;
            contribution /= light.GetFalloff(lightDist);

            return hitSceneObject.Color * light.Color * Math.Max(0, contribution);
        }

        private static Vector3 GetReflectionDir(Vector3 viewDir, Vector3 normal)
        {
            return viewDir - 2f * Vector3.Dot(viewDir, normal) * normal;
        }

        private static Vector3 GetRefractionDir(Vector3 viewDir, Vector3 normal, float ior)
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

        private static float Fresnel(Vector3 viewDir, Vector3 normal, float ior)
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
            var rp = (etai * cosi - etat * cost) / (etai * cosi + etat * cost);

            return (rs * rs + rp * rp) / 2;
        }

        private static float Clamp(float lo, float hi, float value)
        {
            return Math.Max(lo, Math.Min(hi, value));
        }

        private static bool Trace(Vector3 origin, Vector3 dir, out SceneObject hitSceneObject, out Vector3 hitPoint)
        {
            hitSceneObject = null;
            hitPoint = Vector3.Zero;

            var closestT = double.MaxValue;
            foreach (var sceneObject in SceneObjects)
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

        private static bool TraceShadow(Vector3 origin, Vector3 dir, float distToLight)
        {
            // TODO: I still could collide with a light, which is not emitting light into the object's direction
            foreach (var sceneObject in SceneObjectsWithoutLights)
            {
                if (sceneObject.Intersects(origin, dir, out var t) && t < distToLight)
                {
                    if (sceneObject.Material == Material.Transparent)
                    {
                        continue;
                    }

                    return false;
                }
            }

            return true;
        }
    }
}