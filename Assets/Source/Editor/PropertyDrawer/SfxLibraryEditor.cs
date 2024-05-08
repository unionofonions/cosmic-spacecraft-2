
namespace Parlor.Game.Editor
{
	using System.Linq;
	using UnityEditor;
	using Parlor.Editor;

	[CustomPropertyDrawer(typeof(SfxInfo))]
	public sealed class SfxInfoEditor : RecordInfoEditor
	{
		protected override bool Valid(SerializedProperty property)
		{
			var collectionProp = property
				.PropertyOrThrow("m_Bank")
				.PropertyOrThrow("m_Collection");
			if (collectionProp.arraySize == 0) return false;
			return collectionProp
				.Hierarchy()
				.Select(elem => elem.PropertyOrThrow("m_Value").objectReferenceValue)
				.All(elem => elem != null);
		}
	}
}