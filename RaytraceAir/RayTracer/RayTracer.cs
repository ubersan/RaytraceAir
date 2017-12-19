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

        private  static IEnumerable<SceneObject> SceneObjectsWithoutLights => SceneObjects.Where(sceneObject => !sceneObject.IsLight);
        private static IEnumerable<SceneObject> Lights => SceneObjects.Where(sceneObject => sceneObject.IsLight);

        public static Vector3 CastRay(Vector3 origin, Vector3 dir, int depth)
        {
            if (depth == MaxRecursionDepth)
            {
                return _background;
            }

            var color = Vector3.Zero;

            if (Trace(origin, dir, out var hitSceneObject, out var hitPoint))
            {
                if (hitSceneObject.IsLight)
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
                                // no contribution to final color by this sample
                                continue;
                            }

                            foreach (var material in hitSceneObject.Materials)
                            {
                                color += material.GetColorContribution(originShadowRay, dir, hitPoint, hitSceneObject,
                                    depth, light, lightDir, lightDist);
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
                    if (sceneObject.IsTransparent)
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