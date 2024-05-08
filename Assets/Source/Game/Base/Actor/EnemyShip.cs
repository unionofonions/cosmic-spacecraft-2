
namespace Parlor.Game
{
	using UnityEngine;

	[DisallowMultipleComponent]
	public class EnemyShip : Ship
	{
		[Header("EnemyShip")]
		[SerializeField]
		private float m_GunRotationSpeed;
		[SerializeField, NotDefault]
		private Gun[] m_Guns;
		[SerializeField]
		private Route m_Route;

		protected override Gun[] Guns
		{
			get => m_Guns;
		}

		protected void FixedUpdate()
		{
			transform.position = m_Route.MoveTowardsDestination(
				transform.position,
				MoveSpeed * Time.deltaTime);
			UpdateGuns();
			SpinGuns();
		}
		public override void ResetProperties()
		{
			base.ResetProperties();
			m_Route.Reset();
		}
		private void SpinGuns()
		{
			if (m_GunRotationSpeed == 0f) return;
			foreach (var elem in m_Guns)
			{
				if (elem == null) continue;
				elem.transform.localEulerAngles += m_GunRotationSpeed * Time.fixedDeltaTime * Vector3.forward;
			}
		}
	}
}