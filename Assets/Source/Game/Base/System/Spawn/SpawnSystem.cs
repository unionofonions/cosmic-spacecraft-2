
namespace Parlor.Game
{
	using System;
	using System.Collections;
	using UnityEngine;
	using Parlor.Runtime;

	[DisallowMultipleComponent]
	public sealed class SpawnSystem : MonoBehaviour
	{
		public event Action OnBeginSpawn;
		public event Action<LevelInfo> OnBeginLevel;

		[SerializeField, NotDefault]
		private LevelInfo[] m_Levels;
		[SerializeField, ReadOnly]
		private int m_ActiveEnemyCount;

		private void Awake()
		{
			Actor.OnDeath += (Actor instigator, Actor victim) =>
			{
				if (victim is EnemyShip)
				{
					--m_ActiveEnemyCount;
				}
			};
		}
		public void BeginSpawn()
		{
			ResetSystems();
			StopAllCoroutines();
			StartCoroutine(SpawnAsync());
			OnBeginSpawn?.Invoke();
		}
		private void ResetSystems()
		{
			Domain.GetPlayer().Respawn();
			Domain.GetCameraSystem().StopShake();
			ActorProvider.ReturnAll();
			BulletProvider.ReturnAll();
			AfxHandler.StopAll();
			m_ActiveEnemyCount = 0;
		}
		private IEnumerator SpawnAsync()
		{
			for (var i = 0; i < m_Levels.Length - 1; ++i)
			{
				if (m_Levels[i] != null)
				{
					OnBeginLevel?.Invoke(m_Levels[i]);
					foreach (var wave in m_Levels[i].Waves)
					{
						yield return ProcessWave(wave);
					}
				}
			}
			if (m_Levels.Length != 0 && m_Levels[^1] != null)
			{
				while (true)
				{
					foreach (var wave in m_Levels[^1].Waves)
					{
						yield return ProcessWave(wave);
					}
				}
			}
		}
		private object ProcessWave(WaveInfo waveInfo)
		{
			switch (waveInfo.Action)
			{
				case WaveAction.Spawn:
					SpawnActor(waveInfo.SpawnScheme, waveInfo.SpawnCount);
					return null;
				case WaveAction.WaitForTime:
					return new WaitForSeconds(waveInfo.WaitDuration);
				case WaveAction.WaitForClear:
					return new WaitUntil(() => m_ActiveEnemyCount == 0);
				default:
					return null;
			}
		}
		private void SpawnActor(Actor scheme, int count)
		{
			if (scheme != null)
			{
				for (var i = 0; i < count; ++i)
				{
					ActorProvider.Provide(scheme)
						.Respawn();
				}
				if (scheme is EnemyShip)
				{
					m_ActiveEnemyCount += count;
				}
			}
		}
	}
}