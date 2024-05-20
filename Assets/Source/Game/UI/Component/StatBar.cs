
namespace Parlor.Game
{
	using System.Reflection;
	using UnityEngine;
	using UnityEngine.UI;
	using Parlor.Diagnostics;

	[DisallowMultipleComponent]
	public sealed class StatBar : MonoBehaviour
	{
		[SerializeField, NotDefault]
		private string m_PropertyName;
		[SerializeField]
		private Range m_ValueRange;
		[SerializeField]
		private bool m_Reverse;
		[SerializeField]
		private Color m_ColorMin;
		[SerializeField]
		private Color m_ColorMax;
		[SerializeField, NotDefault]
		private Image m_Fill;
		private object m_Target;
		private PropertyInfo m_PropertyInfo;

		private void Awake()
		{
			m_Target = Domain.GetPlayer();
			m_PropertyInfo = typeof(Player).GetProperty(m_PropertyName);
			if (m_PropertyInfo == null)
			{
				Log.Warning($"Property '{m_PropertyName}' not found.");
			}
		}
		private void OnEnable()
		{
			UpdateFill();
		}
		private void UpdateFill()
		{
			if (m_PropertyInfo != null && m_Fill != null)
			{
				var obj = m_PropertyInfo.GetValue(m_Target);
				float value;
				if (obj is float single) value = single; else
				if (obj is int int32) value = int32; else
				if (obj is Quantity quantity) value = quantity.Max;
				else value = InvalidPropertyType();
				var ratio = Mathf.InverseLerp(m_ValueRange.Start, m_ValueRange.End, value);
				if (m_Reverse) ratio = 1f - ratio;
				m_Fill.fillAmount = ratio;
				UpdateColor(ratio);
			}
		}
		private void UpdateColor(float ratio)
		{
			m_Fill.color = Color.Lerp(m_ColorMin, m_ColorMax, ratio);
		}
		private float InvalidPropertyType()
		{
			Log.Warning($"Type of property '{m_PropertyName}' is invalid.");
			return 0f;
		}
	}
}