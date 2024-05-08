#nullable enable

using System;
using UnityEngine;
using UnityEditor;

namespace Parlor.Editor
{
	[CustomPropertyDrawer(typeof(LimitedAttribute), useForChildren: true)]
	public class LimitedAttributeEditor : PropertyDrawer
	{
		protected override void Initialize(SerializedProperty property, GUIContent label)
		{
			FieldType fieldType;
			switch (property.propertyType)
			{
				case SerializedPropertyType.Generic:
					var _fieldType = this.PropertyFieldType();
					if (_fieldType == typeof(Range))
					{
						fieldType = FieldType.Range;
						break;
					}
					else if (_fieldType == typeof(RangeInt))
					{
						fieldType = FieldType.RangeInt;
						break;
					}
					goto default;
				case SerializedPropertyType.Integer:
					fieldType = FieldType.Integer;
					break;
				case SerializedPropertyType.Float:
					fieldType = FieldType.Float;
					break;
				default:
					Revert();
					return;
			}
			var attr = (LimitedAttribute)attribute;
			m_Limit = new(fieldType, attr.Min, attr.Max);
			if (Single.IsInfinity(m_Limit.Min) && Single.IsInfinity(m_Limit.Max))
			{
				Error("Min and max cannot be infinity.");
				return;
			}
			if (Single.IsNaN(m_Limit.Min) || Single.IsNaN(m_Limit.Max))
			{
				Error("Min or max cannot be NaN.");
				return;
			}
			if (m_Limit.FieldType is FieldType.Range or FieldType.RangeInt && m_Limit.Open)
			{
				Error("Range cannot be open.");
				return;
			}
		}
		protected override void Update(Rect rect, SerializedProperty property, GUIContent label)
		{
			switch (m_Limit.FieldType)
			{
				case FieldType.Float:
					DrawFloatField();
					return;
				case FieldType.Integer:
					DrawIntegerField();
					return;
				case FieldType.Range:
					DrawRangeField();
					return;
				case FieldType.RangeInt:
					DrawRangeIntField();
					return;
			}
			void DrawFloatField()
			{
				var value = property.floatValue;
				if (m_Limit.Open)
				{
					value = EditorGUI.FloatField(rect, label, value);
				}
				else
				{
					value = EditorGUI.Slider(rect, label, value, m_Limit.Min, m_Limit.Max);
				}
				property.floatValue = m_Limit.Range.Limit(value);
			}
			void DrawIntegerField()
			{
				var value = property.intValue;
				if (m_Limit.Open)
				{
					value = EditorGUI.IntField(rect, label, value);
				}
				else
				{
					value = EditorGUI.IntSlider(rect, label, value, (int)m_Limit.Min, (int)m_Limit.Max);
				}
				property.intValue = (int)m_Limit.Range.Limit(value);
			}
			void DrawRangeField()
			{
				var startProp = property.PropertyOrThrow("Start");
				var endProp = property.PropertyOrThrow("End");
				var value = new Range(startProp.floatValue, endProp.floatValue);
				this.DrawRangeField(rect, ref value, m_Limit.Range, label);
				startProp.floatValue = value.Start;
				endProp.floatValue = value.End;
			}
			void DrawRangeIntField()
			{
				var startProp = property.PropertyOrThrow("Start");
				var endProp = property.PropertyOrThrow("End");
				var value = new Range(startProp.intValue, endProp.intValue);
				this.DrawRangeField(rect, ref value, m_Limit.Range, label);
				startProp.intValue = (int)value.Start;
				endProp.intValue = (int)value.End;
			}
		}

		#region Internal
		private Limit m_Limit;

		private void DrawRangeField(Rect rect, ref Range value, Range limit, GUIContent label)
		{
			const float SliderWidthPercentage = 0.8f;
			const float boxWidthPercentage = 0.175f;
			var totalWidth = rect.width;
			var sliderWidth = totalWidth * SliderWidthPercentage;
			var boxWidth = totalWidth * boxWidthPercentage / 2f;
			var spaceWidth = totalWidth * (1f - SliderWidthPercentage - boxWidthPercentage) / 2f;
			rect.width = sliderWidth;
			EditorGUI.MinMaxSlider(rect, label, ref value.Start, ref value.End, limit.Start, limit.End);
			rect.x += sliderWidth + spaceWidth;
			rect.width = boxWidth;
			value.Start = EditorGUI.FloatField(rect, value.Start);
			rect.x += boxWidth + spaceWidth;
			value.End = EditorGUI.FloatField(rect, value.End);
			value.Start = Mathf.Clamp(value.Start, limit.Start, value.End);
			value.End = Mathf.Clamp(value.End, value.Start, limit.End);
		}

		private readonly struct Limit
		{
			public Limit(FieldType fieldType, float min, float max)
			{
				FieldType = fieldType;
				Range = min < max ? new(min, max) : new(max, min);
				Open = Single.IsInfinity(min) || Single.IsInfinity(max);
			}

			public readonly FieldType FieldType;
			public readonly Range Range;
			public readonly bool Open;

			public float Min
			{
				get => Range.Start;
			}
			public float Max
			{
				get => Range.End;
			}
		}
		private enum FieldType
		{
			Float,
			Integer,
			Range,
			RangeInt
		}
		#endregion // Internal
	}
}