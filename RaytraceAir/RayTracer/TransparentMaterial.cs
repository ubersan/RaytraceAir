using System;
using System.Numerics;

namespace RaytraceAir
{
    public class TransparentMaterial : MaterialTracer
    {
        public override Vector3 GetColorContribution(Vector3 originShadowRay, Vector3 dir, Vector3 hitPoint, SceneObject hitSceneObject, int depth, SceneObject light, Vector3 lightDir, float lightDist)
        {
            var hitNormal = hitSceneObject.Normal(hitPoint);
            var kr = Fresnel(dir, hitNormal, 1.5f);
            var outside = Vector3.Dot(dir, hitNormal) < 0;
            var bias = 1e-4f * hitNormal;
            var refractionColor = Vector3.Zero;
            if (kr < 1)
            {
                var refractionDir = Vector3.Normalize(GetRefractionDir(dir, hitNormal, 1.5f));
                var refractionorig = outside ? hitPoint - bias : hitPoint + bias;
                refractionColor = RayTracer.CastRay(refractionorig, refractionDir, depth + 1);
            }
            var reflectionDir = Vector3.Normalize(GetReflectionDir(dir, hitNormal));
            var reflectionOrig = outside ? hitPoint + bias : hitPoint - bias;
            var reflectionColor = RayTracer.CastRay(reflectionOrig, reflectionDir, depth + 1);

            return reflectionColor * kr + refractionColor * (1 - kr);
        }

        public override MaterialType MaterialType { get; } = MaterialType.Transparent;

        private static Vector3 GetRefractionDir(Vector3 viewDir, Vector3 normal, float ior)
        {
            var cosi = Clamp(-1, 1, Vector3.Dot(viewDir, normal));
            float etai = 1, etat = ior;

            Vector3 n;
            if (cosi < 0)
            {
                cosi = -cosi;
            }
            {
                // swap etai,  etat
                var t = etai;
                etai = etat;
                etat = t;
                n = -normal;
            }

            var eta = etai / etat;
            var k = 1 - eta * eta * (1 - cosi * cosi);
            return k < 0 ? Vector3.Zero : eta * viewDir + (eta * cosi - (float)Math.Sqrt(k)) * n;
        }

        private static float Fresnel(Vector3 viewDir, Vector3 normal, float ior)
        {
            var cosi = Clamp(-1, 1, Vector3.Dot(viewDir, normal));
            float etai = 1, etat = ior;

            if (cosi > 0)
            {
                var t = etai;
                etai = etat;
                etat = t;
            }

            var sint = etai / etat * (float)Math.Sqrt(Math.Max(0, 1 - cosi * cosi));
            if (sint >= 1)
            {
                return 1;
            }

            var cost = (float)Math.Sqrt(Math.Max(0, 1 - sint * sint));
            cosi = Math.Abs(cosi);
            var rs = (etat * cosi - etai * cost) / (etat * cosi + etai * cost);
            var rp = (etai * cosi - etat * cost) / (etai * cosi + etat * cost);

            return (rs * rs + rp * rp) / 2;
        }

        private static float Clamp(float lo, float hi, float value)
        {
            return Math.Max(lo, Math.Min(hi, value));
        }
    }
}