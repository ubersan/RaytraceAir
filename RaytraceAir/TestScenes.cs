using System.Collections.Generic;
using System.Numerics;

namespace RaytraceAir
{
    public static class TestScenes
    {
        public static Scene SpheresWithMirror()
        {
            var camera = GetBasicCamera();

            var scene = new Scene(
                camera,
                new List<SceneObject>
                {
                    new Sphere(new Vector3(-4, 0, -10), 0.75f, new Vector3(1, 0, 0)),
                    new Sphere(new Vector3(-2, 0, -10), 0.75f, new Vector3(1, 1, 0)),
                    new Sphere(new Vector3(0, 0, -10), 0.75f, new Vector3(0, 1, 0)),
                    new Sphere(new Vector3(2, 0, -10), 0.75f, new Vector3(0, 1, 1)),
                    new Sphere(new Vector3(4, 0, -10), 0.75f, new Vector3(0, 0, 1)),
                    new Plane(new Vector3(0, -1, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 1)),
                    new Rectangle(new Vector3(0, 1, -11), 9.5f, 1.5f, Vector3.Normalize(new Vector3(0, -0.3f, 1f)), Vector3.UnitX, new Vector3(0.8f, 0.4f, 0.3f), Material.Mirror)
                },
                new List<Light>
                {
                    new RectangularLight(new Vector3(1, 1, 1), new Vector3(0, 5, -10), 10f, 1f, -Vector3.UnitY, Vector3.UnitX)
                });

            return scene;
        }

        public static Scene FloorWithRectangularLight()
        {
            var camera = GetBasicCamera();

            var scene = new Scene(
                camera,
                new List<SceneObject>
                {
                    new Plane(pointOnPlane: new Vector3(0, -1, 0), normal: new Vector3(0, 1, 0), color: new Vector3(1, 1, 1)),
                },
                new List<Light>
                {
                    new RectangularLight(new Vector3(1, 1, 1), new Vector3(0, -0.99f, -10), 10f, 1f, -Vector3.UnitY, Vector3.UnitX)
                });

            return scene;
        }

        public static Scene FloorWithPointLight()
        {
            var camera = GetBasicCamera();

            var scene = new Scene(
                camera,
                new List<SceneObject>
                {
                    new Plane(pointOnPlane: new Vector3(0, -1, 0), normal: new Vector3(0, 1, 0), color: new Vector3(1, 1, 1)),
                },
                new List<Light>
                {
                    new PointLight(new Vector3(0, 0, -10), new Vector3(1, 1, 1)),
                });

            return scene;
        }

        private static Camera GetBasicCamera()
        {
            var camera = new Camera(
                position: new Vector3(0, 3, 10),
                upDirection: Vector3.Normalize(new Vector3(0, 10, -1.8f)),
                viewDirection: Vector3.Normalize(new Vector3(0, -1.8f, -10)),
                horizontalFoV: 30,
                widthInPixel: 1980,
                heightInPixel: 1260);

            return camera;
        }
    }
}