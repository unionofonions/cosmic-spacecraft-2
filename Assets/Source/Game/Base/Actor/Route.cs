
namespace Parlor.Game
{
	using System;
	using System.Linq;
	using UnityEngine;
	using Parlor.Diagnostics;

	[Serializable, EditorHandled]
	public sealed class Route
	{
		[SerializeField, NotDefault]
		private Transform[] m_Destinations;
		private Vector3[] m_DestinationValues;
		private int m_CurrentDestinationIndex;

		private Vector3[] DestinationValues
		{
			get
			{
				return m_DestinationValues ??= m_Destinations
					.Where(x => x != null)
					.Select(x => x.position)
					.ToArray();
			}
		}

		public Vector3 MoveTowardsDestination(Vector3 currentPosition, float maxDistanceDelta)
		{
			const float epsilon = 0.1f;
			if (DestinationValues.Length == 0) return currentPosition;
			var ret = Vector3.MoveTowards(
				currentPosition,
				DestinationValues[m_CurrentDestinationIndex],
				maxDistanceDelta);
			if (Vector3.Distance(ret, DestinationValues[m_CurrentDestinationIndex]) < epsilon)
			{
				m_CurrentDestinationIndex = (m_CurrentDestinationIndex + 1) % DestinationValues.Length;
			}
			return ret;
		}
		public void Reset()
		{
			m_CurrentDestinationIndex = 0;
		}

	}
}