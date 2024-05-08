#nullable enable

namespace Parlor.Editor
{
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(Curve3), useForChildren: true)]
	public class Curve3Editor : PropertyDrawer
	{
		protected override void Update(Rect rect, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginChangeCheck();
			base.Update(rect, property, label);
			if (EditorGUI.EndChangeCheck())
			{
				var valid = 0;
				if (Valid("m_X")) valid |= 1;
				if (Valid("m_Y")) valid |= 2;
				if (Valid("m_Z")) valid |= 4;
				property.PropertyOrThrow("m_Valid").intValue = valid;
			}
			bool Valid(string name)
			{
				return property
					.PropertyOrThrow(name)
					.PropertyOrThrow("m_Function")
					.animationCurveValue
					.length > 0;
			}
		}
	}
}