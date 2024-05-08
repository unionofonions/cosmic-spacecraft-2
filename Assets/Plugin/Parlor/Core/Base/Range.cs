#nullable enable

namespace Parlor
{
	using System;

	[Serializable]
	public struct Range : IEquatable<Range>, IFormattable
	{
		public Range(float start, float end)
		{
			Start = start;
			End = end;
		}

		public float Start;
		public float End;

		public readonly float Delta
		{
			get => End - Start;
		}
		public readonly float Length
		{
			get => Math.Abs(Delta);
		}
		public readonly float Random
		{
			get => Parlor.Random.Single(Start, End);
		}

		public readonly float Limit(float value)
		{
			return value <= Start ? Start : value >= End ? End : value;
		}
		public readonly bool Limits(float value)
		{
			return value >= Start && value <= End;
		}
		public readonly bool Equals(Range other)
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
			return obj is Range other && Equals(other);
		}
		public readonly override string ToString()
		{
			return ToString(format: null, formatProvider: null);
		}
	}
}