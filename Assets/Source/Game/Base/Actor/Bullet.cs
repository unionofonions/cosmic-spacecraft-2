
namespace Parlor.Game
{
	using UnityEngine;
	using Parlor.Runtime;

	[DisallowMultipleComponent]
	[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
	public class Bullet : MonoBehaviour, IDestroyOnExitBoundary
	{
		private Actor m_Source;
		private bool m_Active;
		private Rigidbody2D m_Rigidbody;

		public bool Active
		{
			get => m_Active;
		}

		protected void Awake()
		{
			m_Rigidbody = GetComponent<Rigidbody2D>();
		}
		protected void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.TryGetComponent<Actor>(out var target) && target.IsHostileAgainst(m_Source))
			{
				target.TakeDamage(instigator: m_Source, hitPosition: transform.position);
				gameObject.SetActive(false);
			}
		}
		protected void OnEnable()
		{
			m_Active = true;
		}
		protected void OnDisable()
		{
			m_Active = false;
		}
		public void Fire(Actor source, Vector3 position, Quaternion rotation, float speed)
		{
			if (source == null) return;
			m_Source = source;
			transform.SetPositionAndRotation(position, rotation);
			gameObject.SetActive(true);
			m_Rigidbody.velocity = transform.up * speed;
		}
	}
	static public class BulletProvider
	{
		static private readonly ComponentMap<Bullet> s_Map;

		static BulletProvider()
		{
			s_Map = new(create: scheme => new(scheme, active: elem => elem.Active));
		}

		static public Bullet Provide(Bullet scheme)
		{
			return s_Map.Provide(scheme);
		}
		static public void ReturnAll()
		{
			foreach (var coll in s_Map)
			{
				foreach (var elem in coll)
				{
					elem.gameObject.SetActive(false);
				}
			}
		}
	}
}