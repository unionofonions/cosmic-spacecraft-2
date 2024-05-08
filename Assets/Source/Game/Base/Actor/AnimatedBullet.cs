
namespace Parlor.Game
{
	using UnityEngine;

	[RequireComponent(typeof(SpriteRenderer))]
	public class AnimatedBullet : Bullet
	{
		[SerializeField, MinEpsilon]
		private float m_UpdateInterval;
		[SerializeField, NotDefault]
		private Sprite[] m_Sprites;
		private float m_Timer;
		private int m_Index;
		private SpriteRenderer m_Renderer;

		protected new void Awake()
		{
			base.Awake();
			m_Renderer = GetComponent<SpriteRenderer>();
		}
		protected void Update()
		{
			if (m_Sprites.Length != 0)
			{
				m_Timer -= Time.deltaTime;
				if (m_Timer <= 0)
				{
					m_Timer = m_UpdateInterval;
					m_Renderer.sprite = m_Sprites[m_Index];
					m_Index = (m_Index + 1) % m_Sprites.Length;
				}
			}
		}
		protected new void OnEnable()
		{
			base.OnEnable();
			m_Index = 0;
			m_Timer = 0f;
		}
	}
}