using System.Collections;
using System.Collections.Generic;
//using UnityEngine;
namespace DoomBreakers
{
	public class DamageCalculator //: IWeaponDamage //: MonoBehaviour
	{
		public double CalculateDamage(ItemBase weapon, PlayerStats playerStat)
		{
			double totalDamage = 0.0;
			double weaponDamage = 0.0;
			double playerDefence = 0.0;

			//weaponDamage = weapon.RetrieveDamage();
			playerDefence = playerStat.Defence;

			totalDamage = weaponDamage / (weaponDamage + playerDefence);

			return totalDamage;
		}
	}
}

