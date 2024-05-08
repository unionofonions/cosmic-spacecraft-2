
namespace Parlor.Game
{
	using UnityEngine;

	public abstract class Ship : Actor
	{
		public const float MinFireInterval = 0.04f;
		public const float MinBulletSpeed = 3f;

		[Header("Ship")]
		[SerializeField, Limited(MinFireInterval)]
		private float m_FireInterval;
		[SerializeField, Limited(MinBulletSpeed)]
		private float m_BulletSpeed;
		[SerializeField, NotDefault]
		private Bullet m_BulletScheme;
		[SerializeField]
		private SfxReference m_FireSfx;

		public float FireInterval
		{
			get => m_FireInterval;
			set
			{
				SetProperty(ref m_FireInterval, Mathf.Max(value, MinFireInterval));
			}
		}
		public float BulletSpeed
		{
			get => m_BulletSpeed;
			set
			{
				SetProperty(ref m_BulletSpeed, Mathf.Max(value, MinBulletSpeed));
			}
		}
		public Bullet BulletScheme
		{
			get => m_BulletScheme;
			set
			{
				if (value != null)
				{
					SetProperty(ref m_BulletScheme, value);
				}
			}
		}
		protected abstract Gun[] Guns { get; }

		protected void UpdateGuns()
		{
			if (Guns == null) return;
			var fired = false;
			foreach (var elem in Guns)
			{
				if (elem == null) continue;
				fired |= elem.UpdateGun(this);
			}
			if (fired) m_FireSfx.PlayEffect();
		}
	}
}