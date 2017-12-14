using System;
using System.Collections.Generic;
using System.Numerics;

namespace RaytraceAir
{
    public class Rectangle : Plane
    {
        private readonly Vector3 _center;
        private readonly float _halfWidth;
        private readonly float _halfHeight;
        private readonly Vector3 _widthAxis;
        private readonly Vector3 _heightAxis;
        private readonly Random _random;

        public Rectangle(Vector3 center, float width, float height, Vector3 normal, Vector3 widthAxis, Vector3 color, Material material)
            : base(center, normal, color, material)
        {
            _center = center;
            _halfWidth = width / 2f;
            _halfHeight = height / 2f;
            _widthAxis = widthAxis;
            _heightAxis = Vector3.Cross(normal, widthAxis);
            _random = new Random();
        }

        public override bool Intersects(Vector3 origin, Vector3 direction, out float t)
        {
            if (base.Intersects(origin, direction, out t))
            {
                var intersection = origin + t * direction;
                var inPlaneOffset = intersection - _center;

                return Math.Abs(Vector3.Dot(inPlaneOffset, _widthAxis)) <= _halfWidth
                       && Math.Abs(Vector3.Dot(inPlaneOffset, _heightAxis)) <= _halfHeight;
            }

            return false;
        }

        public override (Vector3 direction, float distance) GetRay(Vector3 hitPoint)
        {
            // sample random point in rectangle
            var widthSample = ((float)_random.NextDouble() - 0.5f) * _halfWidth;
            var heightSample = ((float)_random.NextDouble() - 0.5f) * _halfHeight;

            var samplePoint = _center + _widthAxis * widthSample + _heightAxis * heightSample;
            var direction = samplePoint - hitPoint;

            return (Vector3.Normalize(direction), direction.Length());
        }

        public override IEnumerable<(Vector3 direction, float distance)> GetSamples(Vector3 hitPoint, int maxSamples)
        {
            for (var i = 0; i < maxSamples; ++i)
            {
                yield return GetRay(hitPoint);
            }
        }
    }
}