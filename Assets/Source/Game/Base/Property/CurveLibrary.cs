
namespace Parlor.Game
{
	using System;
	using UnityEngine;

	[Serializable]
	public sealed class CurveInfo : RecordInfo
	{
		[SerializeField]
		private Curve m_Value;

		public Curve Value
		{
			get => m_Value;
		}
	}
	[Serializable]
	public sealed class CurveReference : RecordReference<CurveInfo> { }
	[CreateAssetMenu(menuName = "Parlor/Game/CurveLibrary")]
	public sealed class CurveLibrary : RecordLibrary<CurveInfo> { }
}