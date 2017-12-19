using System.Numerics;

namespace RaytraceAir
{
    public class LightMaterial : MaterialTracer
    {
        public override Vector3 GetColorContribution(Vector3 originShadowRay, Vector3 dir, Vector3 hitPoint, SceneObject hitSceneObject,
            int depth, SceneObject light, Vector3 lightDir, float lightDist)
        {
            // TODO: What if light is not emitted into this direction...
            return hitSceneObject.Color;
        }

        public override MaterialType MaterialType { get; } = MaterialType.Light;
    }
}