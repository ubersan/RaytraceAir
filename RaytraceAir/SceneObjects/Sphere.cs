using System;
using System.Collections.Generic;
using System.Numerics;

namespace RaytraceAir
{
    public class Sphere : SceneObject
    {
        private readonly Vector3 _center;
        private readonly float _radius;

        public Sphere(Vector3 center, float radius, Vector3 color, Material material)
            : base(color, material)
        {
            _center = center;
            _radius = radius;
        }

        public override bool Intersects(Vector3 origin, Vector3 direction, out float t)
        {
            t = 0;
            var L = origin - _center;
            var a = Vector3.Dot(direction, direction);
            var b = 2 * Vector3.Dot(direction, L);
            var c = Vector3.Dot(L, L) - _radius * _radius;

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

        private bool SolveQuadratic(float a, float b, float c, out float x0, out float x1)
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
                x0 = x1 = -0.5f * b / a;
            }
            else
            {
                var q = b > 0
                    ? -0.5f * (b + (float)Math.Sqrt(discriminant))
                    : -0.5f * (b - (float)Math.Sqrt(discriminant));
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

        public override Vector3 Normal(Vector3 p)
        {
            return Vector3.Normalize(p - _center);
        }

        public override float GetFalloff(float distance)
        {
            // TODO: move into base class
            return 4f * (float)Math.PI * distance * distance;
        }

        public override (Vector3 direction, float distance) GetRay(Vector3 hitPoint)
        {
            throw new NotImplementedException();
        }

        public override float EmitsLightInto(Vector3 lightDir)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<(Vector3 direction, float distance)> GetSamples(Vector3 hitPoint, int maxSamples)
        {
            throw new NotImplementedException();
        }
    }
}