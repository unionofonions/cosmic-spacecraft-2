
namespace Parlor.Game
{
	using System.Collections;
	using UnityEngine;

	[DisallowMultipleComponent]
	[RequireComponent(typeof(SpriteRenderer))]
	public sealed class BackgroundSystem : MonoBehaviour
	{
		[SerializeField, NotDefault]
		private SpriteRenderer m_TransitionRenderer;
		[SerializeField, Unsigned]
		private float m_TransitionDuration;
		[SerializeField]
		private Formula m_TransitionFunction;
		[SerializeField, NotDefault]
		private Sprite[] m_TransitionSprites;
		private int m_TransitionSpriteIndex;
		private Sprite m_DefaultSprite;
		private SpriteRenderer m_Renderer;

		private void Start()
		{
			m_Renderer = GetComponent<SpriteRenderer>();
			if (
				m_Renderer.material != null &&
				m_TransitionRenderer != null &&
				m_TransitionRenderer.material != null &&
				m_TransitionSprites.Length != 0)
			{
				m_DefaultSprite = m_TransitionSprites[0] != null ? m_TransitionSprites[0] : m_Renderer.sprite;
				Domain.GetSpawnSystem().OnBeginSpawn += OnBeginSpawn;
				Domain.GetSpawnSystem().OnBeginLevel += OnBeginLevel;
			}
		}
		private void OnBeginSpawn()
		{
			m_TransitionSpriteIndex = 0;
		}
		private void OnBeginLevel(LevelInfo level)
		{
			if (m_TransitionSpriteIndex == 0)
			{
				StopAllCoroutines();
				m_Renderer.sprite = m_DefaultSprite;
				SetRendererAlpha(1f);
				m_TransitionRenderer.gameObject.SetActive(false);
			}
			else if (m_TransitionSpriteIndex < m_TransitionSprites.Length)
			{
				StopAllCoroutines();
				var sprite = m_TransitionSprites[m_TransitionSpriteIndex];
				TransitionSprite(sprite);
			}
			++m_TransitionSpriteIndex;
		}
		private void TransitionSprite(Sprite sprite)
		{
			if (sprite != null)
			{
				m_TransitionRenderer.sprite = sprite;
				SetTransitionRendererAlpha(0f);
				m_TransitionRenderer.gameObject.SetActive(true);
				StartCoroutine(TransitionSpriteAsync());
			}
			IEnumerator TransitionSpriteAsync()
			{
				var timer = 0f;
				var tMul = 1f / m_TransitionDuration;
				while (timer < 1f)
				{
					timer += Time.deltaTime * tMul;
					var y = m_TransitionFunction.Evaluate(timer);
					SetRendererAlpha(1f - y);
					SetTransitionRendererAlpha(y);
					yield return null;
				}
				m_Renderer.sprite = m_TransitionRenderer.sprite;
				SetRendererAlpha(1f);
				m_TransitionRenderer.gameObject.SetActive(false);
			}
		}
		private void SetRendererAlpha(float value)
		{
			var color = m_Renderer.material.color;
			color.a = value;
			m_Renderer.material.color = color;
		}
		private void SetTransitionRendererAlpha(float value)
		{
			var color = m_TransitionRenderer.material.color;
			color.a = value;
			m_TransitionRenderer.material.color = color;
		}
	}
}