
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
		private int m_ActiveEnemyCount;
		private WaitUntil m_WaitForClear;

		private void Awake()
		{
			m_WaitForClear = new WaitUntil(() => m_ActiveEnemyCount == 0);
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
			UnloadSystems();
			ResetSystems();
			Domain.GetGameSystem().ResumeGame();
			StopAllCoroutines();
			StartCoroutine(SpawnAsync());
			OnBeginSpawn?.Invoke();
		}
		public void UnloadSystems()
		{
			Domain.GetPlayer().gameObject.SetActive(false);
			Domain.GetCameraSystem().StopShake();
			ActorProvider.ReturnAll();
			BulletProvider.ReturnAll();
			AfxHandler.StopAll();
		}
		private void ResetSystems()
		{
			Domain.GetPlayer().Respawn();
			m_ActiveEnemyCount = 0;
		}
		private IEnumerator SpawnAsync()
		{
			yield return null;
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
					OnBeginLevel?.Invoke(m_Levels[^1]);
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
					return m_WaitForClear;
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