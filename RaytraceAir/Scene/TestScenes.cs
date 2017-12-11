using System.Collections.Generic;
using System.Numerics;

namespace RaytraceAir
{
    public static class TestScenes
    {
        public static IEnumerable<Scene> AllScenes(bool lowPixels = false) => new List<Scene>
        {
            CornellBox(lowPixels),
            FloorWithPointLight(lowPixels),
            FloorWithRectangularLight(lowPixels),
            SpheresWithMirror(lowPixels),
        };

        public static Scene SpheresWithMirror(bool lowPixels = false)
        {
            var scene = new Scene(
                Camera.CreateBasicCamera(lowPixels), 
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
                },
                nameof(SpheresWithMirror));

            return scene;
        }

        public static Scene FloorWithRectangularLight(bool lowPixels = false)
        {
            var scene = new Scene(
                Camera.CreateBasicCamera(lowPixels), 
                new List<SceneObject>
                {
                    new Plane(pointOnPlane: new Vector3(0, -1, 0), normal: new Vector3(0, 1, 0), color: new Vector3(1, 1, 1)),
                },
                new List<Light>
                {
                    new RectangularLight(new Vector3(1, 1, 1), new Vector3(0, 0, -10), 10f, 1f, -Vector3.UnitY, Vector3.UnitX)
                },
                nameof(FloorWithRectangularLight));

            return scene;
        }

        public static Scene FloorWithPointLight(bool lowPixels = false)
        {
            var scene = new Scene(
                Camera.CreateBasicCamera(lowPixels), 
                new List<SceneObject>
                {
                    new Plane(pointOnPlane: new Vector3(0, -1, 0), normal: new Vector3(0, 1, 0), color: new Vector3(1, 1, 1)),
                },
                new List<Light>
                {
                    new PointLight(new Vector3(0, 0, -10), new Vector3(1, 1, 1)),
                },
                nameof(FloorWithPointLight));

            return scene;
        }

        public static Scene CornellBox(bool lowPixels = false)
        {
            var d = 4f;
            var scene = new  Scene(
                Camera.CreateStraightCamera(lowPixels), 
                new List<SceneObject>
                {
                    new Plane(pointOnPlane: new Vector3(0, -d, 0), normal: new Vector3(0, 1, 0), color: new Vector3(1, 1, 1)),
                    new Plane(pointOnPlane: new Vector3(0, d, 0), normal: new Vector3(0, -1, 0), color: new Vector3(1, 1, 1)),
                    new Plane(pointOnPlane: new Vector3(-d, 0, 0), normal: new Vector3(1, 0, 0), color: new Vector3(0, 0, 1)),
                    new Plane(pointOnPlane: new Vector3(d, 0, 0), normal: new Vector3(-1, 0, 0), color: new Vector3(0, 1, 0)),
                    new Plane(pointOnPlane: new Vector3(0, 0, -d), normal: new Vector3(0, 0, 1), color: new Vector3(1, 1, 1)),
                    new Sphere(center: new Vector3(-1f, -2f, 0), radius: 2f,  color: Vector3.One, material: Material.Mirror),
                },
                new List<Light>
                {
                    new RectangularLight(color: Vector3.One, center: new Vector3(0, d - 1e-2f, 0), width: 2f, height: 2f, normal: new Vector3(0, -1, 0), widthAxis: new Vector3(1, 0, 0))
                },
                nameof(CornellBox));

            return scene;
        }
    }
}