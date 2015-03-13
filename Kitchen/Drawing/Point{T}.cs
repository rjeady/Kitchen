using System;

namespace Kitchen.Drawing
{
	/// <summary>
	/// Represents a pair of Cartesian coordinates, defining a point in a two-dimensional plane.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public struct Point<T> where T : struct, IComparable<T>, IEquatable<T>
	{
		/// <summary>
		/// Gets the x-coordinate.
		/// </summary>
		/// <value>
		/// The x-coordinate.
		/// </value>
		public T X { get; private set; }

		/// <summary>
		/// Gets the y-coordinate.
		/// </summary>
		/// <value>
		/// The y-coordinate.
		/// </value>
		public T Y { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Point{T}"/> struct.
		/// </summary>
		/// <param name="x">The x-coordinate.</param>
		/// <param name="y">The y-coordinate.</param>
		public Point(T x, T y) : this()
		{
			X = x;
			Y = y;
		}

		public static bool operator ==(Point<T> left, Point<T> right)
		{
			return left.X.Equals(right.X) && left.Y.Equals(right.Y);
		}

		public static bool operator !=(Point<T> left, Point<T> right)
		{
			return !(left == right);
		}

		public override bool Equals(object obj)
		{
			return obj is Point<T> && this == (Point<T>)obj;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (X.GetHashCode() * 397) ^ Y.GetHashCode();
			}
		}

		public override string ToString()
		{
			return string.Format("{{X={0}, Y={1}}}", X, Y);
		}
	}
}