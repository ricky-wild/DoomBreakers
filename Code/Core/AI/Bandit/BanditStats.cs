//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
	public class BanditStats : CharacterStat//, IPlayerStats 
	{
		private Transform[] _healthDisplayTransform; //index 0 & 1 are the heart fill (0 black, 1 red), 2 in the outer rim that holds these.
		private Vector3 _displayScale;
		private bool _process;

		public override double Health 
		{ 
			get => base.Health; 
			set
			{
				//print("\nBandit Health=" + value);
				SetFillBar(value);
				base.Health = value;

				//if (base.Health <= 0) _process = false;
			}
		}
		public bool Process() => _process;
		public void Disable() => _process = false;

		private void SetFillBar(double fillAmount)
		{
			DisplayFillBar(true);
			//_displayTimer.StartTimer(1.0f);

			// Make sure value is between 0 and 1
			fillAmount = Mathf.Clamp01((float)fillAmount);

			// Scale the fillImage accordingly
			var newScale = _healthDisplayTransform[1].localScale;
			newScale.x = _healthDisplayTransform[0].localScale.x * (float)fillAmount;
			newScale.y = _healthDisplayTransform[0].localScale.y * (float)fillAmount;
			_healthDisplayTransform[1].localScale = newScale;

			
		}
		public void DisplayFillBar(bool display)
		{
			if(display) _healthDisplayTransform[2].localScale = _displayScale; 
			if(!display) _healthDisplayTransform[2].localScale = Vector3.zero;
		}
		public BanditStats(ref Transform[] healthTransforms, double h, double s, double d) : base(health: h, stamina: s, defence: d)
		{
			_healthDisplayTransform = healthTransforms;
			_displayScale = new Vector3();
			_displayScale = _healthDisplayTransform[2].localScale;

			_health = h;
			_stamina = s;
			_defence = d;

			_process = true;
			//_displayTimer = new Timer();
			//_displayTimer = gameObjectToAttach.AddComponent<Timer>();//Cannot pass by ref or out.
			DisplayFillBar(false);
		}
		public override bool IsArmored() => _armored;
		public override void IsArmored(bool b) => base.IsArmored(b);

		//public override void pdate()
		//{
		//	base.Update();

			//if (_displayTimer.HasTimerFinished())
			//	DisplayFillBar(false);
		//}
	}
}


