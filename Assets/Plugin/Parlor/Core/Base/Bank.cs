#nullable enable

namespace Parlor
{
	using System;
	using UnityEngine;
	using Parlor.Diagnostics;

	[Serializable, EditorHandled]
	public class Bank<T>
	{
		public int Count
		{
			get => m_Collection.Length;
		}
		public BankMode Mode
		{
			get => m_Mode;
		}

		public T? Provide()
		{
			return m_Collection.Length switch
			{
				0 => default,
				1 => m_Collection[0].Value,
				_ => m_Mode switch
				{
					BankMode.FullyRandom => FullyRandom(),
					BankMode.NonRepRandom => NonRepRandom(),
					BankMode.FullyWeighted => FullyWeighted(),
					_ => FullyRandom()
				}
			};
		}

		#region Internal
#nullable disable
		[SerializeField]
		private Entry[] m_Collection;
		[SerializeField]
		private BankMode m_Mode;
		[SerializeField, HideInInspector, ReadOnly]
		private int m_Threshold;
		[SerializeField, HideInInspector, ReadOnly]
		private float m_TotalWeight;
		[NonSerialized]
		private int m_Index;

		private T FullyRandom()
		{
			var index = Random.Int32(0, m_Collection.Length);
			return m_Collection[index].Value;
		}
		private T NonRepRandom()
		{
			var index = Random.Int32(m_Threshold, m_Collection.Length);
			var ret = m_Collection[index];
			m_Collection[index] = m_Collection[m_Index];
			m_Collection[m_Index] = ret;
			if (++m_Index >= m_Threshold) m_Index = 0;
			return ret.Value;
		}
		private T FullyWeighted()
		{
			var roll = Random.Single(MathHelper.Epsilon, m_TotalWeight);
			var acc = 0f;
			foreach (var elem in m_Collection)
			{
				acc += elem.Weight;
				if (roll <= acc) return elem.Value;
			}
			return m_TotalWeight <= 0f ? default : m_Collection[^1].Value;
		}
#nullable enable
		#endregion

		[Serializable, EditorHandled]
		internal class Entry
		{
			public T? Value
			{
				get => m_Value;
			}
			public float Weight
			{
				get => m_Weight;
			}

			#region Internal
#nullable disable
			[SerializeField]
			private T m_Value;
			[SerializeField, Unsigned]
			private float m_Weight;
			[SerializeField, HideInInspector]
			private bool m_Weighted;
			[SerializeField, ReadOnly, Limited(0f, 100f)]
			private float m_Chance;
#nullable enable
			#endregion // Internal
		}
	}
	public enum BankMode
	{
		FullyRandom,
		NonRepRandom,
		FullyWeighted
	}
}