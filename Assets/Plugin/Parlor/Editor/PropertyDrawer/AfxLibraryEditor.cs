#nullable enable

namespace Parlor.Editor
{
	using UnityEditor;
	using Parlor.Runtime;

	[CustomPropertyDrawer(typeof(AfxInfo))]
	public sealed class AfxInfoEditor : RecordInfoEditor
	{
		protected override bool Valid(SerializedProperty property)
		{
			var schemeProp = property.PropertyOrThrow("m_Scheme");
			return schemeProp.objectReferenceValue != null;
		}
	}
}