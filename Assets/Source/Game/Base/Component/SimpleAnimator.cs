
namespace Parlor.Game
{
	using UnityEngine;

	[DisallowMultipleComponent]
	[RequireComponent(typeof(SpriteRenderer))]
	public class SimpleAnimator : MonoBehaviour
	{
		[SerializeField, MinEpsilon]
		private float m_IntervalBetweenFrames;
		[SerializeField, NotDefault]
		private Sprite[] m_Frames;
		private int m_FrameIndex;
		private float m_Timer;
		private SpriteRenderer m_Renderer;

		protected void Awake()
		{
			m_Renderer = GetComponent<SpriteRenderer>();
		}
		protected void Update()
		{
			m_Timer -= Time.deltaTime;
			if (m_Timer <= 0f)
			{
				m_Timer = m_IntervalBetweenFrames;
				m_Renderer.sprite = m_Frames[m_FrameIndex];
				m_FrameIndex = (m_FrameIndex + 1) % m_Frames.Length;
			}
		}
		protected void OnEnable()
		{
			m_FrameIndex = 0;
			m_Timer = 0f;
		}
	}
}