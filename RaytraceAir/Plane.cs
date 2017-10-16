using System;

namespace RaytraceAir
{
    public class Plane : SceneObject
    {
        private readonly Vec3 _pointOnPlane;
        private readonly Vec3 _normal;

        public Plane(Vec3 pointOnPlane, Vec3 normal)
        {
            _pointOnPlane = pointOnPlane;
            _normal = normal;
        }

        public override bool Intersects(Vec3 origin, Vec3 direction, out double t)
        {
            var denominator = _normal.Dot(direction);
            if (Math.Abs(denominator) > 1e-14)
            {
                t = (_pointOnPlane - origin).Dot(_normal) / denominator;
                return t >= 0;
            }

            t = 0;
            return false;
        }

        public override Vec3 Normal(Vec3 p)
        {
            return _normal;
        }
    }
}