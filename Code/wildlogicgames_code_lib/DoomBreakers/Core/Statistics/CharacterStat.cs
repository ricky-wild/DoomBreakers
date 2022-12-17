
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


		protected double _health;
		protected double _stamina;
		protected double _defence;
		protected double _bleeding;

		protected int _currency;

		protected bool _armored;
		protected bool _isBleeding;

		private ITimer _bleedTimer;
		private float _bleedDamageTimeIncrement;



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

		//public virtual void Update() => UpdateBleedingDamage();

	}
}


