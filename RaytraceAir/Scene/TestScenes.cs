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
            MultipleWhitePointLightsOnWhiteSphere(lowPixels),
            MultipleColoredPointLightsOnWhiteSphere(lowPixels),
            ThreeColoredLightsOnPlane(lowPixels),
        };

        public static Scene SpheresWithMirror(bool lowPixels = false)
        {
            var basicCamera = Camera.CreateBasicCamera(lowPixels);

            var scene = new Scene(
                basicCamera, 
                new List<SceneObject>
                {
                    new Sphere(new Vector3(-4, 0, -10), 0.75f, new Vector3(1, 0, 0), Material.Diffuse),
                    new Sphere(new Vector3(-2, 0, -10), 0.75f, new Vector3(1, 1, 0), Material.Diffuse),
                    new Sphere(new Vector3(0, 0, -10), 0.75f, new Vector3(0, 1, 0), Material.Diffuse),
                    new Sphere(new Vector3(2, 0, -10), 0.75f, new Vector3(0, 1, 1), Material.Diffuse),
                    new Sphere(new Vector3(4, 0, -10), 0.75f, new Vector3(0, 0, 1), Material.Diffuse),
                    new Plane(new Vector3(0, -1, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 1), Material.Diffuse),
                    new Rectangle(new Vector3(0, 1, -11), 9.5f, 1.5f, Vector3.Normalize(new Vector3(0, -0.3f, 1f)), Vector3.UnitX, new Vector3(0.8f, 0.4f, 0.3f), Material.Mirror),
                    new Point(new Vector3(0, 5, -8), new Vector3(1, 1, 1),  Material.Light),
                },
                new ProgressMonitor(basicCamera.NumberOfPixels),
                nameof(SpheresWithMirror));

            return scene;
        }

        public static Scene FloorWithRectangularLight(bool lowPixels = false)
        {
            var basicCamera = Camera.CreateBasicCamera(lowPixels);
            var scene = new Scene(
                basicCamera, 
                new List<SceneObject>
                {
                    new Plane(pointOnPlane: new Vector3(0, -1, 0), normal: new Vector3(0, 1, 0), color: new Vector3(1, 1, 1), material: Material.Diffuse),
                    new Rectangle(new Vector3(0, 0, -10), 10f, 1f, -Vector3.UnitY, Vector3.UnitX, Vector3.One, Material.Light),
                },
                new ProgressMonitor(basicCamera.NumberOfPixels),
                nameof(FloorWithRectangularLight));

            return scene;
        }

        public static Scene FloorWithPointLight(bool lowPixels = false)
        {
            var basicCamera = Camera.CreateBasicCamera(lowPixels);
            var scene = new Scene(
                basicCamera,
                new List<SceneObject>
                {
                    new Plane(pointOnPlane: new Vector3(0, -1, 0), normal: new Vector3(0, 1, 0), color: new Vector3(1, 1, 1), material: Material.Diffuse),
                    new Point(new Vector3(0, 0, -10), new Vector3(1, 1, 1), Material.Light),
                },
                new ProgressMonitor(basicCamera.NumberOfPixels),
                nameof(FloorWithPointLight));

            return scene;
        }

        public static Scene CornellBox(bool lowPixels = false)
        {
            var straightCamera = Camera.CreateStraightCamera(lowPixels);
            var scene = new Scene(
                straightCamera, 
                new List<SceneObject>
                {
                    new Plane(pointOnPlane: new Vector3(0, -4f, 0), normal: new Vector3(0, 1, 0), color: new Vector3(1, 1, 1), material: Material.Diffuse),
                    new Plane(pointOnPlane: new Vector3(0, 4f, 0), normal: new Vector3(0, -1, 0), color: new Vector3(1, 1, 1),  material: Material.Diffuse),
                    new Plane(pointOnPlane: new Vector3(-4f, 0, 0), normal: new Vector3(1, 0, 0), color: new Vector3(0, 0, 1), material: Material.Diffuse),
                    new Plane(pointOnPlane: new Vector3(4f, 0, 0), normal: new Vector3(-1, 0, 0), color: new Vector3(0, 1, 0), material: Material.Diffuse),
                    new Plane(pointOnPlane: new Vector3(0, 0, -4f), normal: new Vector3(0, 0, 1), color: new Vector3(1, 1, 1), material: Material.Diffuse),
                    new Sphere(center: new Vector3(-1f, -2f, 0), radius: 2f,  color: Vector3.One, material: Material.Mirror),
                    new Rectangle(center: new Vector3(0, 4f- 1e-2f, 0), width: 2f, height: 2f, normal: new Vector3(0, -1, 0), widthAxis: new Vector3(1, 0, 0), color: Vector3.One, material: Material.Light),
                },
                new ProgressMonitor(straightCamera.NumberOfPixels),
                nameof(CornellBox));

            return scene;
        }

        public static Scene MultipleWhitePointLightsOnWhiteSphere(bool lowPixels = false)
        {
            var straightTopCamera = Camera.CreateStraightTopCamera(lowPixels);
            var scene = new Scene(
                straightTopCamera,
                new List<SceneObject>
                {
                    new Plane(pointOnPlane: new Vector3(0, -1, 0), normal: Vector3.UnitY, color: Vector3.One,  material: Material.Diffuse),
                    new Sphere(center: new Vector3(0, 0, 0), radius: 1f, color: Vector3.One, material: Material.Diffuse),
                    new Point(position: new Vector3(4, 4, 0), color: Vector3.One, material: Material.Light),
                    new Point(position: new Vector3(-4, 4, 0), color: Vector3.One, material: Material.Light),
                },
                new ProgressMonitor(straightTopCamera.NumberOfPixels),
                nameof(MultipleWhitePointLightsOnWhiteSphere));

            return scene;
        }

        public static Scene MultipleColoredPointLightsOnWhiteSphere(bool lowPixels = false)
        {
            var straightTopCamera = Camera.CreateStraightTopCamera(lowPixels);
            var scene = new Scene(
                straightTopCamera,
                new List<SceneObject>
                {
                    new Plane(pointOnPlane: new Vector3(0, -1, 0), normal: Vector3.UnitY, color: Vector3.One,  material: Material.Diffuse),
                    new Sphere(center: new Vector3(0, 0, 0), radius: 1f, color: Vector3.One, material: Material.Diffuse),
                    new Point(position: new Vector3(4, 4, 0), color: new Vector3(0, 0, 1), material: Material.Light),
                    new Point(position: new Vector3(-4, 4, 0), color: new Vector3(1, 0, 0), material: Material.Light),
                },
                new ProgressMonitor(straightTopCamera.NumberOfPixels),
                nameof(MultipleColoredPointLightsOnWhiteSphere));

            return scene;
        }

        public static Scene ThreeColoredLightsOnPlane(bool lowPixels = false)
        {
            var straightTopCamera = Camera.CreateTopCamera(lowPixels);
            var scene = new Scene(
                straightTopCamera,
                new List<SceneObject>
                {
                    new Plane(pointOnPlane: new Vector3(0, -1, 0), normal: Vector3.UnitY, color: Vector3.One,  material: Material.Diffuse),
                    new Point(position: new Vector3(1, 3f, 0), color: new Vector3(0, 0, 1), material: Material.Light),
                    new Point(position: new Vector3(-1, 3f, 0), color: new Vector3(0, 1, 0), material: Material.Light),
                    new Point(position: new Vector3(-0, 3f, 2), color: new Vector3(1, 0, 0), material: Material.Light),
                },
                new ProgressMonitor(straightTopCamera.NumberOfPixels),
                nameof(ThreeColoredLightsOnPlane));

            return scene;
        }
    }
}