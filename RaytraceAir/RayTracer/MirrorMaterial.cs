using System.Numerics;

namespace RaytraceAir
{
    public class MirrorMaterial : MaterialTracer
    {
        public override Vector3 GetColorContribution(Vector3 originShadowRay, Vector3 dir, Vector3 hitPoint, SceneObject hitSceneObject, int depth, SceneObject light, Vector3 lightDir, float lightDist)
        {
            var hitNormal = hitSceneObject.Normal(hitPoint);
            var reflectionDir = Vector3.Normalize(GetReflectionDir(dir, hitNormal));
            return 0.8f * RayTracer.CastRay(originShadowRay, reflectionDir, depth + 1);
        }

        public override MaterialType MaterialType { get; } = MaterialType.Mirror;
    }
}