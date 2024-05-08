
namespace Parlor.Game
{
	using UnityEngine;

	[DisallowMultipleComponent]
	[RequireComponent(typeof(SpriteRenderer))]
	public sealed class BackgroundSystem : MonoBehaviour
	{
		private int m_ScrollSpeedHash;
		private SpriteRenderer m_Renderer;

		public Sprite GetSprite()
		{
			LazyAwake();
			return m_Renderer.sprite;
		}
		public void SetSprite(Sprite value)
		{
			if (value == null) return;
			LazyAwake();
			m_Renderer.sprite = value;
		}
		public float GetScrollSpeed()
		{
			LazyAwake();
			return m_Renderer.material.GetFloat(m_ScrollSpeedHash);
		}
		public void SetScrollSpeed(float value)
		{
			LazyAwake();
			m_Renderer.material.SetFloat(m_ScrollSpeedHash, value);
		}
		private void LazyAwake()
		{
			if (m_Renderer == null)
			{
				m_Renderer = GetComponent<SpriteRenderer>();
				m_ScrollSpeedHash = Shader.PropertyToID("_ScrollSpeed");
			}
		}
	}
}