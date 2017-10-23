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

        public override float GetDistToLight(Vector3 hitPoint)
        {
            return (_position - hitPoint).Length();
        }

        public override Vector3 GetDirToLight(Vector3 hitPoint)
        {
            return Vector3.Normalize(_position - hitPoint);
        }
    }
}