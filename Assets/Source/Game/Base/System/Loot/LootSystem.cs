using UnityEngine;

namespace Parlor.Game
{
	using Parlor.Runtime;

	[DisallowMultipleComponent]
	public sealed class LootSystem : MonoBehaviour
	{
		[SerializeField, NotDefault]
		private LootComponent m_ComponentScheme;
		[SerializeField, NotDefault]
		private LootInfo m_LootInfo;
		private ComponentPool<LootComponent> m_Pool;

		private void Awake()
		{
			if (m_ComponentScheme == null) return;
			m_Pool = new(m_ComponentScheme, active: elem => elem.gameObject.activeSelf);
			Actor.OnDeath += (Actor instigator, Actor victim) =>
			{
				if (instigator is Player)
				{
					if (Random.Boolean(m_LootInfo.HealDropChance))
					{
						DropHealth(victim.transform.position, m_LootInfo.HealAmount.Random);
					}
					else if (Random.Boolean(m_LootInfo.ShieldDropChance))
					{
						DropShield(victim.transform.position, m_LootInfo.ShieldAmount.Random);
					}
				}
			};
			Domain.GetSpawnSystem().OnBeginSpawn += ReturnAll;
		}
		public void DropHealth(Vector3 position, float amount)
		{
			if (m_Pool == null) return;
			var comp = m_Pool.Provide();
			comp.DropHealth(position, amount);
		}
		public void DropShield(Vector3 position, float amount)
		{
			if (m_Pool == null) return;
			var comp = m_Pool.Provide();
			comp.DropShield(position, amount);
		}
		public void ReturnAll()
		{
			foreach (var elem in m_Pool)
			{
				elem.gameObject.SetActive(false);
			}
		}
	}
}