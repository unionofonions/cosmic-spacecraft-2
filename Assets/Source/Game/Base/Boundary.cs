
namespace Parlor.Game
{
	using UnityEngine;

	[DisallowMultipleComponent]
	[RequireComponent(typeof(Collider2D))]
	public sealed class Boundary : MonoBehaviour
	{
		private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.TryGetComponent<Bullet>(out var bullet))
			{
				bullet.gameObject.SetActive(false);
			}
		}
	}
}