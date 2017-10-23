using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaytraceAir;

namespace RaytraceAirTest
{
    [TestClass]
    public class Vec3Test
    {
        private readonly Random _random = new Random();
        private const double EqualityTolerance = 1e-12;
        private List<Vec3> _vectors;
        private List<double> _results;

        [TestMethod]
        public void NormTest()
        {
            Given_RandomVectors();
            When_Normalize();
            Then_AllHaveUnitLength();
        }

        [TestMethod]
        public void DotTest()
        {
            var a = Vec3.Ones;
            var b = Vec3.Ones;
            var dot = a.Dot(b);
            Assert.AreEqual(dot, 3.0);
        }

        [TestMethod]
        public void AddTest()
        {
            var a = new Vec3(1, 1, 1);
            var b = new Vec3(-1, 2, 0.5);
            var c = a + b;
            Assert.AreEqual(new Vec3(0, 3, 1.5), c);
        }

        #region Given methods

        private void Given_RandomVectors()
        {
            _vectors = Enumerable.Range(0, 100)
                .Select(i => new Vec3(_random.NextDouble(), _random.NextDouble(), _random.NextDouble()))
                .ToList();
        }

        #endregion

        #region When methods

        private void When_Normalize()
        {
            _vectors = _vectors
                .Select(v => v.Normalized())
                .ToList();
        }

        #endregion

        #region Then methods

        private void Then_AllHaveUnitLength()
        {
            foreach (var v in _vectors)
            {
                Assert.AreEqual(1.0, v.Norm, EqualityTolerance);
            }
        }

        #endregion
    }
}