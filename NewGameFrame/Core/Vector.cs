﻿using System.Security.Cryptography.X509Certificates;
using System.Xml.Schema;

namespace GameFrame.Core
{
    public struct Vector(int x = 0, int y = 0, int z = 0)
    {
        #region 预设
        public static Vector Zero => new();
        public static Vector Up => new(0, 1, 0);
        public static Vector Down => new(0, -1, 0);
        public static Vector Left => new(-1, 0, 0);
        public static Vector Right => new(1, 0, 0);
        public static Vector Forward => new(0, 0, 1);
        public static Vector Back => new(0, 0, -1);
        #endregion

        #region 运算符
        public static Vector operator +(Vector left, Vector right)
        {
            return new Vector(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }
        public static Vector operator -(Vector vector)
        {
            return vector.Operate(c => -c);
        }
        public static Vector operator -(Vector left, Vector right)
        {
            return left + -right;
        }
        public static Vector operator *(Vector vector, int num)
        {
            return vector.Operate(c => c * num);
        }
        public static Vector operator *(int num, Vector vector)
        {
            return vector * num;
        }
        public static Vector operator /(Vector vector, int num)
        {
            return vector.Operate(c => c / num);
        }
        public static bool operator ==(Vector left, Vector right)
        {
            return left.X == right.X && left.Y == right.Y && left.Z == right.Z;
        }
        public static bool operator !=(Vector left, Vector right)
        {
            return !(left == right);
        }

        public static implicit operator Vector((int, int) tuple)
        {
            return new Vector(tuple.Item1, tuple.Item2);
        }
        public static implicit operator Vector((int, int, int) tuple)
        {
            return new Vector(tuple.Item1, tuple.Item2, tuple.Item3);
        }
        #endregion

        #region 数据
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
        public int Z { get; set; } = z;
        public readonly Vector Abs => Operate(Math.Abs);

        public readonly float Length => MathF.Sqrt(SqureLength);
        public readonly float SqureLength => X * X + Y * Y + Z * Z;
        #endregion

        #region 函数
        public readonly Vector Operate(Func<int, int> operate)
        {
            return new(operate(X), operate(Y), operate(Z));
        }
        public override readonly string ToString()
        {
            return $"({X},{Y})";
        }
        public override readonly bool Equals(object? obj)
        {
            return obj is Vector otherVector
                && otherVector.X == X
                && otherVector.Y == Y
                && otherVector.Z == Z;
        }
        public override readonly int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }
        #endregion
    }
}
