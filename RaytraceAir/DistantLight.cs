namespace RaytraceAir
{
    public class DistantLight : Light
    {
        private readonly Vec3 _direction;

        public DistantLight(Vec3 direction, Vec3 color)
            : base(color)
        {
            _direction = direction;
        }

        public override double GetFalloff(double distance)
        {
            return 1;
        }

        public override double GetDistToLight(Vec3 hitPoint)
        {
            return double.MaxValue;
        }

        public override Vec3 GetDirToLight(Vec3 hitPoint)
        {
            return -_direction;
        }
    }
}