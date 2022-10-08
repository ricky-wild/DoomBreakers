//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


namespace DoomBreakers
{
	public class Weapon : IWeapon //: MonoBehaviour
	{
		private double _damageOutput;
		public Weapon(double damageOutput)
		{
			_damageOutput = damageOutput;
		}
		public double RetrieveDamage()
		{
			return _damageOutput;
		}
	}
}

