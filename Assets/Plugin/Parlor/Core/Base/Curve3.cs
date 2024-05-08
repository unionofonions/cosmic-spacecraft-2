#nullable enable

namespace Parlor
{
	using System;
	using UnityEngine;
	using Parlor.Diagnostics;

	[Serializable, EditorHandled]
	public class Curve3
	{
		public Curve X
		{
			get => m_X;
		}
		public Curve Y
		{
			get => m_Y;
		}
		public Curve Z
		{
			get => m_Z;
		}
		public bool Valid
		{
			get => (m_Valid & 7) != 0;
		}

		public Vector3 Evaluate(float x)
		{
			var ret = Vector3.zero;
			if ((m_Valid & 1) != 0) { ret.x = m_X.Evaluate(x); }
			if ((m_Valid & 2) != 0) { ret.y = m_Y.Evaluate(x); }
			if ((m_Valid & 4) != 0) { ret.z = m_Z.Evaluate(x); }
			return ret;
		}

		#region Internal
#nullable disable
		[SerializeField]
		private Curve m_X;
		[SerializeField]
		private Curve m_Y;
		[SerializeField]
		private Curve m_Z;
		[SerializeField, ReadOnly]
		private int m_Valid;
#nullable enable
		#endregion // Internal
	}
}