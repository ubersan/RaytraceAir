using System;
using System.Numerics;

namespace RaytraceAir
{
    public class RectangularLight : Light
    {
        private readonly Vector3 _center;
        private readonly Vector3 _normal;
        private readonly Vector3 _widthAxis;
        private readonly float _halfWidth;
        private readonly float _halfHeight;
        private readonly Vector3 _heightAxis;

        private readonly Random _random;

        public RectangularLight(Vector3 color, Vector3 center, float width, float height, Vector3 normal, Vector3 widthAxis)
            : base(color)
        {
            _center = center;
            _normal = normal;
            _widthAxis = widthAxis;
            _halfWidth = width / 2f;
            _halfHeight = height / 2f;
            _heightAxis = Vector3.Cross(normal, widthAxis);
            _random = new Random();
        }

        public override float GetFalloff(float distance)
        {
            return 4f * (float)Math.PI * distance * distance;
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

        public override float EmitsLightInto(Vector3 lightDir)
        {
            return Vector3.Dot(_normal, -lightDir) < 0 ? 0f : 1f;
        }

        public override bool Intersects(Vector3 origin, Vector3 direction, out float t)
        {
            // TODO: Code copied from Rectangle
            var denominator = Vector3.Dot(_normal, direction);
            t = 0;
            if (Math.Abs(denominator) > 1e-14)
            {
                t = Vector3.Dot(_center - origin, _normal) / denominator;
            }

            if (t >= 0)
            {
                var intersection = origin + t * direction;
                var inPlaneOffset = intersection - _center;

                var effWidht = Math.Abs(Vector3.Dot(inPlaneOffset, _widthAxis));
                var effHeight = Math.Abs(Vector3.Dot(inPlaneOffset, _heightAxis));

                var widhtOk = effWidht <= _halfWidth;
                var heightOk = effHeight <= _halfHeight;

                return widhtOk && heightOk;
            }

            return false;
        }

        public override Vector3 Normal(Vector3 p)
        {
            return _normal;
        }
    }
}