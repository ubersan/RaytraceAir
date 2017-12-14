using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using RaytraceAir;
using Plane = RaytraceAir.Plane;

namespace Renderer
{
    internal static class Renderer
    {
        private static void Main(string[] argv)
        {
            var rendererMode = CheckArguments(argv);

            switch (rendererMode)
            {
                case RendererMode.RenderScene:
                    RenderScene();
                    break;
                case RendererMode.ProduceTestReferences:
                    ProduceTestReferences();
                    break;
                case RendererMode.Invalid:
                    Console.WriteLine("Usage: Renderer [-T]");
                    Console.WriteLine("-T\tGenerate reference images for testing");
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void ProduceTestReferences()
        {
            if (Directory.Exists(AppEnvironment.TestReferenceFolder))
            {
                Directory.Delete(AppEnvironment.TestReferenceFolder, true);
            }

            Directory.CreateDirectory(AppEnvironment.TestReferenceFolder);

            foreach (var testScene in TestScenes.AllScenes(lowPixels: true))
            {
                RenderSingleFrame(testScene, AppEnvironment.TestReferenceFolder, testScene.Name);
            }
        }

        private static void RenderScene()
        {
            RenderSingleFrame(TestScenes.CornellBox(), AppEnvironment.OutputFolder, "render");
            //RenderAnimationFrames(numSteps: 3);
        }

        private static RendererMode CheckArguments(IReadOnlyList<string> argv)
        {
            switch (argv.Count)
            {
                case 0:
                    return RendererMode.RenderScene;
                case 1 when string.CompareOrdinal(argv[0], "-T") == 0:
                    return RendererMode.ProduceTestReferences;
                default:
                    return RendererMode.Invalid;
            }
        }

        private static void RenderSingleFrame(Scene scene, string targetFolder, string outputName)
        {
            var renderTask = Task.Factory.StartNew(scene.Render);

            while (!renderTask.IsCompleted)
            {
                Console.Write(ProgressString(scene));
                Thread.Sleep(50);
            }
            Console.WriteLine(ProgressString(scene));

            BitmapExporter.Export(scene.Camera, targetFolder, outputName);
        }

        private static string ProgressString(Scene scene) => $"\rScene {scene.Name}: { scene.ProgressMonitor.Progress() }";

        private static void RenderAnimationFrames(int numSteps)
        {
            var start = new Vector3(-5, 5, -8);
            var end = new Vector3(5, 5, -8);
            var step = (end - start) / numSteps;

            var targetFolder = Path.Combine(AppEnvironment.OutputFolder, $"{ Guid.NewGuid() }");

            for (var i = 0; i < numSteps; ++i)
            {
                var camera = Camera.CreateBasicCamera();

                var scene = new Scene(
                    camera,
                    new List<SceneObject>
                    {
                        new Sphere(new Vector3(-4, 0, -10), 0.75f, new Vector3(1, 0, 0)),
                        new Sphere(new Vector3(-2, 0, -10), 0.75f, new Vector3(1, 1, 0)),
                        new Sphere(new Vector3(-0, 0, -10), 0.75f, new Vector3(0, 1, 0)),
                        new Sphere(new Vector3(2, 0, -10), 0.75f, new Vector3(0, 1, 1)),
                        new Sphere(new Vector3(4, 0, -10), 0.75f, new Vector3(0, 0, 1)),
                        new Plane(new Vector3(0, -1, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 1)),
                        new Point(start + i * step, new Vector3(1, 1, 1), Material.Light),
                    },
                    new ProgressMonitor(camera.NumberOfPixels),
                    "render");

                scene.Render();
                BitmapExporter.Export(camera, targetFolder, $@"frame_{PrependWithZerosIfNeeded(i.ToString(), 4)}.jpg");

                Console.WriteLine($"Rendered frame {i}");
            }
        }

        private static string PrependWithZerosIfNeeded(string name, int len)
        {
            while (name.Length < len)
            {
                name = "0" + name;
            }

            return name;
        }
    }
}
