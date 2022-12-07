//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
	public class PlayerStats : CharacterStat, IPlayerStats //
	{
		private bool _process;
		public bool Process() => _process;
		public void Disable() => _process = false;
		public PlayerStats(double h,double s, double d) : base (health:h, stamina:s, defence:d)
		{
			_health = h;
			_stamina = s;
			_defence = d;
			_process = true;
		}
		public override bool IsArmored() => _armored;
		public override void IsArmored(bool b) => base.IsArmored(b);

	}
}


