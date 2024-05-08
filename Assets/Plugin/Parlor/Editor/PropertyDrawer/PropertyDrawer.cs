#nullable enable

namespace Parlor.Editor
{
	using UnityEngine;
	using UnityEditor;

	public abstract class PropertyDrawer : UnityEditor.PropertyDrawer
	{
		protected virtual void Initialize(SerializedProperty property, GUIContent label)
		{
			/*nop*/
		}
		protected virtual void Update(Rect rect, SerializedProperty property, GUIContent label)
		{
			EditorGUI.PropertyField(rect, property, label, includeChildren: true);
		}
		protected virtual float GetHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(property, label, includeChildren: true);
		}
		protected void Error(string? message)
		{
			m_Exception = message;
		}
		protected void Revert()
		{
			m_Exception = s_Revert;
		}

		#region Internal
		static PropertyDrawer()
		{
			s_Revert = new();
		}

		static private readonly object s_Revert;
		private bool m_Initialized;
		private object? m_Exception;

		public sealed override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
		{
			InitializeInternal(property, label);
			if (m_Exception != null)
			{
				if (m_Exception == s_Revert) EditorGUI.PropertyField(rect, property, label, includeChildren: true);
				else EditorGUILayout.HelpBox(m_Exception.ToString(), MessageType.Error);
			}
			else Update(rect, property, label);
		}
		public sealed override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			InitializeInternal(property, label);
			return m_Exception != null
				? m_Exception == s_Revert ? EditorGUI.GetPropertyHeight(property, label, includeChildren: true) : EditorGUIUtility.singleLineHeight
				: GetHeight(property, label);
		}
		private void InitializeInternal(SerializedProperty property, GUIContent label)
		{
			if (m_Initialized) return;
			Initialize(property, label);
			m_Initialized = true;
		}
		#endregion // Internal
	}
}