#nullable enable

namespace Parlor.Game
{
	using System;
	using UnityEngine;
	using Parlor.Diagnostics;

	[Serializable, EditorHandled]
	public struct Quantity : IEquatable<Quantity>, IFormattable
	{
		[SerializeField]
		private float m_Current;
		[SerializeField]
		private float m_Max;

		public Quantity(float current, float max)
		{
			m_Max = Mathf.Max(max, 0f);
			m_Current = Mathf.Clamp(current, 0f, m_Max);
		}

		public readonly float Current
		{
			get => m_Current;
		}
		public readonly float Max
		{
			get => m_Max;
		}
		public readonly float Ratio
		{
			get
			{
				return m_Max > 0f ? m_Current / m_Max : 0f;
			}
		}
		public readonly bool IsFull
		{
			get
			{
				return m_Current >= m_Max;
			}
		}

		static public Quantity Full(float value)
		{
			return new(current: value, max: value);
		}

		public readonly bool Equals(Quantity other)
		{
			return m_Current == other.m_Current && m_Max == other.m_Max;
		}
		public readonly string ToString(string? format, IFormatProvider? formatProvider)
		{
			return String.Format(
				"Current: {0}, Max: {1}",
				m_Current.ToString(format, formatProvider),
				m_Max.ToString(format, formatProvider));
		}
		public readonly override int GetHashCode()
		{
			return HashCode.Combine(m_Current, m_Max);
		}
		public readonly override bool Equals(object? obj)
		{
			return obj is Quantity other && Equals(other);
		}
		public readonly override string ToString()
		{
			return ToString(format: null, formatProvider: null);
		}
	}
}