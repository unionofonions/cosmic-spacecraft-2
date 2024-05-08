#nullable enable

namespace Parlor.Runtime
{
	using System;
	using UnityEngine;

	public class ComponentPool<T> : Pool<T> where T : Component
	{
		public ComponentPool(T scheme, Func<T, bool> active)
		: base(active)
		{
			if (scheme == null) throw new ArgumentNullException(nameof(scheme));
			m_Scheme = scheme;
			m_Container = UnityHelper.CreateGameObject(
				name: String.Format(c_ContainerName, m_Scheme.name),
				permanent: true);
		}

		public T Scheme
		{
			get => m_Scheme;
		}
		public GameObject Container
		{
			get => m_Container;
		}

		protected override T Create()
		{
			var ret = UnityHelper.InstantiateScheme(m_Scheme, name: Count.ToString());
			ret.transform.SetParent(m_Container.transform, worldPositionStays: false);
			return ret;
		}

		#region Internal
		private const string c_ContainerName = "[pool-{0}]";
		private readonly T m_Scheme;
		private readonly GameObject m_Container;
		#endregion // Internal
	}
}