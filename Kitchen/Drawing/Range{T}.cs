using System;

namespace Kitchen.Drawing
{
	/// <summary>
	/// Represents an ordered pair of numeric values, defining the start and end of a continuous range in one dimension.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public struct Range<T> : IEquatable<Range<T>> where T : struct, IComparable<T>, IEquatable<T>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Range{T}"/> struct.
		/// </summary>
		/// <param name="start">The range start value.</param>
		/// <param name="end">The range end value.</param>
		public Range(T start, T end) : this()
		{
			Start = start;
			End = end;
		}

		/// <summary>
		/// Gets the range start value.
		/// </summary>
		/// <value>
		/// The range start value.
		/// </value>
		public T Start { get; private set; }

		/// <summary>
		/// Gets the range end value.
		/// </summary>
		/// <value>
		/// The range end value.
		/// </value>
		public T End { get; private set; }

		public static bool operator ==(Range<T> left, Range<T> right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Range<T> left, Range<T> right)
		{
			return !left.Equals(right);
		}

		public bool Equals(Range<T> other)
		{
			return Start.Equals(other.Start) && End.Equals(other.End);
		}

		public override bool Equals(object obj)
		{
			return obj is Range<T> && Equals((Range<T>)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Start.GetHashCode() * 397) ^ End.GetHashCode();
			}
		}
	}
}