using UnityEngine;

namespace Parlor.Game.UI
{
	using Parlor.Runtime;

	[DisallowMultipleComponent]
	public sealed class FloatingTextSystem : MonoBehaviour
	{
		[SerializeField, NotDefault]
		private FloatingTextComponent m_ComponentScheme;
		[SerializeField]
		private float m_RandomPositionRadius;
		private ComponentPool<FloatingTextComponent> m_Pool;
		private RandomHelper m_RandomHelper;

		private void Awake()
		{
			if (m_ComponentScheme == null) return;
			m_Pool = new(m_ComponentScheme, active: elem => elem.Active);
			m_Pool.Provide().gameObject.SetActive(false);
			m_RandomHelper = new(m_RandomPositionRadius);
			Actor.OnReaction += OnReaction;
			Actor.OnAction += OnAction;
			Domain.GetSpawnSystem().OnBeginSpawn += ReturnAll;
		}
		public void ReturnAll()
		{
			if (m_Pool == null) return;
			foreach (var elem in m_Pool)
			{
				elem.gameObject.SetActive(false);
			}
		}
		private void OnReaction(in ReactionInfo info)
		{
			var comp = m_Pool.Provide();
			var pos = info.HitPosition + m_RandomHelper.Position();
			comp.PlayReactionAnimation(pos, info);
		}
		private void OnAction(Actor subject, string actionName, object args)
		{
			var comp = m_Pool.Provide();
			var pos = subject.transform.position;
			comp.PlayActionAnimation(pos, actionName, args);
		}

		private sealed class RandomHelper
		{
			private const int c_BufSize = 16;
			private readonly Vector3[] m_Buffer;
			private int m_Index;

			public RandomHelper(float randomPositionRadius)
			{
				m_Buffer = new Vector3[c_BufSize];
				for (var i = 0; i < m_Buffer.Length; ++i)
				{
					m_Buffer[i] = Random.Circle(outerRadius: randomPositionRadius);
				}
				m_Index = 0;
			}

			public Vector3 Position()
			{
				return m_Buffer[m_Index = (m_Index + 1) % c_BufSize];
			}
		}
	}
}