#nullable enable

namespace Parlor.Editor
{
	using System;
	using System.Linq;
	using UnityEngine;
	using UnityEditor;

	[CustomPropertyDrawer(typeof(Bank<>))]
	public class BankEditor : PropertyDrawer
	{
		protected override void Update(Rect rect, SerializedProperty property, GUIContent label)
		{
			var collectionProp = property.PropertyOrThrow("m_Collection");
			if (collectionProp.arraySize <= 1) EditorGUI.PropertyField(rect, collectionProp, label);
			else
			{
				EditorGUI.BeginChangeCheck();
				rect.StretchHeight(property);
				base.Update(rect, property, label);
				if (EditorGUI.EndChangeCheck())
				{
					foreach (var elem in collectionProp.Hierarchy())
						elem.PropertyOrThrow("m_Weighted").boolValue = Mode(property) is BankMode.FullyWeighted;
					switch (Mode(property))
					{
						case BankMode.NonRepRandom:
							property.PropertyOrThrow("m_Threshold").intValue = (int)(collectionProp.arraySize * c_ThresholdPercentage);
							break;
						case BankMode.FullyWeighted:
							var totalWeight = collectionProp
								.Hierarchy()
								.Sum(elem => elem.PropertyOrThrow("m_Weight").floatValue);
							property.PropertyOrThrow("m_TotalWeight").floatValue = totalWeight;
							foreach (var elem in collectionProp.Hierarchy())
							{
								var chance = elem.PropertyOrThrow("m_Weight").floatValue / totalWeight * 100f;
								elem.PropertyOrThrow("m_Chance").floatValue = Single.IsNaN(chance) ? 0f : chance;
							}
							break;
					}
				}
				switch (Mode(property))
				{
					case BankMode.NonRepRandom:
						++EditorGUI.indentLevel;
						var thresholdProp = property.PropertyOrThrow("m_Threshold");
						rect.NewLine(property);
						rect.StretchHeight();
						EditorGUI.PropertyField(rect, thresholdProp);
						--EditorGUI.indentLevel;
						break;
					case BankMode.FullyWeighted:
						++EditorGUI.indentLevel;
						var totalWeight = property.PropertyOrThrow("m_TotalWeight");
						rect.NewLine(property);
						rect.StretchHeight();
						EditorGUI.PropertyField(rect, totalWeight);
						--EditorGUI.indentLevel;
						break;
				}
			}
		}
		protected override float GetHeight(SerializedProperty property, GUIContent label)
		{
			var collectionProp = property.PropertyOrThrow("m_Collection");
			if (collectionProp.arraySize <= 1) return EditorGUI.GetPropertyHeight(collectionProp, label);
			else
			{
				var ret = base.GetHeight(property, label);
				if (Mode(property) is not BankMode.FullyRandom) ret += EditorGUIUtility.singleLineHeight;
				return ret;
			}
		}

		#region Internal
		private const float c_ThresholdPercentage = 0.5f;

		private BankMode Mode(SerializedProperty property)
		{
			var modeProp = property.PropertyOrThrow("m_Mode");
			return (BankMode)modeProp.enumValueIndex;
		}
		#endregion // Internal
	}
	[CustomPropertyDrawer(typeof(Bank<>.Entry))]
	public class BankEntryEditor : PropertyDrawer
	{
		protected override void Update(Rect rect, SerializedProperty property, GUIContent label)
		{
			var weightedProp = property.PropertyOrThrow("m_Weighted");
			if (weightedProp.boolValue) base.Update(rect, property, label);
			else
			{
				var valueProp = property.PropertyOrThrow("m_Value");
				EditorGUI.PropertyField(rect, valueProp, label, includeChildren: true);
			}
		}
		protected override float GetHeight(SerializedProperty property, GUIContent label)
		{
			var weightedProp = property.PropertyOrThrow("m_Weighted");
			if (weightedProp.boolValue) return base.GetHeight(property, label);
			else
			{
				var valueProp = property.PropertyOrThrow("m_Value");
				return EditorGUI.GetPropertyHeight(valueProp, label, includeChildren: true);
			}
		}
	}
}