namespace RaytraceAir
{
    public abstract class SceneObject
    {
        public abstract bool Intersects(Vec3 origin, Vec3 direction, out double t);
        public abstract Vec3 Normal(Vec3 p);

        public double Albedo = 0.18;
    }
}