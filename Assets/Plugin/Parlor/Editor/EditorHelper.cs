#nullable enable

namespace Parlor.Editor
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.InteropServices;
	using UnityEngine;
	using UnityEditor;

	static public class EditorHelper
	{
		static public GUIContent Label(string? text, [Optional] string? tooltip, [Optional] Texture? image)
		{
			var ret = LabelPool.Provide();
			ret.text = text;
			ret.tooltip = tooltip;
			ret.image = image;
			return ret;
		}
		static public Type PropertyFieldType(this UnityEditor.PropertyDrawer drawer)
		{
			if (drawer == null) throw new ArgumentNullException(nameof(drawer));
			var reg = drawer.fieldInfo.FieldType;
			return reg.IsArray ? reg.GetElementType() : reg;
		}
		static public SerializedProperty PropertyOrThrow(this SerializedProperty property, string name)
		{
			if (property == null) throw new ArgumentNullException(nameof(property));
			var ret = property.FindPropertyRelative(name);
			if (ret == null) throw new ArgumentException($"Property not found. Property: {property}, name: {name}.");
			return ret;
		}
		static public SerializedProperty PropertyOrThrow(this SerializedObject serializedObject, string name)
		{
			if (serializedObject == null) throw new ArgumentNullException(nameof(serializedObject));
			var ret = serializedObject.FindProperty(name);
			if (ret == null) throw new ArgumentException($"Property not found. SerializedObject: {serializedObject}, name: {name}.");
			return ret;
		}
		static public IEnumerable<SerializedProperty> Hierarchy(this SerializedProperty property)
		{
			if (property == null) throw new ArgumentNullException(nameof(property));
			if (!property.isArray) throw new ArgumentException($"Property must be an array. Property: {property}.");
			var count = property.arraySize;
			for (var i = 0; i < count; ++i) yield return property.GetArrayElementAtIndex(i);
		}
		static public void NewLine(this ref Rect rect)
		{
			rect.y += EditorGUIUtility.singleLineHeight;
		}
		static public void NewLine(this ref Rect rect, SerializedProperty property, bool includeBase = true)
		{
			rect.y += property != null ? EditorGUI.GetPropertyHeight(property, includeBase) : EditorGUIUtility.singleLineHeight;
		}
		static public void StretchHeight(this ref Rect rect)
		{
			rect.height = EditorGUIUtility.singleLineHeight;
		}
		static public void StretchHeight(this ref Rect rect, SerializedProperty property, bool includeBase = true)
		{
			rect.height = property != null ? EditorGUI.GetPropertyHeight(property, includeBase) : EditorGUIUtility.singleLineHeight;
		}

		#region Internal
		static private class LabelPool
		{
			static LabelPool()
			{
				s_Collection = new GUIContent[c_Size];
				for (var i = 0; i < s_Collection.Length; ++i) s_Collection[i] = new();
				s_Index = 0;
			}

			private const int c_Size = 2;
			static private readonly GUIContent[] s_Collection;
			static private int s_Index;

			static public GUIContent Provide()
			{
				return s_Collection[s_Index = (s_Index + 1) % s_Collection.Length];
			}
		}
		#endregion // Internal
	}
}