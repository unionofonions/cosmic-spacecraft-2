#nullable enable

namespace Parlor
{
	using System;

	[Serializable]
	public struct RangeInt : IEquatable<RangeInt>, IFormattable
	{
		public RangeInt(int start, int end)
		{
			Start = start;
			End = end;
		}

		public int Start;
		public int End;

		public readonly int Delta
		{
			get => End - Start;
		}
		public readonly int Length
		{
			get => Math.Abs(Delta);
		}
		public readonly int Random
		{
			get => Parlor.Random.Int32(Start, End);
		}

		public readonly int Limit(int value)
		{
			return value <= Start ? Start : value >= End ? End : value;
		}
		public readonly bool Limits(int value)
		{
			return value >= Start && value <= End;
		}
		public readonly bool Equals(RangeInt other)
		{
			return Start == other.Start && End == other.End;
		}
		public readonly string ToString(string? format, IFormatProvider? formatProvider)
		{
			return String.Format(
				"[{0}..{1}]",
				Start.ToString(format, formatProvider),
				End.ToString(format, formatProvider));
		}
		public readonly override int GetHashCode()
		{
			return HashCode.Combine(Start, End);
		}
		public readonly override bool Equals(object? obj)
		{
			return obj is RangeInt other && Equals(other);
		}
		public readonly override string ToString()
		{
			return ToString(format: null, formatProvider: null);
		}
	}
}