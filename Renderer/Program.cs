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
                new Sphere(new Vec3(0, 0, -10), 1)
            },
            new List<Vec3>
            {
                new Vec3(0, 0, -8)
            });
            scene.Trace();
            scene.Render();
        }
    }
}
