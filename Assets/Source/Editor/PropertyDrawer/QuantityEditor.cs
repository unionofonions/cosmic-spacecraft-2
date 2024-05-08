using UnityEngine;
using UnityEditor;

namespace Parlor.Game.Editor
{
	using Parlor.Editor;

	[CustomPropertyDrawer(typeof(Quantity))]
	public sealed class QuantityEditor : PropertyDrawer
	{
		protected override void Update(Rect rect, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginChangeCheck();
			base.Update(rect, property, label);
			if (EditorGUI.EndChangeCheck())
			{
				var currentProp = property.PropertyOrThrow("m_Current");
				var maxProp = property.PropertyOrThrow("m_Max");
				maxProp.floatValue = Mathf.Max(maxProp.floatValue, 0f);
				currentProp.floatValue = Mathf.Clamp(currentProp.floatValue, 0f, maxProp.floatValue);
			}
		}
	}
}