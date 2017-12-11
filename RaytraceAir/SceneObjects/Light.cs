using System.Numerics;

namespace RaytraceAir
{
    public abstract class Light : SceneObject
    {
        protected Light(Vector3 color, Material material = Material.Light)
            : base(color, material)
        {
        }

        public abstract float GetFalloff(float distance);
        public abstract (Vector3 direction, float distance) GetRay(Vector3 hitPoint);
        public abstract float EmitsLightInto(Vector3 lightDir);
    }
}