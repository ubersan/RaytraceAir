using System.Numerics;

namespace RaytraceAir
{
    public abstract class MaterialTracer
    {
        public abstract Vector3 GetColorContribution(Vector3 originShadowRay, Vector3 dir, Vector3 hitPoint, SceneObject hitSceneObject, int depth, SceneObject light, Vector3 lightDir, float lightDist);

        protected static Vector3 GetReflectionDir(Vector3 viewDir, Vector3 normal)
        {
            return viewDir - 2f * Vector3.Dot(viewDir, normal) * normal;
        }

        public abstract MaterialType MaterialType { get; }
    }
}