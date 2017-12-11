using System.Numerics;

namespace RaytraceAir
{
    public abstract class SceneObject
    {
        protected SceneObject(Vector3 color, Material material)
        {
            Color = color;
            Material = material;
        }

        public abstract bool Intersects(Vector3 origin, Vector3 direction, out float t);
        public abstract Vector3 Normal(Vector3 p);

        public Vector3 Color { get; }
        public Material Material { get; }

        public float Albedo = 0.18f;
    }
}