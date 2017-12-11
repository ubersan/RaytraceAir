using System.Numerics;

namespace RaytraceAir
{
    public class DistantLight : Light
    {
        private readonly Vector3 _direction;

        public DistantLight(Vector3 direction, Vector3 color)
            : base(color)
        {
            _direction = direction;
        }

        public override float GetFalloff(float distance)
        {
            return 1;
        }

        public override (Vector3 direction, float distance) GetRay(Vector3 hitPoint)
        {
            return (_direction, float.MaxValue);
        }

        public override float EmitsLightInto(Vector3 lightDir)
        {
            return 1f;
        }

        public override bool Intersects(Vector3 origin, Vector3 direction, out float t)
        {
            t = 0;
            return false;
        }

        public override Vector3 Normal(Vector3 p)
        {
            return  Vector3.Zero;
        }
    }
}