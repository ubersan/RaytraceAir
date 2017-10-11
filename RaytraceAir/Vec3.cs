using System;

namespace RaytraceAir
{
    public class Vec3
    {
        private const double EqualityTolerance = 1e-12;

        public Vec3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;

            Norm = Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public static Vec3 Ones()
        {
            return new Vec3(1, 1, 1);
        }

        public static Vec3 Zeros()
        {
            return new Vec3(0, 0, 0);
        }

        public Vec3 Normalized()
        {
            return new Vec3(X / Norm, Y / Norm, Z / Norm);
        }

        public double Dot(Vec3 v)
        {
            return X * v.X + Y * v.Y + Z * v.Z;
        }

        public static Vec3 operator +(Vec3 a, Vec3 b)
        {
            return new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vec3 operator -(Vec3 a, Vec3 b)
        {
            return new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vec3 operator *(Vec3 a, double c)
        {
            return new Vec3(a.X * c, a.Y * c, a.Z * c);
        }

        public static Vec3 operator *(double c, Vec3 a)
        {
            return a * c;
        }

        public override bool Equals(object obj)
        {
            if (obj is Vec3 v)
            {
                return Equals(v);
            }

            return false;
        }

        private bool Equals(Vec3 other)
        {
            return Math.Abs(X - other.X) < EqualityTolerance
                && Math.Abs(Y - other.Y) < EqualityTolerance
                && Math.Abs(Z - other.Z) < EqualityTolerance;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }

        public double X { get; }
        public double Y { get; }
        public double Z { get; }
        public double Norm { get; }
    }
}