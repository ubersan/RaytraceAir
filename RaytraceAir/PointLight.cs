using System;

namespace RaytraceAir
{
    public class PointLight : Light
    {
        private readonly Vec3 _position;

        public PointLight(Vec3 position, Vec3 color)
            : base(color)
        {
            _position = position;
        }

        public override double GetFalloff(double distance)
        {
            return 4 * Math.PI * distance * distance;
        }

        public override double GetDistToLight(Vec3 hitPoint)
        {
            return (_position - hitPoint).Norm;
        }

        public override Vec3 GetDirToLight(Vec3 hitPoint)
        {
            return (_position - hitPoint).Normalized();
        }
    }
}