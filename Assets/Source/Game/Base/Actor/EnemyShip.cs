
namespace Parlor.Game
{
	using UnityEngine;

	[DisallowMultipleComponent]
	public class EnemyShip : Ship
	{
		[Header("EnemyShip")]
		[SerializeField, NotDefault]
		private Gun[] m_Guns;
		[SerializeField, MinEpsilon]
		private float m_RotationSpeed;
		[SerializeField, MinEpsilon]
		private float m_Range;
		private Vector3 m_Direction;
		private bool m_IsAtDestination;
		private Collider2D m_Collider;

		protected override Gun[] Guns
		{
			get => m_Guns;
		}

		protected new void Awake()
		{
			base.Awake();
			m_Collider = GetComponent<Collider2D>();
		}
		protected void FixedUpdate()
		{
			if (m_IsAtDestination)
			{
				LookAtPlayer();
				UpdateGuns();
			}
			else
			{
				MoveTowardsDestination();
			}
		}
		public override void Respawn()
		{
			base.Respawn();
			m_IsAtDestination = false;
			transform.position = Domain.GetBoundarySystem().GetSpawnPoint();
			m_Direction = (Domain.GetBoundarySystem().Center - transform.position).normalized;
			LookAtCenter();
			m_Collider.enabled = false;
		}
		private void MoveTowardsDestination()
		{
			if (Vector2.Distance(Domain.GetBoundarySystem().Center, transform.position) <= m_Range)
			{
				m_Collider.enabled = true;
				m_IsAtDestination = true;
			}
			else
			{
				transform.position += MoveSpeed * Time.fixedDeltaTime * m_Direction;
			}
		}
		private void LookAtCenter()
		{
			transform.rotation = Quaternion.LookRotation(
				Vector3.forward,
				Domain.GetBoundarySystem().Center - transform.position);
		}
		private void LookAtPlayer()
		{
			var dir = Domain.GetPlayer().transform.position - transform.position;
			var angle = MathHelper.Dir2Deg(dir) - 90f;
			transform.rotation = Quaternion.RotateTowards(
				transform.rotation,
				Quaternion.Euler(0f, 0f, angle),
				m_RotationSpeed * Time.fixedDeltaTime);
		}
	}
}