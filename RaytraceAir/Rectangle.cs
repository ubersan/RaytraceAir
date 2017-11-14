using System;
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

        public Rectangle(Vector3 center, float width, float height, Vector3 normal, Vector3 widthAxis, Vector3 color, Material material = Material.Diffuse)
            : base(center, normal, color, material)
        {
            _center = center;
            _halfWidth = width / 2f;
            _halfHeight = height / 2f;
            _widthAxis = widthAxis;
            _heightAxis = Vector3.Cross(normal, widthAxis);
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
    }
}