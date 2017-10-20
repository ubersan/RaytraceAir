using System;

namespace RaytraceAir
{
    public class Sphere : SceneObject
    {
        private readonly Vec3 _center;
        private readonly double _radius;

        public Sphere(Vec3 center, double radius, Vec3 color, bool isMirror = false)
            : base(color, isMirror)
        {
            _center = center;
            _radius = radius;
        }

        public override bool Intersects(Vec3 origin, Vec3 direction, out double t)
        {
            t = 0;
            var L = origin - _center;
            var a = direction.Dot(direction);
            var b = 2 * direction.Dot(L);
            var c = L.Dot(L) - _radius * _radius;

            if (!SolveQuadratic(a, b, c, out var t0, out var t1))
            {
                return false;
            }

            if (t0 > t1)
            {
                var temp = t1;
                t1 = t0;
                t0 = temp;
            }

            if (t0 < 0)
            {
                t0 = t1;
                if (t0 < 0)
                {
                    return false;
                }
            }

            t = t0;

            return true;
        }

        private bool SolveQuadratic(double a, double b, double c, out double x0, out double x1)
        {
            x0 = 0;
            x1 = 0;

            var discriminant = b * b - 4 * a * c;
            if (discriminant < 0)
            {
                return false;
            }

            if (Math.Abs(discriminant) < 1e-12)
            {
                x0 = x1 = -0.5 * b / a;
            }
            else
            {
                var q = b > 0
                    ? -0.5 * (b + Math.Sqrt(discriminant))
                    : -0.5 * (b - Math.Sqrt(discriminant));
                x0 = q / a;
                x1 = c / q;
            }

            if (x0 > x1)
            {
                var t = x1;
                x1 = x0;
                x0 = t;
            }

            return true;
        }

        public override Vec3 Normal(Vec3 p)
        {
            return (p - _center).Normalized();
        }
    }
}