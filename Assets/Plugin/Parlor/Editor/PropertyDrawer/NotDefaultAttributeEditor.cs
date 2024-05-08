#nullable enable

namespace Parlor.Editor
{
	using System;
	using UnityEngine;
	using UnityEditor;

	[CustomPropertyDrawer(typeof(NotDefaultAttribute), useForChildren: true)]
	public class NotDefaultAttributeEditor : PropertyDrawer
	{
		protected override void Initialize(SerializedProperty property, GUIContent label)
		{
			var valid = property.propertyType is
				SerializedPropertyType.String or
				SerializedPropertyType.ObjectReference;
			if (!valid) Revert();
		}
		protected override void Update(Rect rect, SerializedProperty property, GUIContent label)
		{
			var def = property.propertyType switch
			{
				SerializedPropertyType.String => String.IsNullOrEmpty(property.stringValue),
				SerializedPropertyType.ObjectReference => property.objectReferenceValue == null,
				_ => false
			};
			var reg = GUI.backgroundColor;
			if (def) GUI.backgroundColor = new(1.0f, 0.2f, 0.2f);
			base.Update(rect, property, label);
			if (def) GUI.backgroundColor = reg;
		}
	}
}