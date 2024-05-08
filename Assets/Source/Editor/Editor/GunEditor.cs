
namespace Parlor.Game.Editor
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(Gun), editorForChildClasses: true)]
	public class GunEditor : Editor
	{
		private const float c_LineLength = 4f;

		protected void OnSceneGUI()
		{
			base.OnInspectorGUI();
			var muzzles = ((Gun)serializedObject.targetObject).Muzzles;
			if (muzzles == null) return;
			foreach (var elem in muzzles)
			{
				if (elem == null) continue;
				var reg = Handles.color;
				Handles.color = Color.red;
				Handles.DrawAAPolyLine(elem.position, elem.position + elem.up * c_LineLength);
				Handles.color = reg;
			}
		}
	}
}