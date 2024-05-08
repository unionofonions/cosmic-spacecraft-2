#nullable enable

namespace Parlor.Editor
{
	using UnityEngine;
	using UnityEditor;

	[CustomPropertyDrawer(typeof(ReadOnlyAttribute), useForChildren: true)]
	public class ReadOnlyAttributeEditor : PropertyDrawer
	{
		protected override void Update(Rect rect, SerializedProperty property, GUIContent label)
		{
			var reg = GUI.enabled;
			GUI.enabled = false;
			base.Update(rect, property, label);
			GUI.enabled = reg;
		}
	}
}