
namespace Parlor.Game
{
	using UnityEngine;
	using Parlor.Runtime;

	[DisallowMultipleComponent]
	public sealed class LootSystem : MonoBehaviour
	{
		[SerializeField, NotDefault]
		private LootComponent m_ComponentScheme;
		[SerializeField, MinEpsilon]
		private float m_ComponentLifetime;
		[SerializeField, NotDefault]
		private LootInfo m_LootInfo;
		private ComponentPool<LootComponent> m_Pool;

		private void Awake()
		{
			if (m_ComponentScheme == null) return;
			m_Pool = new(m_ComponentScheme, active: elem => elem.gameObject.activeSelf);
		}
		public void DropHealth(Vector3 position, float amount)
		{
			if (m_Pool == null) return;
			var comp = m_Pool.Provide();
			comp.DropHealth(position, amount, m_ComponentLifetime);
		}
		public void DropShield(Vector3 position, float amount)
		{
			if (m_Pool == null) return;
			var comp = m_Pool.Provide();
			comp.DropShield(position, amount, m_ComponentLifetime);
		}
	}
}