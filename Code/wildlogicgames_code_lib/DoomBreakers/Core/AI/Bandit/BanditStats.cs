//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
	public class BanditStats : CharacterStat//, IPlayerStats 
	{
		private Transform[] _healthDisplayTransform; //index 0 & 1 are the heart fill (0 black, 1 red), 2 in the outer rim that holds these.
		private Transform[] _bleedDisplayTransform; //index 0 & 1 are the bleed fill (0 black, 1 red), 2 in the outer rim that holds these.
		private Vector3 _displayScale;
		private bool _process;

		private ITimer _bleedTimer;
		private float _bleedDamageTimeIncrement;

		public override double Health 
		{ 
			get => base.Health; 
			set
			{
				SetHealthFillBar(value);
				base.Health = value;
			}
		}
		public override double Bleeding
		{
			get => base.Bleeding;
			set
			{
				SetBleedFillBar(value);
				base.Bleeding = value;
				//Health -= Health / 1000; //Moved to Main Update() loop.
			}
		}
		public bool Process() => _process;
		public void Disable() => _process = false;

		private void SetHealthFillBar(double fillAmount)
		{
			DisplayHealthFillBar(true);
			fillAmount = Mathf.Clamp01((float)fillAmount);

			//Scale the fillImage accordingly.
			var newScale = _healthDisplayTransform[1].localScale;
			newScale.x = _healthDisplayTransform[0].localScale.x * (float)fillAmount;
			newScale.y = _healthDisplayTransform[0].localScale.y * (float)fillAmount;
			_healthDisplayTransform[1].localScale = newScale;		
		}
		public void DisplayHealthFillBar(bool display)
		{
			if(display) _healthDisplayTransform[2].localScale = _displayScale; 
			if(!display) _healthDisplayTransform[2].localScale = Vector3.zero;
		}
		private void SetBleedFillBar(double fillAmount)
		{
			DisplayBleedFillBar(true);
			fillAmount = Mathf.Clamp01((float)fillAmount);

			//Scale the fillImage accordingly.
			var newScale = _bleedDisplayTransform[1].localScale;
			newScale.x = _bleedDisplayTransform[0].localScale.x * (float)fillAmount;
			newScale.y = _bleedDisplayTransform[0].localScale.y * (float)fillAmount;
			_bleedDisplayTransform[1].localScale = newScale;
		}
		public void DisplayBleedFillBar(bool display)
		{
			if (display) _bleedDisplayTransform[2].localScale = _displayScale; 
			if (!display) _bleedDisplayTransform[2].localScale = Vector3.zero;
		}
		public BanditStats(ref Transform[] healthTransforms, ref Transform[] bleedTransforms, double h, double s, double d) : base(health: h, stamina: s, defence: d)
		{
			_healthDisplayTransform = healthTransforms;
			_bleedDisplayTransform = bleedTransforms;
			_displayScale = new Vector3();
			_displayScale = _healthDisplayTransform[2].localScale;

			_health = h;
			_stamina = s;
			_defence = d;

			_process = true;
			_bleedTimer = new Timer();
			_bleedDamageTimeIncrement = 0.44f;
			DisplayHealthFillBar(false);
			DisplayBleedFillBar(false);
		}
		//public override void Update() //=> base.Update();
		//{
		//	base.Update();
		//	UpdateBleedingDamage();
		//}

		public override bool IsArmored() => _armored;
		public override void IsArmored(bool b) => base.IsArmored(b);

		public override bool IsBleeding() => base.IsBleeding();
		public override void IsBleeding(bool b) //=> base.IsBleeding(b);
		{
			base.IsBleeding(b);
			DisplayBleedFillBar(b);
			if(b) _bleedTimer.StartTimer(_bleedDamageTimeIncrement);
		}
		public override void UpdateBleedingDamage() //=> base.UpdateBleedingDamage();
		{
			base.UpdateBleedingDamage();
		}
	}
}


