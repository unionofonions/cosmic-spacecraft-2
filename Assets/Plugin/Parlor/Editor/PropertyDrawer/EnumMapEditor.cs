#nullable enable

namespace Parlor.Editor
{
	using System.Linq;
	using UnityEngine;
	using UnityEditor;

	[CustomPropertyDrawer(typeof(EnumMap<,>), useForChildren: true)]
	public class EnumMapEditor : PropertyDrawer
	{
		protected override void Initialize(SerializedProperty property, GUIContent label)
		{
			m_Labels = this
				.PropertyFieldType()
				.GetGenericArguments()[0]
				.GetEnumNames()
				.Select(name => new GUIContent(name.Replace('_', '/')))
				.ToArray();
		}
		protected override void Update(Rect rect, SerializedProperty property, GUIContent label)
		{
			var valuesProp = property.PropertyOrThrow("m_Values");
			UpdateLength(valuesProp);
			rect.StretchHeight();
			var expand = valuesProp.isExpanded = EditorGUI.Foldout(
				rect,
				valuesProp.isExpanded,
				label,
				toggleOnLabelClick: true);
			if (expand)
			{
				++EditorGUI.indentLevel;
				rect.NewLine();
				for (var i = 0; i < m_Labels.Length; ++i)
				{
					var elem = valuesProp.GetArrayElementAtIndex(i);
					rect.StretchHeight(elem);
					EditorGUI.PropertyField(
						rect,
						elem,
						m_Labels[i],
						includeChildren: true);
					rect.NewLine(elem);
				}
				--EditorGUI.indentLevel;
			}
		}
		protected override float GetHeight(SerializedProperty property, GUIContent label)
		{
			var ret = EditorGUIUtility.singleLineHeight;
			var valuesProp = property.PropertyOrThrow("m_Values");
			UpdateLength(valuesProp);
			if (valuesProp.isExpanded)
			{
				for (var i = 0; i < m_Labels.Length; ++i)
					ret += EditorGUI.GetPropertyHeight(valuesProp.GetArrayElementAtIndex(i), includeChildren: true);
			}
			return ret;
		}

		#region Internal
#nullable disable
		private GUIContent[] m_Labels;

		private void UpdateLength(SerializedProperty valuesProp)
		{
			if (valuesProp.arraySize != m_Labels.Length) valuesProp.arraySize = m_Labels.Length;
		}
#nullable enable
		#endregion // Internal
	}
}