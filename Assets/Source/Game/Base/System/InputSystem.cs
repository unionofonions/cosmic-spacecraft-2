
namespace Parlor.Game
{
	using UnityEngine;

	public sealed class InputSystem : MonoBehaviour
	{
		public float Axis(string code)
		{
			switch (code)
			{
				case "hor":
					if (Input.GetKey(KeyCode.A)) return -1f;
					if (Input.GetKey(KeyCode.D)) return +1f;
					return 0f;
				case "ver":
					if (Input.GetKey(KeyCode.S)) return -1f;
					if (Input.GetKey(KeyCode.W)) return +1f;
					return 0f;
				default:
					return 0f;
			}
		}
		public bool Hold(string code)
		{
			switch (code)
			{
				case "fir":
					return Input.GetKey(KeyCode.Space);
				default:
					return false;
			}
		}
	}
}