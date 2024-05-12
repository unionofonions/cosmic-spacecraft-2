
namespace Parlor.Game.Editor
{
	using UnityEditor;
	using Parlor.Game;
	using Parlor.Editor;

	[CustomEditor(typeof(Asteroid), editorForChildClasses: true)]
	public class AsteroidEditor : Editor
	{
		private SerializedProperty m_AsteroidFlagsProp;
		private SerializedProperty m_SplitAsteroidProp;
		private SerializedProperty m_SplitCountProp;

		protected void OnEnable()
		{
			m_AsteroidFlagsProp = serializedObject.PropertyOrThrow("m_AsteroidFlags");
			m_SplitAsteroidProp = serializedObject.PropertyOrThrow("m_SplitAsteroid");
			m_SplitCountProp = serializedObject.PropertyOrThrow("m_SplitCount");
		}
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			base.OnInspectorGUI();
			if (HasFlags(AsteroidFlags.Splitting))
			{
				EditorGUILayout.PropertyField(m_SplitAsteroidProp);
				EditorGUILayout.PropertyField(m_SplitCountProp);
			}
			serializedObject.ApplyModifiedProperties();
		}
		private bool HasFlags(AsteroidFlags flags)
		{
			return (m_AsteroidFlagsProp.intValue & (int)flags) != 0;
		}
	}
}