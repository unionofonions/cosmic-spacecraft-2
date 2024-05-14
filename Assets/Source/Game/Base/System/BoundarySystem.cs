using UnityEngine;

namespace Parlor.Game
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Collider2D))]
	public sealed class BoundarySystem : MonoBehaviour
	{
		[SerializeField]
		private Vector3 m_Center;
		[SerializeField, MinEpsilon]
		private float m_Radius;
		[SerializeField]
		private Bank<Vector3> m_SpawnPoints;

		public Vector3 Center
		{
			get => m_Center;
		}
		public float Radius
		{
			get => m_Radius;
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.TryGetComponent<IDestroyOnExitBoundary>(out _))
			{
				collision.gameObject.SetActive(false);
			}
		}
		public Vector3 GetSpawnPoint()
		{
			return m_SpawnPoints.Provide();
		}
	}
	public interface IDestroyOnExitBoundary { }
}