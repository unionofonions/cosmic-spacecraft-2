#nullable enable

namespace Parlor.Editor
{
	using UnityEngine;
	using UnityEditor;

	[CustomPropertyDrawer(typeof(Curve), useForChildren: true)]
	public class CurveEditor : PropertyDrawer
	{
		protected override void Update(Rect rect, SerializedProperty property, GUIContent label)
		{
			EditorGUI.CurveField(
				rect,
				property.PropertyOrThrow("m_Function"),
				new Color(0.9f, 0.1f, 0.1f),
				default(Rect),
				label);
		}
		protected override float GetHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}
	}
}