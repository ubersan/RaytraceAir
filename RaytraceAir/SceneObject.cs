namespace RaytraceAir
{
    public abstract class SceneObject
    {
        protected SceneObject(Vec3 color, bool isMirror = false)
        {
            Color = color;
            IsMirror = isMirror;
        }

        public abstract bool Intersects(Vec3 origin, Vec3 direction, out double t);
        public abstract Vec3 Normal(Vec3 p);

        public Vec3 Color { get; }
        public bool IsMirror { get; }

        public double Albedo = 0.18;
    }
}