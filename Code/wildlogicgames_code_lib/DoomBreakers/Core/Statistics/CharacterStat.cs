
using UnityEngine;

namespace DoomBreakers
{
	public class CharacterStat : MonoBehaviour//, IPlayerStats //
	{
		protected const double _maxHealth = 1.0;
		protected const double _minHealth = 0;
		protected const double _maxStamina = 1.0;
		protected const double _minStamina = 0;
		protected const double _maxDefence = 1.0;
		protected const double _minDefence = 0;
		protected const double _maxBleeding = 1.0;
		protected const double _minBleeding = 0;
		protected const double _maxBludgeoning = 1.0;
		protected const double _minBludgeoning = 0;


		protected double _health;
		protected double _stamina;
		protected double _defence;
		protected double _bleeding;
		protected double _bludgeoning;

		protected int _currency;

		protected double _currentWeaponDamage;
		protected const double _baseQuickAttackDamage = 0.002;
		protected const double _basePowerAttackDamage = 0.01;
		protected const double _baseKnockAttackDamage = 0.004;
		protected const double _baseUpwardAttackDamage = 0.003;

		protected bool _armored;
		protected bool _isBleeding;
		protected bool _isBludgeoning;

		protected ITimer _bleedTimer;
		protected float _bleedDamageTimeIncrement;

		protected ITimer _bludgeonTimer;
		protected float _bludgeonDamageTimeIncrement;

		public CharacterStat(double health, double stamina, double defence)
		{
			_health = health;
			_stamina = stamina;
			_defence = defence;
			_bleeding = _minBleeding;
			_armored = false;
			_isBleeding = false;
			_currency = 0;
			_bleedTimer = new Timer();
			_bleedDamageTimeIncrement = 0.44f;
			_bludgeonTimer = new Timer();
			_bludgeonDamageTimeIncrement = 2.0f;
		}
		public virtual double Health
		{
			set
			{
				_health = value;
				if (_health > _maxHealth)
					_health = _maxHealth;
				if (_health < _minHealth)
					_health = _minHealth;
				//print("\nHEALTH=" + _health);
			}
			get
			{
				return _health;
			}
		}
		public virtual double Stamina
		{
			set
			{
				_stamina = value;
				if (_stamina > _maxStamina)
					_stamina = _maxStamina;
				if (_stamina < _minStamina)
					_stamina = _minStamina;
				//print("\nSTAMINA=" + _stamina);
			}
			get
			{
				return _stamina;
			}
		}
		public virtual double Defence
		{
			set
			{
				_defence = value;
				if (_defence > _maxDefence)
					_defence = _maxDefence;
				if (_defence < _minDefence)
					_defence = _minDefence;

				//if (_defence <= 0)
				//	_armored = false;
				//print("\nDEFENSE=" + _defence);
			}
			get
			{
				return _defence;
			}
		}
		public virtual double Bleeding
		{
			set
			{
				_bleeding = value;
				if (_bleeding < _minBleeding)
					_bleeding = _minBleeding;
				if (_bleeding > _maxBleeding)
					_bleeding = _maxBleeding;
			}
			get
			{
				return _bleeding;
			}
		}
		public virtual double Bludgeoning
		{
			set
			{
				_bludgeoning = value;
				if (_bludgeoning < _minBludgeoning)
					_bludgeoning = _minBludgeoning;
				if (_bludgeoning > _maxBludgeoning)
					_bludgeoning = _maxBludgeoning;
			}
			get
			{
				return _bludgeoning;
			}
		}


		public virtual double WeaponDamage
		{
			set => _currentWeaponDamage = value;
			get => _currentWeaponDamage;
		}

		public virtual double QuickAttackDamage => _baseQuickAttackDamage;
		public virtual double PowerAttackDamage => _basePowerAttackDamage;
		public virtual double KnockAttackDamage => _baseKnockAttackDamage;
		public virtual double UpwardAttackDamage => _baseUpwardAttackDamage;



		public virtual int Currency
		{
			set => _currency = value;
			get => _currency;
		}

		public virtual bool IsArmored() => _armored;
		public virtual void IsArmored(bool b)
		{
			_armored = b;
			if (b) _defence = _maxDefence;
			else _defence = _minDefence;
		}

		public virtual bool IsBleeding() => _isBleeding;
		public virtual void IsBleeding(bool b)
		{
			_isBleeding = b;
			if (b) //_bleeding = _maxBleeding;
			{
				_bleeding = _maxBleeding;
				_bleedTimer.Reset();
			}
			else _bleeding = _minBleeding;
		}
		public virtual void UpdateBleedingDamage()
		{
			if (!_isBleeding) return;
			if (_bleedTimer.HasTimerFinished())
			{
				Health -= Health / 100;//250;// 500;// 1000;
				_bleedTimer.Reset();
				_bleedTimer.StartTimer(_bleedDamageTimeIncrement);
			}
		}


		public virtual bool IsBludgeoning() => _isBludgeoning;
		public virtual void IsBludgeoning(bool b)
		{
			_isBludgeoning = b;
			if (b) 
			{
				_bludgeoning = _maxBludgeoning;
				_bludgeonTimer.Reset();
			}
			else _bludgeoning = _minBludgeoning;
		}
		public virtual void UpdateBludgeoningDamage()
		{
			if (!_isBludgeoning) return;
			if (_bludgeonTimer.HasTimerFinished())
			{
				Health -= Health / 500;//250;// 500;// 1000;
				_bludgeonTimer.Reset();
				_bludgeonTimer.StartTimer(_bludgeonDamageTimeIncrement);
			}
		}

		//public virtual void Update() => UpdateBleedingDamage();

	}
}


