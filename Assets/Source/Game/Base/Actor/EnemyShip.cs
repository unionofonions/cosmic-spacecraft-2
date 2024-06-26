
namespace Parlor.Game
{
	using System;
	using UnityEngine;

	public class EnemyShip : Ship
	{
		[Header("EnemyShip")]
		[SerializeField]
		private EnemyShipType m_ShipType;
		[SerializeField]
		private EnemyShipFlags m_Flags;
		[SerializeField]
		private float m_RotationSpeed;
		[SerializeField]
		private bool m_DynamicSpin;
		[SerializeField]
		private Route m_Route;
		[SerializeField, NotDefault]
		private Gun[] m_Guns;
		private Collider2D m_Collider;

		protected override Gun[] Guns
		{
			get => m_Guns;
		}
		public EnemyShipFlags Flags
		{
			get => m_Flags;
		}

		protected new void Awake()
		{
			base.Awake();
			m_Collider = GetComponent<Collider2D>();
		}
		protected void FixedUpdate()
		{
			m_Route.Update();
			switch (m_Route.Status)
			{
				case RouteStatus.MovingTowardsFirstDestination:
					transform.position = m_Route.CurrentPosition;
					break;
				case RouteStatus.JustArrivedAtFirstDestination:
					transform.position = m_Route.CurrentPosition;
					m_Collider.enabled = true;
					break;
				case RouteStatus.Moving:
					transform.position = m_Route.CurrentPosition;
					AfterReachingFirstDestination();
					if (m_DynamicSpin && m_Route.JustChangedDestination && m_ShipType is EnemyShipType.Boss)
					{
						m_RotationSpeed  = -m_RotationSpeed;
					}
					break;
				case RouteStatus.Finished:
					AfterReachingFirstDestination();
					break;
				case RouteStatus.Invalid:
				default:
					return;
			}
		}
		public override void Respawn()
		{
			base.Respawn();
			Shield = Quantity.Full(Shield.Max);
			if (m_ShipType is EnemyShipType.Normal)
			{
				m_Route = Domain.GetBoundarySystem().GetRoute();
			}
			m_Route.Restart();
			m_Route.Speed = MoveSpeed;
			transform.SetPositionAndRotation(m_Route.StartPosition, m_Route.StartRotation);
			m_Collider.enabled = false;
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
		private void Spin()
		{
			foreach (var elem in m_Guns)
			{
				if (elem != null)
				{
					elem.transform.eulerAngles += Vector3.forward * m_RotationSpeed;
				}
			}
		}
		private void AfterReachingFirstDestination()
		{
			UpdateGuns();
			if (m_ShipType is EnemyShipType.Boss)
			{
				Spin();
			}
			else
			{
				LookAtPlayer();
			}
		}
	}
	public enum EnemyShipType
	{
		Normal,
		Elite,
		Boss
	}
	[Flags]
	public enum EnemyShipFlags
	{
		None = 0x0,
		PlayerUpgradeOnDeath = 0x1
	}
}