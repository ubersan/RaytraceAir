using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace RaytraceAir
{
    public abstract class SceneObject
    {
        protected SceneObject(Vector3 color, params MaterialTracer[] materials)
        {
            Color = color;
            Materials = materials;
        }

        public abstract bool Intersects(Vector3 origin, Vector3 direction, out float t);
        public abstract Vector3 Normal(Vector3 p);

        // light properties
        public abstract float GetFalloff(float distance);
        public abstract bool EmitsLightInto(Vector3 lightDir);
        public abstract IEnumerable<(Vector3 direction, float distance)> GetSamples(Vector3 hitPoint, int maxSamples);

        public Vector3 Color { get; }
        public MaterialTracer[] Materials { get; }

        // TODO: Refactor.
        public bool IsLight => Materials.SingleOrDefault(material => material.MaterialType == MaterialType.Light) != null;
        public bool IsTransparent => Materials.SingleOrDefault(material => material.MaterialType == MaterialType.Transparent) != null;

        public const float Albedo = 0.18f;
    }
}