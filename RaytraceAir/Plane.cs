﻿using System;
using System.Numerics;

namespace RaytraceAir
{
    public class Plane : SceneObject
    {
        private readonly Vector3 _pointOnPlane;
        private readonly Vector3 _normal;

        public Plane(Vector3 pointOnPlane, Vector3 normal, Vector3 color, Material material =  Material.Diffuse)
            : base(color, material)
        {
            _pointOnPlane = pointOnPlane;
            _normal = normal;
        }

        public override bool Intersects(Vector3 origin, Vector3 direction, out float t)
        {
            var denominator = Vector3.Dot(_normal, direction);
            if (Math.Abs(denominator) > 1e-14)
            {
                t = Vector3.Dot(_pointOnPlane - origin, _normal) / denominator;
                return t >= 0;
            }

            t = 0;
            return false;
        }

        public override Vector3 Normal(Vector3 p)
        {
            return _normal;
        }
    }
}