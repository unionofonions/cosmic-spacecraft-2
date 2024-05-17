
namespace Parlor.Game
{
	using UnityEngine;
	using UnityEngine.UI;

	[DisallowMultipleComponent]
	[RequireComponent(typeof(Image))]
	public class AutoScrollImage : MonoBehaviour
	{
		private int m_UnscaledTimeHash;
		private Material m_Material;

		protected void Awake()
		{
			m_Material = GetComponent<Image>().material;
			m_UnscaledTimeHash = Shader.PropertyToID("_UnscaledTime");
		}
		private void Update()
		{
			if (m_Material != null)
			{
				m_Material.SetFloat(m_UnscaledTimeHash, Time.unscaledTime);
			}
		}
	}
}