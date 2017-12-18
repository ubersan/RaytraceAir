using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace RaytraceAir
{
    public class Scene
    {
        private readonly List<SceneObject> _sceneObjects;
        private readonly Vector3 _background = Vector3.Zero;
        private const int MaxRecursionDepth = 5;
        private const int MaxLightSamples = 5;

        public Scene(Camera camera, List<SceneObject> sceneObjects, ProgressMonitor progressMonitor, string name)
        {
            Camera = camera;
            Name = name;
            _sceneObjects = sceneObjects;
            ProgressMonitor = progressMonitor;
        }

        public Camera Camera { get; }
        public string Name { get; }

        public ProgressMonitor ProgressMonitor { get; }

        private IEnumerable<SceneObject> SceneObjectsWithoutLights => _sceneObjects.Where(sceneObject => sceneObject.Material != Material.Light);
        private IEnumerable<SceneObject> Lights => _sceneObjects.Where(sceneObject => sceneObject.Material == Material.Light);

        public void Render()
        {
            RayTracer.SetSceneObjects(_sceneObjects);

            ProgressMonitor.Start();

            foreach (var pixel in Rasterizer.GetPixels(Camera))
            {
                if (pixel.I == 960 && pixel.J == 540)
                {
                    var i = 313;
                }
                var originPrimaryRay = Camera.Position;
                var dir = Vector3.Normalize(Camera.ViewDirection + pixel.X * Camera.RightDirection + pixel.Y * Camera.UpDirection);

                Camera.Pixels[pixel.I, pixel.J] = RayTracer.CastRay(originPrimaryRay, dir, depth: 0);

                ProgressMonitor.Advance();
            }
        }
    }
}