using System;
using System.Collections.Generic;
using RaytraceAir;

namespace Renderer
{
    class Program
    {
        static void Main()
        {
            var camera = new Camera(
                position: new Vec3(0, 0, 0),
                upDirection: new Vec3(0, 1, 0).Normalized(),
                viewDirection: new Vec3(0, 0, -1).Normalized(),
                horizontalFoV: 60,
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
                    new Plane(new Vec3(0, -1, 0), new Vec3(0, 1, 0), new Vec3(1, 1, 1), isMirror: true)
                },
                new List<Light>
                {
                    new PointLight(new Vec3(0, 5, -10), new Vec3(1, 1, 1)),
                });

            scene.Render();
            BitmapExporter.Export(camera, "bmp");
        }
    }
}
