
namespace Parlor.Game
{
	using System;
	using UnityEngine;

	[Serializable]
	public struct Route
	{
		[SerializeField]
		private Vector3 m_StartPosition;
		[SerializeField]
		private Vector3[] m_Destinations;
		[SerializeField]
		private bool m_Loop;
		private RouteStatus m_Status;
		private Vector3 m_CurrentPosition;
		private Vector3 m_TargetPosition;
		private float m_Speed;
		private bool m_JustChangedDestination;
		private int m_i;

		public Vector3 StartPosition
		{
			get => m_StartPosition;
		}
		public Quaternion StartRotation
		{
			get
			{
				if (m_Destinations.Length == 0) return new(0f, 0f, 1f, 0f);
				var dir = m_Destinations[0] - m_StartPosition;
				var angle = MathHelper.Dir2Deg(dir) - 90f;
				return Quaternion.Euler(0f, 0f, angle);
			}
		}
		public RouteStatus Status
		{
			get => m_Status;
		}
		public bool JustChangedDestination
		{
			get => m_JustChangedDestination;
		}
		public Vector3 CurrentPosition
		{
			get => m_CurrentPosition;
		}
		public float Speed
		{
			get => m_Speed;
			set => m_Speed = Mathf.Max(value, 0f);
		}

		public void Restart()
		{
			if (m_Destinations.Length == 0)
			{
				m_Status = RouteStatus.Invalid;
				m_CurrentPosition = m_StartPosition;
			}
			else
			{
				m_Status = RouteStatus.MovingTowardsFirstDestination;
				m_CurrentPosition = m_StartPosition;
				m_TargetPosition = m_Destinations[0];
				m_i = 0;
			}
		}
		public void Update()
		{
			if (m_Status is RouteStatus.Invalid or RouteStatus.Finished)
			{
				return;
			}
			const float epsilon = 0.01f;
			m_CurrentPosition = Vector3.MoveTowards(
				m_CurrentPosition,
				m_TargetPosition,
				m_Speed * Time.fixedDeltaTime);
			if (Vector2.Distance(m_CurrentPosition, m_TargetPosition) <= epsilon)
			{
				switch (m_Status)
				{
					case RouteStatus.MovingTowardsFirstDestination:
						m_Status = RouteStatus.JustArrivedAtFirstDestination;
						break;
					case RouteStatus.JustArrivedAtFirstDestination:
						if (m_Destinations.Length == 1)
						{
							m_Status = RouteStatus.Finished;
						}
						else
						{
							m_Status = RouteStatus.Moving;
							m_i = 0;
							m_TargetPosition = m_Destinations[0];
						}
						break;
					case RouteStatus.Moving:
						if (m_i >= m_Destinations.Length - 1)
						{
							if (m_Loop)
							{
								m_i = 0;
								m_TargetPosition = m_Destinations[0];
							}
							else
							{
								m_Status = RouteStatus.Finished;
							}
						}
						else
						{
							m_TargetPosition = m_Destinations[++m_i];
						}
						break;
					case RouteStatus.Finished:
						break;
				}
				m_JustChangedDestination = true;
			}
			else
			{
				m_JustChangedDestination = false;
			}
		}
	}
	public enum RouteStatus
	{
		Invalid,
		MovingTowardsFirstDestination,
		JustArrivedAtFirstDestination,
		Moving,
		Finished,
	}
}