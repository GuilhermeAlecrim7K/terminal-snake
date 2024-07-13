using System.Diagnostics.CodeAnalysis;

namespace TerminalSnake.GameObjects
{
    internal struct Point(int x, int y) : IEquatable<Point>
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;

        public override readonly bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is Point point && Equals(point);
        }
        public readonly bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static bool operator ==(Point left, Point right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Point left, Point right)
        {
            return !(left == right);
        }
    }
}