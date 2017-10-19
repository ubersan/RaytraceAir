namespace RaytraceAir
{
    public abstract class Light
    {
        protected Light(Vec3 color)
        {
            Color = color;
        }

        public Vec3 Color { get; }

        public abstract double GetFalloff(double distance);
        public abstract double GetDistToLight(Vec3 hitPoint);
        public abstract Vec3 GetDirToLight(Vec3 hitPoint);
    }
}