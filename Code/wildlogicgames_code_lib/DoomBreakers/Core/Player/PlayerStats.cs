//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
	public class PlayerStats : CharacterStat, IPlayerStats //
	{
		private HealingItemType _mostRecentHealType;
		private int _killCount;

		private bool _process;
		public bool Process() => _process;
		public void Disable() => _process = false;
		public PlayerStats(double h,double s, double d) : base (health:h, stamina:s, defence:d)
		{
			_health = h;
			_stamina = s;
			_defence = d;
			_process = true;
			_killCount = 0;
			_mostRecentHealType = HealingItemType.None;
		}
		public override bool IsArmored() => _armored;
		public override void IsArmored(bool b) => base.IsArmored(b);

		public int GetKillCount() => _killCount;
		public void IncrementKillCount(int value) => _killCount += value;

		public double GetMaxHealthLimit() => _maxHealth;

		public void SetRecentHealItemType(HealingItemType recentHealType) => _mostRecentHealType = recentHealType;
		public HealingItemType GetRecentHealItemType() => _mostRecentHealType;

	}
}


