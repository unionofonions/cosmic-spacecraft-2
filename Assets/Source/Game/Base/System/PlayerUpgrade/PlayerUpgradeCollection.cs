
namespace Parlor.Game
{
	using UnityEngine;

	[CreateAssetMenu(menuName = "Parlor/Game/PlayerUpgradeCollection")]
	public sealed class PlayerUpgradeCollection : ScriptableObject
	{
		[SerializeField]
		private Bank<PlayerUpgrade> m_Bank;

		public PlayerUpgrade GetUpgrade()
		{
			return m_Bank.Provide();
		}
	}
}