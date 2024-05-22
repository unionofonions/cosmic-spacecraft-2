using System;
using UnityEngine;

namespace Parlor.Game
{
	using Parlor.Diagnostics;

	[RequireComponent(typeof(Rigidbody2D))]
	[EditorHandled]
	public class Asteroid : Actor, IDestroyOnExitBoundary
	{
		private const float c_LimitedRandomDirectionMaxAngle = 40f;

		[Header("Asteroid")]
		[SerializeField]
		private AsteroidFlags m_AsteroidFlags;
		[SerializeField, Limited(0f, 360f)]
		private Range m_SpawnArc;
		[SerializeField, HideInInspector, NotDefault]
		private Asteroid m_SplitAsteroid;
		[SerializeField, HideInInspector, MinOne]
		private int m_SplitCount;
		private Rigidbody2D m_Rigidbody;

		protected new void Awake()
		{
			base.Awake();
			m_Rigidbody = GetComponent<Rigidbody2D>();
		}
		protected void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.TryGetComponent<Actor>(out var target) && target.IsHostileAgainst(this))
			{
				target.TakeDamage(instigator: this, hitPosition: transform.position);
				Die(instigator: null);
			}
		}
		public override void Respawn()
		{
			var radius = Domain.GetBoundarySystem().Radius;
			var pos = Random.Circle(outerRadius: radius, innerRadius: radius, minAngle: m_SpawnArc.Start, maxAngle: m_SpawnArc.End);
			var dir = Random.Boolean(0.5f) ? SpawnDirection.LimitedRandom : SpawnDirection.TowardsPlayer;
			Respawn(pos, dir);
		}
		public override void Die(Actor instigator)
		{
			base.Die(instigator);
			if (instigator != null && (m_AsteroidFlags & AsteroidFlags.Splitting) != 0)
			{
				Split();
			}
		}
		private void Respawn(Vector3 position, SpawnDirection direction)
		{
			base.Respawn();
			transform.position = position;
			Vector2 dirVec;
			switch (direction)
			{
				default:
				case SpawnDirection.FullyRandom:
					dirVec = Random.Circle(outerRadius: 1.0f, innerRadius: 1.0f);
					break;
				case SpawnDirection.LimitedRandom:
					var bisector = Domain.GetBoundarySystem().Center - position;
					var angle = MathHelper.Dir2Deg(bisector);
					dirVec = Random.Circle(
						outerRadius: 1.0f,
						innerRadius: 1.0f,
						minAngle: angle - c_LimitedRandomDirectionMaxAngle,
						maxAngle: angle + c_LimitedRandomDirectionMaxAngle);
					break;
				case SpawnDirection.TowardsPlayer:
					dirVec = (Domain.GetPlayer().transform.position - position).normalized;
					break;
			}
			m_Rigidbody.velocity = dirVec * MoveSpeed;
			transform.rotation = new(0f, 0f, Random.Single(0f, 1f), Random.Single(-1f, 1f));
		}
		private void Split()
		{
			if (m_SplitAsteroid == null) return;
			for (var i = 0; i < m_SplitCount; ++i)
			{
				var obj = (Asteroid)ActorProvider.Provide(m_SplitAsteroid);
				obj.Respawn(transform.position, SpawnDirection.FullyRandom);
			}
		}

		private enum SpawnDirection
		{
			FullyRandom,
			LimitedRandom,
			TowardsPlayer
		}
	}
	[Flags]
	public enum AsteroidFlags
	{
		None = 0x0,
		Splitting = 0x1
	}
}