
namespace Parlor.Game
{
	using System;
	using UnityEngine;
	using Parlor.Diagnostics;

	[CreateAssetMenu(menuName = "Parlor/Game/LevelInfo")]
	public sealed class LevelInfo : ScriptableObject
	{
		[SerializeField]
		private string m_LevelName;
		[SerializeField]
		private WaveInfo[] m_Waves;

		public string LevelName
		{
			get => m_LevelName;
		}
		public WaveInfo[] Waves
		{
			get => m_Waves;
		}
	}
	[Serializable, EditorHandled]
	public sealed class WaveInfo
	{
		[SerializeField]
		private WaveAction m_Action;
		[SerializeField, Unsigned]
		private float m_WaitDuration;
		[SerializeField, NotDefault]
		private Actor m_SpawnScheme;
		[SerializeField, MinOne]
		private int m_SpawnCount;

		public WaveAction Action
		{
			get => m_Action;
		}
		public float WaitDuration
		{
			get => m_WaitDuration;
		}
		public Actor SpawnScheme
		{
			get => m_SpawnScheme;
		}
		public int SpawnCount
		{
			get => m_SpawnCount;
		}
	}
	public enum WaveAction
	{
		Spawn,
		WaitForTime,
		WaitForClear
	}
}