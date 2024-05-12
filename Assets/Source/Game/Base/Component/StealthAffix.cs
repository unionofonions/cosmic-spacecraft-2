
namespace Parlor.Game
{
	using UnityEngine;

	[DisallowMultipleComponent]
	[RequireComponent(typeof(SpriteRenderer))]
	public class StealthAffix : MonoBehaviour
	{
		[SerializeField]
		private CurveReference m_InvisibilityCurve;
		private Curve m_InvisibilityCurveValue;
		private SpriteRenderer m_Renderer;

		protected void Awake()
		{
			m_Renderer = GetComponent<SpriteRenderer>();
			m_InvisibilityCurveValue = m_InvisibilityCurve.Info?.Value;
		}
		protected void FixedUpdate()
		{
			if (m_InvisibilityCurveValue == null) return;
			var distance = Vector2.Distance(Domain.GetPlayer().transform.position, transform.position);
			var alpha = m_InvisibilityCurveValue.Evaluate(distance);
			var color = m_Renderer.color;
			color.a = alpha;
			m_Renderer.color = color;
		}
		protected void OnEnable()
		{
			var color = m_Renderer.color;
			color.a = 0.0f;
			m_Renderer.color = color;
		}
	}
}