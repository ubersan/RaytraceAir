using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaytraceAir;

namespace RaytraceAirTest
{
    [TestClass]
    public class RenderTests
    {
        private const double LoMaxAverageError = 0;
        private const double MidMaxAverageError = 2;
        private const double HiMaxAverageError = 10;

        private Scene _scene;
        private string _actualImagePath;
        private string _referenceImagePath;

        #region Test Methods

        [TestMethod]
        public void CornellBox_Render_ResultsMatchReferences()
        {
            Given_SceneCornellBox();
            When_RenderAndExportScene();
            Then_RenderedImageMatchesReferenceWithMediumTolerance();
        }

        [TestMethod]
        public void FloorWithPointLight_Render_ResultsMatchReferences()
        {
            Given_SceneFloorWithPointLight();
            When_RenderAndExportScene();
            Then_RenderedImageMatchesReferenceWithLowTolerance();
        }

        [TestMethod]
        public void FloorWithRectangularLight_Render_ResultsMatchReferences()
        {
            Given_SceneFloorWithRectangularLight();
            When_RenderAndExportScene();
            Then_RenderedImageMatchesReferenceWithHighTolerance();
        }

        [TestMethod]
        public void SpheresWithMirror_Render_ResultsMatchReferences()
        {
            Given_SceneSpheresWithMirror();
            When_RenderAndExportScene();
            Then_RenderedImageMatchesReferenceWithLowTolerance();
        }

        #endregion

        #region Given, When, Then Methods

        private void Given_SceneCornellBox()
        {
            Set_Scene(TestScenes.CornellBox);
        }

        private void Given_SceneFloorWithPointLight()
        {
            Set_Scene(TestScenes.FloorWithPointLight);
        }

        private void Given_SceneFloorWithRectangularLight()
        {
            Set_Scene(TestScenes.FloorWithRectangularLight);
        }

        private void Given_SceneSpheresWithMirror()
        {
            Set_Scene(TestScenes.SpheresWithMirror);
        }

        private void When_RenderAndExportScene()
        {
            _scene.Render();

            var directory = Directory.CreateDirectory(AppEnvironment.TempFolderInTestResults);

            _actualImagePath = BitmapExporter.Export(_scene.Camera, directory.FullName, _scene.Name);
            _referenceImagePath = Path.Combine(AppEnvironment.TestReferenceFolder, $"{_scene.Name}.jpg");
        }

        private void Then_RenderedImageMatchesReferenceWithLowTolerance()
        {
            Check_RenderedImageMatchesReferenceWithTolerance(LoMaxAverageError);
        }

        private void Then_RenderedImageMatchesReferenceWithMediumTolerance()
        {
            Check_RenderedImageMatchesReferenceWithTolerance(MidMaxAverageError);
        }

        private void Then_RenderedImageMatchesReferenceWithHighTolerance()
        {
            Check_RenderedImageMatchesReferenceWithTolerance(HiMaxAverageError);
        }

        #endregion

        #region  Helper Methods

        private void Set_Scene(Func<bool, Scene> testScene)
        {
            // true to choose the lowPixel version
            _scene = testScene(true);
        }

        private void Check_RenderedImageMatchesReferenceWithTolerance(double maxAverageError)
        {
            var actualBytes = GetRgbValuesFrom(_actualImagePath);
            var referenceBytes = GetRgbValuesFrom(_referenceImagePath);

            Assert.AreEqual(actualBytes.Length, referenceBytes.Length);

            var sumOfDifferences = actualBytes
                .Zip(referenceBytes, (actualByte, referenceByte) => Math.Abs(actualByte - referenceByte))
                .Sum();

            var averageError = sumOfDifferences / (double)actualBytes.Length;
            Assert.IsTrue(averageError <= maxAverageError);
        }

        private static byte[] GetRgbValuesFrom(string imagePath)
        {
            using (var bmp = new Bitmap(imagePath))
            {
                var data = bmp.LockBits(
                    new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                    ImageLockMode.ReadOnly,
                    bmp.PixelFormat);

                var numBytes = Math.Abs(data.Stride) * data.Height;
                var rgbValues = new byte[numBytes];

                Marshal.Copy(data.Scan0, rgbValues, 0, numBytes);

                return rgbValues;
            }
        }

        #endregion
    }
}