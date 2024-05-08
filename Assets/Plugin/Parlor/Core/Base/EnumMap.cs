#nullable enable

namespace Parlor
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using Parlor.Diagnostics;

	[Serializable, EditorHandled]
	public class EnumMap<TEnum, TValue> : IReadOnlyCollection<TValue> where TEnum : Enum
	{
		public TValue? this[int index]
		{
			get
			{
				return (uint)index >= (uint)m_Values.Length
					? m_Values.Length != 0 ? m_Values[0] : default
					: m_Values[index];
			}
		}
		public TValue? this[TEnum key]
		{
			get
			{
				var index = Convert.ToInt32(key);
				return this[index];
			}
		}

		public int Count
		{
			get => m_Values.Length;
		}

		public IEnumerator<TValue> GetEnumerator()
		{
			return ((IEnumerable<TValue>)m_Values).GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#region Internal
#nullable disable
		[SerializeField]
		private TValue[] m_Values;
#nullable disable
		#endregion // Internal
	}
}