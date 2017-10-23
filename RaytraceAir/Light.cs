using System.Numerics;

namespace RaytraceAir
{
    public abstract class Light
    {
        protected Light(Vector3 color)
        {
            Color = color;
        }

        public Vector3 Color { get; }

        public abstract float GetFalloff(float distance);
        public abstract float GetDistToLight(Vector3 hitPoint);
        public abstract Vector3 GetDirToLight(Vector3 hitPoint);
    }
}