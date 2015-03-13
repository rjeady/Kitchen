using System;

namespace Kitchen.Drawing
{
	/// <summary>
	/// Represents a pair of x-coordinates and a pair of y-coordinates, defining a rectangular region by its start and end position in each dimension.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public struct RectRegion<T> where T : struct, IComparable<T>, IEquatable<T>
	{
		/// <summary>
		/// Gets the left (lesser) x-coordinate.
		/// </summary>
		/// <value>
		/// The left (lesser) x-coordinate.
		/// </value>
		public T X1 { get; private set; }

		/// <summary>
		/// Gets the right (greater) x-coordinate.
		/// </summary>
		/// <value>
		/// The right (greater) x-coordinate.
		/// </value>
		public T X2 { get; private set; }

		/// <summary>
		/// Gets the bottom (lesser) y-coordinate.
		/// </summary>
		/// <value>
		/// The bottom (lesser) y-coordinate.
		/// </value>
		public T Y1 { get; private set; }

		/// <summary>
		/// Gets the top (greater) y-coordinate.
		/// </summary>
		/// <value>
		/// The top (greater) y-coordinate.
		/// </value>
		public T Y2 { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="RectRegion{T}"/> struct.
		/// </summary>
		/// <param name="x1">The left (lesser) x-coordinate.</param>
		/// <param name="x2">The right (greater) x-coordinate.</param>
		/// <param name="y1">The bottom (lesser) y-coordinate.</param>
		/// <param name="y2">The top (greater) y-coordinate.</param>
		public RectRegion(T x1, T x2, T y1, T y2): this()
		{
			X1 = x1;
			X2 = x2;
			Y1 = y1;
			Y2 = y2;
		}

		public static bool operator ==(RectRegion<T> left, RectRegion<T> right)
		{
			return left.X1.Equals(right.X1) && left.X2.Equals(right.X2) && left.Y1.Equals(right.Y1) && left.Y2.Equals(right.Y2);
		}

		public static bool operator !=(RectRegion<T> left, RectRegion<T> right)
		{
			return !(left == right);
		}

		public override bool Equals(object obj)
		{
			return obj is RectRegion<T> && this == (RectRegion<T>)obj;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = X1.GetHashCode();
				hashCode = (hashCode * 397) ^ X2.GetHashCode();
				hashCode = (hashCode * 397) ^ Y1.GetHashCode();
				hashCode = (hashCode * 397) ^ Y2.GetHashCode();
				return hashCode;
			}
		}
	}
}