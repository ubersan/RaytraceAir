using System;
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
                    new PointLight(new Vector3(0, 5, -8), new Vector3(1, 1, 1)),
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
                    new RectangularLight(new Vector3(1, 1, 1), new Vector3(0, 0, -10), 10f, 1f, -Vector3.UnitY, Vector3.UnitX)
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

        public static Scene CornellBox()
        {
            var camera = GetStraigthCamera();

            var d = 4f;
            var scene = new  Scene(
                camera,
                new List<SceneObject>
                {
                    new Plane(pointOnPlane: new Vector3(0, -d, 0), normal: new Vector3(0, 1, 0), color: new Vector3(1, 1, 1)),
                    new Plane(pointOnPlane: new Vector3(0, d, 0), normal: new Vector3(0, -1, 0), color: new Vector3(1, 1, 1)),
                    new Plane(pointOnPlane: new Vector3(-d, 0, 0), normal: new Vector3(1, 0, 0), color: new Vector3(0, 0, 1)),
                    new Plane(pointOnPlane: new Vector3(d, 0, 0), normal: new Vector3(-1, 0, 0), color: new Vector3(0, 1, 0)),
                    new Plane(pointOnPlane: new Vector3(0, 0, -d), normal: new Vector3(0, 0, 1), color: new Vector3(1, 1, 1)),
                    new Sphere(center: new Vector3(-1f, -2f, 0), radius: 2f,  color: Vector3.One),
                },
                new List<Light>
                {
                    new RectangularLight(color: Vector3.One, center: new Vector3(0, d - 1e-2f, 0), width: 0.1f, height: 0.1f, normal: new Vector3(0, -1, 0), widthAxis: new Vector3(1, 0, 0))
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

        private static Camera GetStraigthCamera()
        {
            var camera = new Camera(
                position: new Vector3(0, 0, 30),
                upDirection: Vector3.Normalize(new Vector3(0, 1, 0)),
                viewDirection: Vector3.Normalize(new Vector3(0, 0, -1)),
                horizontalFoV: 30,
                widthInPixel: 1980,
                heightInPixel: 1260);

            return camera;
        }
    }
}