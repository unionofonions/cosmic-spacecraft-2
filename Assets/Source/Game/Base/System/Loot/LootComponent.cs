
namespace Parlor.Game
{
	using UnityEngine;

	[DisallowMultipleComponent]
	[RequireComponent(typeof(SpriteRenderer), typeof(Animator), typeof(Collider2D))]
	public sealed class LootComponent : MonoBehaviour, IBoundaryItem
	{
		static private readonly int s_HealthHash;
		static private readonly int s_ShieldHash;

		private LootType m_LootType;
		private float m_Amount;
		private Animator m_Animator;

		static LootComponent()
		{
			s_HealthHash = Animator.StringToHash("health");
			s_ShieldHash = Animator.StringToHash("shield");
		}

		private void Awake()
		{
			m_Animator = GetComponent<Animator>();
		}
		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.TryGetComponent<Player>(out var player))
			{
				switch (m_LootType)
				{
					case LootType.Health:
						var healed = player.Heal(m_Amount);
						if (!healed) return;
						break;
					case LootType.Shield:
						var shielded = player.ShieldUp(m_Amount);
						if (!shielded) return;
						break;
				}
				gameObject.SetActive(false);
			}
		}
		public void DropHealth(Vector3 position, float amount)
		{
			if (amount <= 0f) return;
			transform.position = position;
			gameObject.SetActive(true);
			m_Animator.Play(s_HealthHash);
			m_LootType = LootType.Health;
			m_Amount = amount;
		}
		public void DropShield(Vector3 position, float amount)
		{
			if (amount <= 0f) return;
			transform.position = position;
			gameObject.SetActive(true);
			m_Animator.Play(s_ShieldHash);
			m_LootType = LootType.Shield;
			m_Amount = amount;
		}

		private enum LootType
		{
			Health,
			Shield,
		}
	}
}