//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
	public class BanditStats : CharacterStat//, IPlayerStats 
	{
		private Transform[] _healthDisplayTransform; //index 0 & 1 are the heart fill (0 black, 1 red), 2 in the outer rim that holds these.
		private Transform[] _bleedDisplayTransform; //index 0 & 1 are the bleed fill (0 black, 1 red), 2 in the outer rim that holds these.
		private Transform[] _bludgeonDisplayTransform;
		private Vector3 _displayScale;
		private bool _process;
		private ITimer _healthDisplayTimer, _bleedingTimer, _bludgeoningTimer;

		private double _banditQuickAttackDamage = 0.008;
		private double _banditPowerAttackDamage = 0.01;

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
		public override double Bludgeoning
		{
			get => base.Bludgeoning;
			set
			{
				SetBludgeonFillBar(value);
				base.Bludgeoning = value;
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
		private void SetBludgeonFillBar(double fillAmount)
		{
			DisplayBludgeonFillBar(true);
			fillAmount = Mathf.Clamp01((float)fillAmount);

			//Scale the fillImage accordingly.
			var newScale = _bludgeonDisplayTransform[1].localScale;
			newScale.x = _bludgeonDisplayTransform[0].localScale.x * (float)fillAmount;
			newScale.y = _bludgeonDisplayTransform[0].localScale.y * (float)fillAmount;
			_bludgeonDisplayTransform[1].localScale = newScale;
		}
		public void DisplayBludgeonFillBar(bool display)
		{
			if (display) _bludgeonDisplayTransform[2].localScale = _displayScale;
			if (!display) _bludgeonDisplayTransform[2].localScale = Vector3.zero;
		}

		public BanditStats(double h, double s, double d) : base(health: h, stamina: s, defence: d)
		{
		}
		public BanditStats(ref Transform[] healthTransforms, ref Transform[] bleedTransforms, ref Transform[] bludgeonTransforms, double h, double s, double d) : base(health: h, stamina: s, defence: d)
		{
			
			_healthDisplayTransform = healthTransforms;
			_bleedDisplayTransform = bleedTransforms;
			_bludgeonDisplayTransform = bludgeonTransforms;
			_displayScale = new Vector3();
			_displayScale = _healthDisplayTransform[2].localScale;

			_health = h;
			_stamina = s;
			_defence = d;


			_process = true;
			_bleedTimer = new Timer();
			_bleedDamageTimeIncrement = 0.44f;
			_bludgeonTimer = new Timer();
			_bludgeonDamageTimeIncrement = 2.0f;

			_healthDisplayTimer = new Timer(); //this.gameObject.AddComponent<Timer>();
			_bleedingTimer = new Timer(); //this.gameObject.AddComponent<Timer>();
			_bludgeoningTimer = new Timer(); //this.gameObject.AddComponent<Timer>();

			DisplayHealthFillBar(false);
			DisplayBleedFillBar(false);
			DisplayBludgeonFillBar(false);
		}

		//public override void Update() //=> base.Update();
		public override double QuickAttackDamage => _baseQuickAttackDamage + _banditQuickAttackDamage;
		public override double PowerAttackDamage => _basePowerAttackDamage + _banditPowerAttackDamage;


		public void UpdateStatus(ref BanditStateMachine banditStateMachine, ref Vector3 velocity, int enemyId, bool setDeath)
		{
			if (!Process()) return;

			if (_healthDisplayTimer.HasTimerFinished()) DisplayHealthFillBar(false);


			if (Health <= 0f)
			{
				if (setDeath)//SafeToSetDying()
				{
					UIPlayerManager.TriggerEvent("ReportUIPlayerKillScoreEvent");
					banditStateMachine.SetState(new BanditDying(banditStateMachine, velocity, enemyId));
					DisplayHealthFillBar(false);
					DisplayBleedFillBar(false);
					DisplayBludgeonFillBar(false);
					Disable();
				}
				return;
			}
			if (IsBleeding())
			{
				UpdateBleedingDamage();
				if (_bleedingTimer.HasTimerFinished())
				{
					//_banditSprite.SetBehaviourTextureFlash(0.25f, Color.red);// _bleedingTimer.StartTimer(0.05f);
					if (Bleeding > 0.01f) Bleeding -= 0.01f;
					if (Bleeding < 0.01f) IsBleeding(false);

					_bleedingTimer.StartTimer(0.05f);
				}
			}
			if (IsBludgeoning())
			{
				UpdateBludgeoningDamage();
				if (_bludgeoningTimer.HasTimerFinished())
				{
					//_banditSprite.SetBehaviourTextureFlash(0.25f, Color.red);// _bleedingTimer.StartTimer(0.1f);
					if (Bludgeoning > 0.01f) Bludgeoning -= 0.0025f;
					if (Bludgeoning < 0.01f) IsBludgeoning(false);

					_bleedingTimer.StartTimer(0.1f);
				}
			}
		}
		public void SetHealthDisplayTimer(float time) => _healthDisplayTimer.StartTimer(time);


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


		public override bool IsBludgeoning() => base.IsBludgeoning();
		public override void IsBludgeoning(bool b) //=> base.IsBleeding(b);
		{
			base.IsBludgeoning(b);
			DisplayBludgeonFillBar(b);
			if (b) _bludgeonTimer.StartTimer(_bludgeonDamageTimeIncrement);
		}
		public override void UpdateBludgeoningDamage() //=> base.UpdateBleedingDamage();
		{
			base.UpdateBludgeoningDamage();
		}
	}
}


