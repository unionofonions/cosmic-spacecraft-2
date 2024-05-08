
namespace Parlor.Game
{
	using UnityEngine;

	[DisallowMultipleComponent]
	[RequireComponent(typeof(SpriteRenderer), typeof(Animator), typeof(Collider2D))]
	public sealed class LootComponent : MonoBehaviour
	{
		static private readonly int s_HealthHash;
		static private readonly int s_ShieldHash;

		private LootType m_LootType;
		private float m_Amount;
		private float m_Lifetime;
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
		private void Update()
		{
			m_Lifetime -= Time.deltaTime;
			if (m_Lifetime <= 0f)
			{
				gameObject.SetActive(false);
			}
		}
		public void DropHealth(Vector3 position, float amount, float lifetime)
		{
			if (amount <= 0f || lifetime <= 0f) return;
			transform.position = position;
			gameObject.SetActive(true);
			m_Animator.Play(s_HealthHash);
			m_LootType = LootType.Health;
			m_Amount = amount;
			m_Lifetime = lifetime;
		}
		public void DropShield(Vector3 position, float amount, float lifetime)
		{
			if (amount <= 0f || lifetime <= 0f) return;
			transform.position = position;
			gameObject.SetActive(true);
			m_Animator.Play(s_ShieldHash);
			m_LootType = LootType.Shield;
			m_Amount = amount;
			m_Lifetime = lifetime;
		}

		private enum LootType
		{
			Health,
			Shield,
		}
	}
}