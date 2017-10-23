using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RaytraceAirTest
{
    [TestClass]
    public class Vector3Test
    {
        private readonly Random _random = new Random();
        private const double EqualityTolerance = 1e-12;
        private List<Vector3> _vectors;
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
            var a = Vector3.One;
            var b = Vector3.One;
            var dot = Vector3.Dot(a, b);
            Assert.AreEqual(dot, 3.0);
        }

        [TestMethod]
        public void AddTest()
        {
            var a = new Vector3(1, 1, 1);
            var b = new Vector3(-1, 2, 0.5f);
            var c = a + b;
            Assert.AreEqual(new Vector3(0, 3, 1.5f), c);
        }

        #region Given methods

        private void Given_RandomVectors()
        {
            _vectors = Enumerable.Range(0, 100)
                .Select(i => new Vector3(_random.NextDouble(), _random.NextDouble(), _random.NextDouble()))
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