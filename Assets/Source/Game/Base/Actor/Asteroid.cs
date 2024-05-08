
namespace Parlor.Game
{
	using System;
	using UnityEngine;
	using Parlor.Diagnostics;

	[RequireComponent(typeof(Rigidbody2D))]
	[EditorHandled]
	public class Asteroid : Actor
	{
		[Header("Asteroid")]
		[SerializeField]
		private AsteroidFlags m_AsteroidFlags;
		[SerializeField, HideInInspector, NotDefault]
		private Asteroid m_SplitAsteroid;
		[SerializeField, HideInInspector, MinOne]
		private int m_SplitCount;
		[SerializeField, HideInInspector]
		private CurveReference m_InvisibilityCurve;

		protected void FixedUpdate()
		{
			if ((m_AsteroidFlags & AsteroidFlags.Stealth) != 0)
			{
				UpdateInvisibility();
			}
		}
		public override void Die(Actor instigator)
		{
			base.Die(instigator);
			if (instigator != null && (m_AsteroidFlags & AsteroidFlags.Splitting) != 0)
			{
				Split();
			}
		}
		private void UpdateInvisibility()
		{
			var distance = Vector2.Distance(transform.position, Domain.GetPlayer().transform.position);
			var alpha = m_InvisibilityCurve.Info.Value.Evaluate(distance);
			var color = Renderer.color;
			color.a = alpha;
			Renderer.color = color;
		}
		private void Split()
		{
			if (m_SplitAsteroid == null) return;
			for (var i = 0; i < m_SplitCount; ++i)
			{
				var obj = (Asteroid)ActorProvider.Provide(m_SplitAsteroid);
				obj.ResetProperties();
				obj.transform.position = transform.position;
				obj.Rigidbody.velocity = UnityEngine.Random.insideUnitCircle.normalized * obj.MoveSpeed;
				obj.gameObject.SetActive(true);
			}
		}
	}
	[Flags]
	public enum AsteroidFlags
	{
		None,
		Splitting,
		Stealth
	}
}