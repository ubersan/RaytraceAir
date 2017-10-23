using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using RaytraceAir;

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
                position: new Vec3(0, 3, 10),
                upDirection: new Vec3(0, 10, -1.8).Normalized(),
                viewDirection: new Vec3(0, -1.8, -10).Normalized(),
                horizontalFoV: 30,
                widthInPixel: 1980,
                heightInPixel: 1260);

            var scene = new Scene(
                camera,
                new List<SceneObject>
                {
                    new Sphere(new Vec3(-4, 0, -10), 0.75, new Vec3(1, 0, 0)),
                    new Sphere(new Vec3(-2, 0, -10), 0.75, new Vec3(1, 1, 0)),
                    new Sphere(new Vec3(-0, 0, -10), 0.75, new Vec3(0, 1, 0)),
                    new Sphere(new Vec3(2, 0, -10), 0.75, new Vec3(0, 1, 1)),
                    new Sphere(new Vec3(4, 0, -10), 0.75, new Vec3(0, 0, 1)),
                    new Plane(new Vec3(0, -1, 0), new Vec3(0, 1, 0), new Vec3(1, 1, 1))
                },
                new List<Light>
                {
                    new PointLight(new Vec3(0, 5, -8), new Vec3(1, 1, 1)),
                });

            scene.Render();
            BitmapExporter.Export(camera, "bmp");
        }

        private static void RenderAnimationFrames(int numSteps)
        {
            var folder = Directory.CreateDirectory($@"..\..\..\Output\{Guid.NewGuid().ToString()}");

            var start = new Vec3(-5, 5, -8);
            var end = new Vec3(5, 5, -8);
            var step = (end - start) / numSteps;

            for (var i = 0; i < numSteps; ++i)
            {
                var camera = new Camera(
                    position: new Vec3(0, 3, 10),
                    upDirection: new Vec3(0, 10, -1.8).Normalized(),
                    viewDirection: new Vec3(0, -1.8, -10).Normalized(),
                    horizontalFoV: 30,
                    widthInPixel: 1980,
                    heightInPixel: 1260);

                var scene = new Scene(
                    camera,
                    new List<SceneObject>
                    {
                        new Sphere(new Vec3(-4, 0, -10), 0.75, new Vec3(1, 0, 0)),
                        new Sphere(new Vec3(-2, 0, -10), 0.75, new Vec3(1, 1, 0)),
                        new Sphere(new Vec3(-0, 0, -10), 0.75, new Vec3(0, 1, 0)),
                        new Sphere(new Vec3(2, 0, -10), 0.75, new Vec3(0, 1, 1)),
                        new Sphere(new Vec3(4, 0, -10), 0.75, new Vec3(0, 0, 1)),
                        new Plane(new Vec3(0, -1, 0), new Vec3(0, 1, 0), new Vec3(1, 1, 1))
                    },
                    new List<Light>
                    {
                        new PointLight(start + i * step, new Vec3(1, 1, 1)),
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
