
namespace Parlor.Game
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "PlayerInfo", menuName = "Parlor/Game/PlayerInfo")]
	public sealed class PlayerInfo : ScriptableObject
	{
		[SerializeField, Unsigned]
		private float m_Damage;
		[SerializeField, Percentage]
		private float m_DamageDeviation;
		[SerializeField]
		private float m_CritChance;
		[SerializeField, MinOne]
		private float m_CritDamage;
		[SerializeField]
		private float m_ArmorPenetration;
		[SerializeField, MinEpsilon]
		private float m_Health;
		[SerializeField]
		private float m_Armor;
		[SerializeField]
		private Quantity m_Shield;
		[SerializeField, Unsigned]
		private float m_MoveSpeed;
		[SerializeField, Limited(Ship.MinFireInterval)]
		private float m_FireInterval;
		[SerializeField, Limited(Ship.MinBulletSpeed)]
		private float m_BulletSpeed;
		[SerializeField, NotDefault]
		private Bullet m_BulletScheme;
		[SerializeField, Limited(1, Player.MaxGunCount)]
		private int m_GunCount;
		[SerializeField]
		private Vector3 m_Position;

		public float Damage
		{
			get => m_Damage;
		}
		public float DamageDeviation
		{
			get => m_DamageDeviation;
		}
		public float CritChance
		{
			get => m_CritChance;
		}
		public float CritDamage
		{
			get => m_CritDamage;
		}
		public float ArmorPenetration
		{
			get => m_ArmorPenetration;
		}
		public float Health
		{
			get => m_Health;
		}
		public float Armor
		{
			get => m_Armor;
		}
		public Quantity Shield
		{
			get => m_Shield;
		}
		public float MoveSpeed
		{
			get => m_MoveSpeed;
		}
		public float FireInterval
		{
			get => m_FireInterval;
		}
		public float BulletSpeed
		{
			get => m_BulletSpeed;
		}
		public Bullet BulletScheme
		{
			get => m_BulletScheme;
		}
		public int GunCount
		{
			get => m_GunCount;
		}
		public Vector3 Position
		{
			get => m_Position;
		}
	}
}