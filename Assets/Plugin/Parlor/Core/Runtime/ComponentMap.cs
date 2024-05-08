#nullable enable

namespace Parlor.Runtime
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;
	using Parlor.Diagnostics;
	using System.Collections;

	public class ComponentMap<T> : IReadOnlyCollection<ComponentPool<T>> where T : Component
	{
		public ComponentMap(Func<T, ComponentPool<T>> create)
		{
			if (create == null) throw new ArgumentNullException(nameof(create));
			m_Collection = new();
			m_Create = create;
			m_Container = UnityHelper.CreateGameObject(
				name: String.Format(c_ContainerName, typeof(T).PartialName()),
				permanent: true);
			m_Cache = null;
		}

		public int Count
		{
			get => m_Collection.Count;
		}

		public T Provide(T scheme)
		{
			if (scheme == null) throw new ArgumentNullException(nameof(scheme));
			if (m_Cache == null || Compare(m_Cache, scheme) != 0)
			{
				var index = IndexOf(scheme);
				if (index >= 0)
				{
					m_Cache = m_Collection[index];
				}
				else
				{
					m_Cache = Create(scheme);
					m_Collection.Insert(~index, m_Cache);
				}
			}
			return m_Cache.Provide();
		}
		public IEnumerator<ComponentPool<T>> GetEnumerator()
		{
			return m_Collection.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#region Internal
		private const string c_ContainerName = "[map-{0}]";
		private readonly List<ComponentPool<T>> m_Collection;
		private readonly Func<T, ComponentPool<T>> m_Create;
		private readonly GameObject m_Container;
		private ComponentPool<T>? m_Cache;

		private int Compare(ComponentPool<T> pool, T scheme)
		{
			return pool.Scheme.GetInstanceID().CompareTo(scheme.GetInstanceID());
		}
		private int IndexOf(T scheme)
		{
			var low = 0;
			var high = m_Collection.Count - 1;
			while (low <= high)
			{
				var mid = low + ((high - low) >> 1);
				var comp = Compare(m_Collection[mid], scheme);
				if (comp < 0) low = mid + 1;
				else if (comp > 0) high = mid - 1;
				else return mid;
			}
			return ~low;
		}
		private ComponentPool<T> Create(T scheme)
		{
			var ret = m_Create(scheme);
			Contract.Assume(ret != null);
			ret.Container.transform.SetParent(m_Container.transform, worldPositionStays: false);
			return ret;
		}
		#endregion // Internal
	}
}