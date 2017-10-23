using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using RaytraceAir;
using Plane = RaytraceAir.Plane;

namespace Renderer
{
    class Program
    {
        static void Main()
        {
            var sw = new Stopwatch();
            sw.Start();

            //RenderSingleFrame();
            RenderAnimationFrames(numSteps: 150);

            sw.Stop();

            Console.WriteLine($"Elapsed time: {sw.ElapsedMilliseconds / 1000.0} sec");
            Console.ReadKey();
        }

        private static void RenderSingleFrame()
        {
            var camera = new Camera(
                position: new Vector3(0, 3, 10),
                upDirection: Vector3.Normalize(new Vector3(0, 10, -1.8f)),
                viewDirection: Vector3.Normalize(new Vector3(0, -1.8f, -10)),
                horizontalFoV: 30,
                widthInPixel: 1980,
                heightInPixel: 1260);

            var scene = new Scene(
                camera,
                new List<SceneObject>
                {
                    new Sphere(new Vector3(-4, 0, -10), 0.75f, new Vector3(1, 0, 0)),
                    new Sphere(new Vector3(-2, 0, -10), 0.75f, new Vector3(1, 1, 0)),
                    new Sphere(new Vector3(-0, 0, -10), 0.75f, new Vector3(0, 1, 0)),
                    new Sphere(new Vector3(2, 0, -10), 0.75f, new Vector3(0, 1, 1)),
                    new Sphere(new Vector3(4, 0, -10), 0.75f, new Vector3(0, 0, 1)),
                    new Plane(new Vector3(0, -1, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 1))
                },
                new List<Light>
                {
                    new PointLight(new Vector3(0, 5, -8), new Vector3(1, 1, 1)),
                });

            scene.Render();
            BitmapExporter.Export(camera, "bmp");
        }

        private static void RenderAnimationFrames(int numSteps)
        {
            var folder = Directory.CreateDirectory($@"..\..\..\Output\{Guid.NewGuid().ToString()}");

            var start = new Vector3(-5, 5, -8);
            var end = new Vector3(5, 5, -8);
            var step = (end - start) / numSteps;

            for (var i = 0; i < numSteps; ++i)
            {
                var camera = new Camera(
                    position: new Vector3(0, 3, 10),
                    upDirection: Vector3.Normalize(new Vector3(0, 10, -1.8f)),
                    viewDirection: Vector3.Normalize(new Vector3(0, -1.8f, -10)),
                    horizontalFoV: 30,
                    widthInPixel: 1980,
                    heightInPixel: 1260);

                var scene = new Scene(
                    camera,
                    new List<SceneObject>
                    {
                        new Sphere(new Vector3(-4, 0, -10), 0.75f, new Vector3(1, 0, 0)),
                        new Sphere(new Vector3(-2, 0, -10), 0.75f, new Vector3(1, 1, 0)),
                        new Sphere(new Vector3(-0, 0, -10), 0.75f, new Vector3(0, 1, 0)),
                        new Sphere(new Vector3(2, 0, -10), 0.75f, new Vector3(0, 1, 1)),
                        new Sphere(new Vector3(4, 0, -10), 0.75f, new Vector3(0, 0, 1)),
                        new Plane(new Vector3(0, -1, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 1))
                    },
                    new List<Light>
                    {
                        new PointLight(start + i * step, new Vector3(1, 1, 1)),
                    });

                scene.Render();
                BitmapExporter.Export(camera, $@"{folder}\frame_{PrependWithZerosIfNeeded(i.ToString(), 4)}");
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
