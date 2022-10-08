//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

namespace DoomBreakers
{
	public class PlayerStats : IPlayerStats //: MonoBehaviour 
	{
		private const double _maxHealth = 1.0;
		private const double _minHealth = 0;
		private const double _maxStamina = 1.0;
		private const double _minStamina = 0;
		private const double _maxDefence = 1.0;
		private const double _minDefence = 0;


		private double _health;
		private double _stamina;
		private double _defence;

		public PlayerStats(double health, double stamina, double defence)
		{
			_health = health;
			_stamina = stamina;
			_defence = defence;
		}
		public double Health
		{
			set
			{
				_health = value;
				if (_health > _maxHealth)
					_health = _maxHealth;
				if (_health < _minHealth)
					_health = _minHealth;
			}
			get
			{
				return _health;
			}
		}

		public double Stamina
		{
			set
			{
				_stamina = value;
				if (_stamina > _maxStamina)
					_stamina = _maxStamina;
				if (_stamina < _minStamina)
					_stamina = _minStamina;
			}
			get
			{
				return _stamina;
			}
		}

		public double Defence
		{
			set
			{
				_defence = value;
				if (_defence > _maxDefence)
					_defence = _maxDefence;
				if (_defence < _minDefence)
					_defence = _minDefence;
			}
			get
			{
				return _defence;
			}
		}


	}
}


