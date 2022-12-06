
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


		protected double _health;
		protected double _stamina;
		protected double _defence;

		protected bool _armored;

		public CharacterStat(double health, double stamina, double defence)
		{
			_health = health;
			_stamina = stamina;
			_defence = defence;
			_armored = false;
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

		public virtual bool IsArmored() => _armored;
		public virtual void IsArmored(bool b)
		{
			_armored = b;
			if (b) _defence = _maxDefence;
			else _defence = _minDefence;
		}

		public virtual void Update() { }

	}
}


