
namespace Parlor.Game
{
	using UnityEngine;
	using UnityEngine.UI;

	[DisallowMultipleComponent]
	[RequireComponent(typeof(Image))]
	public class AutoScrollImage : MonoBehaviour
	{
		private int m_TimerHash;
		private Material m_Material;

		protected void Awake()
		{
			m_Material = GetComponent<Image>().material;
			m_TimerHash = Shader.PropertyToID("_UnscaledTime");
		}
		private void Update()
		{
			if (m_Material != null)
			{
				m_Material.SetFloat(m_TimerHash, Time.unscaledTime);
			}
		}
	}
}