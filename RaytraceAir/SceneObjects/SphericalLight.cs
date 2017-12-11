using System;
using System.Numerics;

namespace RaytraceAir
{
    public class SphericalLight : PointLight
    {
        private readonly float _radius;

        public SphericalLight(Vector3 position, float radius, Vector3 color)
            : base(position, color)
        {
            _radius = radius;
        }

        public override (Vector3 direction, float distance) GetRay(Vector3 hitPoint)
        {
            // sample unit sphere
            var rand = new Random();
            var pointOnSphere = Vector3.Zero;

            while (pointOnSphere.Length() < 1e-4)
            {
                pointOnSphere = new Vector3(
                    (float)rand.NextDouble(),
                    (float)rand.NextDouble(),
                    (float)rand.NextDouble());
            }

            var dir = Vector3.Normalize(pointOnSphere) * _radius - hitPoint;
            return (Vector3.Normalize(dir), dir.Length());
        }
    }
}