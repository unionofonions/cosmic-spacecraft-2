
namespace Parlor.Game
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "StatsSystem", menuName = "Parlor/Game/StatsSystem")]
	public sealed class StatsSystem : ScriptableObject
	{
		[SerializeField]
		private Curve m_ArmorFactorFunction;
		[SerializeField]
		private Curve m_ArmorPenetrationFactorFunction;

		static private StatsSystem s_Instance;

		static private StatsSystem Instance
		{
			get
			{
				if (s_Instance == null)
				{
					s_Instance = Resources.Load<StatsSystem>("Game/StatsSystem");
				}
				return s_Instance;
			}
		}

		static public float CalculateArmorFactor(float armor)
		{
			if (Instance == null) return 0f;
			return Instance.m_ArmorFactorFunction.Evaluate(armor);
		}
		static public float CalculateArmorPenetrationFactor(float armorPenetration)
		{
			if (Instance == null) return 0f;
			return Instance.m_ArmorPenetrationFactorFunction.Evaluate(armorPenetration);
		}
	}
}