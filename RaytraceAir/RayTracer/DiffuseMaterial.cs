using System;
using System.Numerics;

namespace RaytraceAir
{
    public class DiffuseMaterial : MaterialTracer
    {
        public override Vector3 GetColorContribution(Vector3 originShadowRay, Vector3 dir, Vector3 hitPoint, SceneObject hitSceneObject, int depth, SceneObject light, Vector3 lightDir, float lightDist)
        {
            var contribution = Vector3.Dot(lightDir, hitSceneObject.Normal(hitPoint));
            contribution *= 4000 * SceneObject.Albedo / (float)Math.PI;
            contribution /= light.GetFalloff(lightDist);

            return hitSceneObject.Color * light.Color * Math.Max(0, contribution);
        }

        public override MaterialType MaterialType { get; } = MaterialType.Diffuse;
    }
}