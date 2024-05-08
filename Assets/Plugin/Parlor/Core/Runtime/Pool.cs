#nullable enable

namespace Parlor.Runtime
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using Parlor.Diagnostics;

	public abstract class Pool<T> : IReadOnlyCollection<T>
	{
		public Pool(Func<T, bool> active)
		{
			if (active == null) throw new ArgumentNullException(nameof(active));
			m_Collection = new();
			m_Active = active;
		}

		public int Count
		{
			get => m_Collection.Count;
		}

		public T Provide()
		{
			int index;
			for (var i = m_Collection.Count; --i >= 0;)
			{
				if (!m_Active(m_Collection[i]))
				{
					index = i;
					goto SWAP;
				}
			}
			Accrete();
			index = m_Collection.Count - 1;
		SWAP:
			var ret = m_Collection[index];
			if (index != 0)
			{
				m_Collection[index] = m_Collection[0];
				m_Collection[0] = ret;
			}
			return ret;
		}
		protected abstract T Create();
		public IEnumerator<T> GetEnumerator()
		{
			return m_Collection.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#region Internal
		private readonly List<T> m_Collection;
		private readonly Func<T, bool> m_Active;

		private void Accrete()
		{
			var reg = Create();
			Contract.Assume(!Parlor.UnityHelper.IsNull(reg));
			m_Collection.Add(reg);
		}
		#endregion // Internal
	}
}