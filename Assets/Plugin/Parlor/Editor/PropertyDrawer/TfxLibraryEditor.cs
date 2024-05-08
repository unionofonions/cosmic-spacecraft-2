#nullable enable

namespace Parlor.Editor
{
	using UnityEditor;
	using Parlor.Runtime;

	[CustomPropertyDrawer(typeof(TfxInfo))]
	public class TfxInfoEditor : RecordInfoEditor
	{
		protected override bool Valid(SerializedProperty property)
		{
			return Valid("m_Position") || Valid("m_Rotation");
			bool Valid(string name)
			{
				return (property
					.PropertyOrThrow(name)
					.PropertyOrThrow("m_Valid")
					.intValue & 7) != 0;
			}
		}
	}
}