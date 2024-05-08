
namespace Parlor.Game
{
	using UnityEngine;
	using Parlor.Diagnostics;

	[DisallowMultipleComponent]
	[EditorHandled]
	public class Gun : MonoBehaviour
	{
		[SerializeField, NotDefault]
		private Transform[] m_Muzzles;
		private float m_LastTimeFired;

		public Transform[] Muzzles
		{
			get => m_Muzzles;
		}

		public bool UpdateGun(Ship ship)
		{
			if (ship == null) return false;
			if (Time.time - m_LastTimeFired >= ship.FireInterval)
			{
				m_LastTimeFired = Time.time;
				Fire(ship);
				return true;
			}
			return false;
		}
		private void Fire(Ship ship)
		{
			if (ship.BulletScheme == null) return;
			foreach (var elem in m_Muzzles)
			{
				if (elem == null) continue;
				var bullet = BulletProvider.Provide(ship.BulletScheme);
				bullet.Fire(source: ship, elem.position, elem.rotation, ship.BulletSpeed);
			}
		}
	}
}