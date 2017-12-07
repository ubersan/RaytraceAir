using System;
using System.Numerics;

namespace RaytraceAir
{
    public class PointLight : Light
    {
        private readonly Vector3 _position;

        public PointLight(Vector3 position, Vector3 color)
            : base(color)
        {
            _position = position;
        }

        public override float GetFalloff(float distance)
        {
            return 4f * (float)Math.PI * distance * distance;
        }

        public override (Vector3 direction, float distance) GetRay(Vector3 hitPoint)
        {
            var dir = _position - hitPoint;
            return (Vector3.Normalize(dir), dir.Length());
        }

        public override float EmitsLightInto(Vector3 lightDir)
        {
            return 1f;
        }

        public override bool Intersects(Vector3 origin, Vector3 direction, out float t)
        {
            t = 0;
            return false;
        }

        public override Vector3 Normal(Vector3 p)
        {
            return Vector3.Zero;
        }
    }
}