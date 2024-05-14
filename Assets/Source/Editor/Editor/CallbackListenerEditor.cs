
namespace Parlor.Game.Editor
{
	using UnityEditor;
	using Parlor.Editor;
	using Parlor.Game.UI;

	[CustomEditor(typeof(CallbackListener), editorForChildClasses: true)]
	public class CallbackListenerEditor : Editor
	{
		private SerializedProperty m_CallbacksProp;
		private SerializedProperty m_OnAwakeProp;
		private SerializedProperty m_OnStartProp;
		private SerializedProperty m_OnEnableProp;
		private SerializedProperty m_OnDisableProp;

		protected void OnEnable()
		{
			m_CallbacksProp = serializedObject.PropertyOrThrow("m_Callbacks");
			m_OnAwakeProp = serializedObject.PropertyOrThrow("m_OnAwake");
			m_OnStartProp = serializedObject.PropertyOrThrow("m_OnStart");
			m_OnEnableProp = serializedObject.PropertyOrThrow("m_OnEnable");
			m_OnDisableProp = serializedObject.PropertyOrThrow("m_OnDisable");
		}
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUILayout.PropertyField(m_CallbacksProp);
			if (HasFlag(CallbackType.Awake))
			{
				EditorGUILayout.PropertyField(m_OnAwakeProp);
			}
			if (HasFlag(CallbackType.Start))
			{
				EditorGUILayout.PropertyField(m_OnStartProp);
			}
			if (HasFlag(CallbackType.OnEnable))
			{
				EditorGUILayout.PropertyField(m_OnEnableProp);
			}
			if (HasFlag(CallbackType.OnDisable))
			{
				EditorGUILayout.PropertyField(m_OnDisableProp);
			}
			serializedObject.ApplyModifiedProperties();
		}
		private bool HasFlag(CallbackType flag)
		{
			return (m_CallbacksProp.enumValueFlag & (int)flag) != 0;
		}
	}
}