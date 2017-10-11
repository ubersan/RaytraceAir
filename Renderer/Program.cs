using System;
using System.Collections.Generic;
using RaytraceAir;

namespace Renderer
{
    class Program
    {
        static void Main(string[] args)
        {
            var camera = new Camera(Vec3.Zeros(), new Vec3(0,  1, 0), new Vec3(0, 0, -1), 60, 1980, 1260);
            var scene = new Scene(camera, new List<Sphere>
            {
                new Sphere(new Vec3(0, 0, -10), 1),
                new Sphere(new Vec3(-2, 0, -10), 0.7),
                new Sphere(new Vec3(-1, -2, -12),  0.6)
            },
            new List<Vec3>
            {
                new Vec3(2, 2, -10)
            });

            scene.Trace();
            scene.Render();
        }
    }
}
