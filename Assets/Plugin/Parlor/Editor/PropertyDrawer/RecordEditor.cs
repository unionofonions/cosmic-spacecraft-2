#nullable enable

namespace Parlor.Editor
{
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;
	using UnityEditor;

	[CustomPropertyDrawer(typeof(RecordInfo), useForChildren: true)]
	public class RecordInfoEditor : PropertyDrawer
	{
		protected override void Update(Rect rect, SerializedProperty property, GUIContent label)
		{
			base.Update(rect, property, label);
			var validProp = property.PropertyOrThrow("m_Valid");
			validProp.boolValue = Valid(property);
		}
		protected virtual bool Valid(SerializedProperty property)
		{
			return true;
		}
	}
	[CustomPropertyDrawer(typeof(RecordReference<>), useForChildren: true)]
	public class RecordReferenceEditor : PropertyDrawer
	{
		protected override void Initialize(SerializedProperty property, GUIContent label)
		{
			var infoType = this
				.PropertyFieldType()
				.Hierarchy()
				.ToArray()[^2]
				.GenericTypeArguments[0];
			RecordDatabase.ClearAll();
			m_LibraryValues = RecordDatabase.LibrariesByInfoType(infoType) as UnityEngine.Object[];
			if (m_LibraryValues == null)
			{
				Error("No library type found.");
				return;
			}
			if (m_LibraryValues.Length == 0)
			{
				Error("No library asset found.");
				return;
			}
			m_LibraryLabels = m_LibraryValues
				.Select(elem => new GUIContent(elem.name.Replace('.', '/')))
				.ToArray();
			m_LibraryContentMap = new();
		}
		protected override void Update(Rect rect, SerializedProperty property, GUIContent label)
		{
			var libraryProp = property.PropertyOrThrow("m_Library");
			rect.StretchHeight();
			var expand = libraryProp.isExpanded = EditorGUI.Foldout(
				rect,
				libraryProp.isExpanded,
				label,
				toggleOnLabelClick: true);
			if (expand)
			{
				++EditorGUI.indentLevel;
				var idProp = property.PropertyOrThrow("m_Id");
				rect.NewLine();
				EditorGUI.BeginChangeCheck();
				var libIndex = EditorGUI.Popup(
					rect,
					EditorHelper.Label("Library"),
					LibraryIndex(libraryProp),
					m_LibraryLabels);
				if (EditorGUI.EndChangeCheck()) idProp.intValue = 0;
				SetLibraryIndex(libraryProp, libIndex);
				var libContent = GetLibraryContent(libraryProp);
				rect.NewLine();
				var idIndex = EditorGUI.Popup(
					rect,
					EditorHelper.Label("Info"),
					IdIndex(idProp, libContent),
					libContent.Labels);
				SetIdIndex(idProp, libContent, idIndex);
				if (property.hasVisibleChildren)
				{
					rect.NewLine();
					rect.StretchHeight(property);
					EditorGUI.PropertyField(
						rect,
						property,
						EditorHelper.Label("Params"),
						includeChildren: true);
				}
				--EditorGUI.indentLevel;
			}
		}
		protected override float GetHeight(SerializedProperty property, GUIContent label)
		{
			var ret = EditorGUIUtility.singleLineHeight;
			var libraryProp = property.PropertyOrThrow("m_Library");
			if (libraryProp.isExpanded)
			{
				ret += EditorGUIUtility.singleLineHeight * 2f;
				if (property.hasVisibleChildren) ret += EditorGUI.GetPropertyHeight(property, includeChildren: true);
			}
			return ret;
		}

		#region Internal
#nullable disable
		private GUIContent[] m_LibraryLabels;
		private UnityEngine.Object[] m_LibraryValues;
		private Dictionary<UnityEngine.Object, LibraryContent> m_LibraryContentMap;

		private int LibraryIndex(SerializedProperty libraryProp)
		{
			var value = libraryProp.objectReferenceValue;
			for (var i = 0; i < m_LibraryValues.Length; ++i)
				if (value == m_LibraryValues[i]) return i;
			return 0;
		}
		private void SetLibraryIndex(SerializedProperty libraryProp, int index)
		{
			libraryProp.objectReferenceValue = m_LibraryValues[index];
		}
		private LibraryContent GetLibraryContent(SerializedProperty libraryProp)
		{
			var value = libraryProp.objectReferenceValue;
			if (!m_LibraryContentMap.TryGetValue(value, out var ret))
				m_LibraryContentMap.Add(value, ret = new(value));
			return ret;
		}
		private int IdIndex(SerializedProperty idProp, LibraryContent libraryContent)
		{
			var value = idProp.intValue;
			for (var i = 0; i < libraryContent.Values.Length; ++i)
				if (value == libraryContent.Values[i].Id) return i;
			return 0;
		}
		private void SetIdIndex(SerializedProperty idProp, LibraryContent libraryContent, int index)
		{
			idProp.intValue = libraryContent.Values[index].Id;
		}

		private sealed class LibraryContent
		{
			public LibraryContent(UnityEngine.Object library)
			{
				Values = ((IEnumerable<RecordInfo>)library).ToArray();
				Labels = Values.Select(elem => new GUIContent(elem.Name)).ToArray();
			}

			public readonly GUIContent[] Labels;
			public readonly RecordInfo[] Values;
		}
#nullable enable
		#endregion // Internal
	}
	[CustomEditor(typeof(RecordLibrary<>), editorForChildClasses: true)]
	public class RecordLibraryEditor : Editor
	{
		protected void OnEnable()
		{
			m_HeapProp = serializedObject.PropertyOrThrow("m_Heap");
			m_NextIdProp = serializedObject.PropertyOrThrow("m_NextId");
		}
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			var reg = m_HeapProp.arraySize;
			base.OnInspectorGUI();
			if (reg != m_HeapProp.arraySize) UpdateIds();
			serializedObject.ApplyModifiedProperties();
		}

		#region Internal
#nullable disable
		private SerializedProperty m_HeapProp;
		private SerializedProperty m_NextIdProp;

		private void UpdateIds()
		{
			var idProps = m_HeapProp
				.Hierarchy()
				.Reverse()
				.Select(elem => elem.FindPropertyRelative("m_Id"))
				.ToArray();
			foreach (var elem in idProps)
				if (!Valid(elem.intValue)) elem.intValue = ++m_NextIdProp.intValue;
			bool Valid(int id)
			{
				if (id == 0) return false;
				var flag = false;
				foreach (var elem in idProps)
				{
					if (id == elem.intValue)
					{
						if (flag) return false;
						else flag = true;
					}
				}
				return true;
			}
		}
#nullable enable
		#endregion // Internal
	}
}