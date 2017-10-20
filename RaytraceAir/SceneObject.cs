namespace RaytraceAir
{
    public abstract class SceneObject
    {
        public SceneObject(Vec3 color)
        {
            Color = color;
        }

        public abstract bool Intersects(Vec3 origin, Vec3 direction, out double t);
        public abstract Vec3 Normal(Vec3 p);

        public Vec3 Color { get; }

        public double Albedo = 0.18;
    }
}