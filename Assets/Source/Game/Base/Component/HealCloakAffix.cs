
namespace Parlor.Game
{
	using UnityEngine;

	public class HealCloakAffix : MonoBehaviour
	{
		private const int c_BufferSize = 8;

		[SerializeField, MinEpsilon]
		private float m_Amount;
		[SerializeField, MinEpsilon]
		private float m_Radius;
		[SerializeField, MinEpsilon]
		private float m_ProcInterval;
		[SerializeField]
		private LayerMask m_TargetLayer;
		private float m_ProcTimer;
		private Collider2D[] m_Buffer;

		protected void Awake()
		{
			m_Buffer = new Collider2D[c_BufferSize];
		}
		protected void Update()
		{
			m_ProcTimer += Time.deltaTime;
			if (m_ProcTimer >= m_ProcInterval)
			{
				m_ProcTimer = 0f;
				Proc();
			}
		}
		private void Proc()
		{
			var count = Physics2D.OverlapCircleNonAlloc(transform.position, m_Radius, m_Buffer, m_TargetLayer);
			for (var i = 0; i < count; ++i)
			{
				if (m_Buffer[i].TryGetComponent<EnemyShip>(out var target))
				{
					target.Heal(m_Amount);
				}
			}
		}
	}
}