using System;
using System.Collections.Generic;
using RaytraceAir;

namespace Renderer
{
    class Program
    {
        static void Main()
        {
            var camera = new Camera(new Vec3(0, 0, 0), new Vec3(0, 1, 0).Normalized(), new Vec3(0, 0, -1).Normalized(), 60, 1980, 1260);
            var scene = new Scene(camera, new List<SceneObject>
            {
                new Sphere(new Vec3(-0.5, 0, -8), 0.5),
                new Sphere(new Vec3(0, 0, -10), 1),
                new Sphere(new Vec3(-2, 0, -10), 0.7),
                new Sphere(new Vec3(-1, -2, -10),  0.6),
                new Plane(new Vec3(0, -1, 0), new Vec3(0, 1, 0))
            },
            new List<Vec3>
            {
                new Vec3(4, 4, -10),
                new Vec3(-4, 4, -10),
                //new Vec3(0, 0, -8.5),
                //new Vec3(0, 0, -7.5),
            });

            scene.Render();
            scene.Export();
        }
    }
}
