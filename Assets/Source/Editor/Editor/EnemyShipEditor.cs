
namespace Parlor.Game.Editor
{
	using UnityEditor;
	using Parlor.Editor;

	[CustomEditor(typeof(EnemyShip), editorForChildClasses: true)]
	public class EnemyShipEditor : Editor
	{
		private SerializedProperty m_FlagsProp;
		private SerializedProperty m_RotationSpeedProp;
		private SerializedProperty m_DynamicSpinProp;

		protected void OnEnable()
		{
			m_FlagsProp = serializedObject.PropertyOrThrow("m_Flags");
			m_RotationSpeedProp = serializedObject.PropertyOrThrow("m_RotationSpeed");
			m_DynamicSpinProp = serializedObject.PropertyOrThrow("m_DynamicSpin");
		}
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			base.OnInspectorGUI();
			if ((m_FlagsProp.intValue & (int)EnemyShipFlags.Boss) != 0)
			{
				EditorGUILayout.PropertyField(m_RotationSpeedProp);
				EditorGUILayout.PropertyField(m_DynamicSpinProp);
			}
			serializedObject.ApplyModifiedProperties();
		}
	}
}