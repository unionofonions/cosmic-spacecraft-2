
namespace Parlor.Game
{
	using System;
	using UnityEngine;

	[RequireComponent(typeof(Rigidbody2D))]
	public sealed class Player : Ship
	{
		public const int MaxGunCount = 4;

		[Header("Player")]
		[SerializeField, NotDefault]
		private PlayerInfo m_PlayerInfo;
		[SerializeField, Limited(1, MaxGunCount)]
		private int m_GunCount;
		[SerializeField, NotDefault]
		private Animator m_Engine;
		[SerializeField]
		private GunCollection m_Guns;
		private int m_IdleHash;
		private int m_MoveHash;
		private bool m_Moving;
		private Rigidbody2D m_Rigidbody;

		public int GunCount
		{
			get => m_GunCount;
			set
			{
				SetProperty(ref m_GunCount, Mathf.Clamp(value, 1, MaxGunCount));
			}
		}
		protected override Gun[] Guns
		{
			get
			{
				return m_GunCount switch
				{
					<= 1 => m_Guns.One,
					2 => m_Guns.Two,
					3 => m_Guns.Three,
					_ => m_Guns.Four,
				};
			}
		}

		private new void Awake()
		{
			base.Awake();
			m_Rigidbody = GetComponent<Rigidbody2D>();
			m_IdleHash = Animator.StringToHash("idle");
			m_MoveHash = Animator.StringToHash("move");
		}
		private void Update()
		{
			var input = Domain.GetInputSystem();
			if (input.Hold("fir")) UpdateGuns();
			var dir = new Vector2(input.Axis("hor"), input.Axis("ver"));
			m_Rigidbody.velocity = dir.normalized * MoveSpeed;
			UpdateEngine(moving: dir.x != 0f || dir.y != 0f);
		}
		public override void Respawn()
		{
			base.Respawn();
			if (m_PlayerInfo != null)
			{
				Damage = m_PlayerInfo.Damage;
				DamageDeviation = m_PlayerInfo.DamageDeviation;
				Health = Quantity.Full(m_PlayerInfo.Health);
				Shield = m_PlayerInfo.Shield;
				MoveSpeed = m_PlayerInfo.MoveSpeed;
				FireInterval = m_PlayerInfo.FireInterval;
				BulletSpeed = m_PlayerInfo.BulletSpeed;
				BulletScheme = m_PlayerInfo.BulletScheme;
				GunCount = m_PlayerInfo.GunCount;
				transform.position = m_PlayerInfo.Position;
			}
		}
		private void UpdateEngine(bool moving)
		{
			if (m_Moving != moving && m_Engine != null)
			{
				m_Moving = moving;
				m_Engine.Play(moving ? m_MoveHash : m_IdleHash);
			}
		}

		[Serializable]
		private sealed class GunCollection
		{
			[NotDefault]
			public Gun[] One;
			[NotDefault]
			public Gun[] Two;
			[NotDefault]
			public Gun[] Three;
			[NotDefault]
			public Gun[] Four;
		}
	}
}