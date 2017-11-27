using System;
using System.ComponentModel;
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

        public RectangularLight(Vector3 color, Vector3 center, float width, float height, Vector3 normal, Vector3 widthAxis)
            : base(color)
        {
            _center = center;
            _normal = normal;
            _widthAxis = widthAxis;
            _halfWidth = width / 2f;
            _halfHeight = height / 2f;
            _heightAxis = Vector3.Cross(normal, widthAxis);
        }

        public override float GetFalloff(float distance)
        {
            return 4f * (float)Math.PI * distance * distance;
        }

        public override (Vector3 direction, float distance) GetRay(Vector3 hitPoint)
        {
            // sample random point in rectangle
            var random = new Random();
            var widthSample = ((float)random.NextDouble() - 0.5f) * _halfWidth;
            var heightSample = ((float)random.NextDouble() - 0.5f) * _halfWidth;

            var samplePoint = _center + _widthAxis * widthSample + _heightAxis * heightSample;
            var direction = samplePoint - hitPoint;

            return (Vector3.Normalize(direction), direction.Length());
        }
    }
}