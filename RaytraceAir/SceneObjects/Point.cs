using System;
using System.Collections.Generic;
using System.Numerics;

namespace RaytraceAir
{
    public class Point : SceneObject
    {
        private readonly Vector3 _position;

        public Point(Vector3 position, Vector3 color, Material material)
            : base(color, material)
        {
            _position = position;
        }

        public override float GetFalloff(float distance)
        {
            return 4f * (float)Math.PI * distance * distance;
        }

        public override bool EmitsLightInto(Vector3 lightDir)
        {
            return true;
        }

        public override IEnumerable<(Vector3 direction, float distance)> GetSamples(Vector3 hitPoint, int maxSamples)
        {
            var dir = _position - hitPoint;
            yield return (Vector3.Normalize(dir), dir.Length());
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