using UnityEngine;
using UnityEditor;

namespace Parlor.Game.Editor
{
	using Parlor.Editor;

	[CustomPropertyDrawer(typeof(WaveInfo))]
	public sealed class WaveInfoEditor : PropertyDrawer
	{
		protected override void Update(Rect rect, SerializedProperty property, GUIContent label)
		{
			var actionProp = property.PropertyOrThrow("m_Action");
			var color = GUI.color;
			GUI.backgroundColor = GetBackgroundColor(actionProp);
			rect.StretchHeight();
			property.isExpanded = EditorGUI.Foldout(
				rect,
				property.isExpanded,
				GetLabel(property),
				toggleOnLabelClick: true);
			GUI.backgroundColor = color;
			if (property.isExpanded)
			{
				++EditorGUI.indentLevel;
				rect.NewLine();
				EditorGUI.PropertyField(rect, actionProp);
				switch ((WaveAction)actionProp.enumValueIndex)
				{
					case WaveAction.Spawn:
						rect.NewLine();
						var spawnSchemeProp = property.PropertyOrThrow("m_SpawnScheme");
						EditorGUI.PropertyField(rect, spawnSchemeProp);
						rect.NewLine();
						var spawnCountProp = property.PropertyOrThrow("m_SpawnCount");
						EditorGUI.PropertyField(rect, spawnCountProp);
						break;
					case WaveAction.WaitForTime:
						var waitDurationProp = property.PropertyOrThrow("m_WaitDuration");
						rect.NewLine();
						EditorGUI.PropertyField(rect, waitDurationProp);
						break;
					case WaveAction.WaitForClear:
					default:
						break;
				}
				--EditorGUI.indentLevel;
			}
		}
		protected override float GetHeight(SerializedProperty property, GUIContent label)
		{
			var ret = EditorGUIUtility.singleLineHeight;
			if (property.isExpanded)
			{
				var actionProp = property.PropertyOrThrow("m_Action");
				ret += EditorGUIUtility.singleLineHeight;
				switch ((WaveAction)actionProp.enumValueIndex)
				{
					case WaveAction.Spawn:
						ret += EditorGUIUtility.singleLineHeight * 2;
						break;
					case WaveAction.WaitForTime:
						ret += EditorGUIUtility.singleLineHeight;
						break;
					case WaveAction.WaitForClear:
						break;
				}
			}
			return ret;
		}
		private Color GetBackgroundColor(SerializedProperty actionProp)
		{
			return (WaveAction)actionProp.enumValueIndex switch
			{
				WaveAction.Spawn => Color.red,
				WaveAction.WaitForTime => Color.green,
				WaveAction.WaitForClear => Color.blue,
				_ => Color.white,
			};
		}
		private GUIContent GetLabel(SerializedProperty property)
		{
			var actionProp = property.PropertyOrThrow("m_Action");
			switch ((WaveAction)actionProp.enumValueIndex)
			{
				case WaveAction.Spawn:
					var spawnSchemeProp = property.PropertyOrThrow("m_SpawnScheme");
					if (spawnSchemeProp.objectReferenceValue == null)
					{
						return EditorHelper.Label("NULL");
					}
					var spawnCountProp = property.PropertyOrThrow("m_SpawnCount");
					return EditorHelper.Label($"Spawn {spawnSchemeProp.objectReferenceValue.name} {spawnCountProp.intValue}");
				case WaveAction.WaitForTime:
					var waitDurationProp = property.PropertyOrThrow("m_WaitDuration");
					return EditorHelper.Label($"Wait for {waitDurationProp.floatValue}s");
				case WaveAction.WaitForClear:
					return EditorHelper.Label("Wait for clear");
				default:
					return GUIContent.none;
			}
		}
	}
}