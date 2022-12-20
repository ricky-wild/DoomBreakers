//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
	public class PlayerStats : CharacterStat, IPlayerStats //
	{
		private HealingItemType _mostRecentHealType;
		private int _killCount;
		private ITimer _buttonHeldTimer, _staminaTimer;

		private bool _process;
		public bool Process() => _process;
		public void Disable() => _process = false;
		public PlayerStats(double h,double s, double d) : base (health:h, stamina:s, defence:d)
		{
			_health = h;
			_stamina = s;
			_defence = d;
			_process = true;
			_killCount = 0;
			_mostRecentHealType = HealingItemType.None;
			_staminaTimer = new Timer();
			_staminaTimer.StartTimer(0.05f); //increment stamina every 20th of a sec.
		}

		public void UpdateStatus(ref PlayerStateMachine playerStateMachine, ref Transform transform, ref PlayerAnimator playerAnimator, 
			ref IPlayerEquipment playerEquipment, ref PlayerStats playerStats,ref Animator indicatorAnimator, ref Vector3 velocity, 
			int playerId, bool setDeath)
		{
			if (!Process()) return;
			UpdateHealth(ref playerStateMachine, ref playerStats, ref playerAnimator, ref velocity, playerId, setDeath);
			UpdateStamina(ref playerStateMachine, ref transform, ref playerStats, ref playerAnimator, ref velocity, playerId);
			UpdateDefense(ref playerStateMachine, ref transform, ref velocity, ref playerAnimator, ref playerEquipment);
		}
		private void UpdateHealth(ref PlayerStateMachine playerStateMachine, ref PlayerStats playerStats,
			ref PlayerAnimator playerAnimator, ref Vector3 velocity, int playerId, bool setDeath)
		{
			if (Health <= 0f)
			{
				if (setDeath)
				{
					AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerDeathSFX);
					playerAnimator.PlayIndicatorAnimation(IndicatorAnimID.Dead);
					playerStateMachine.SetState(new PlayerDying(playerStateMachine, velocity));
					UIPlayerManager.TriggerEvent("ReportUIPlayerStatEvent", ref playerStats, playerId);
					Disable();
				}
			}
		}
		private void UpdateStamina(ref PlayerStateMachine playerStateMachine, ref Transform transform, ref PlayerStats playerStats, 
			ref PlayerAnimator playerAnimator, ref Vector3 velocity, int playerId)
		{
			if (_staminaTimer.HasTimerFinished())
			{
				Stamina += 0.008; //magic numbers are bad.
				UIPlayerManager.TriggerEvent("ReportUIPlayerStatEvent", ref playerStats, playerId);
				_staminaTimer.StartTimer(0.05f);
			}
			if (Stamina <= 0f)
			{
				//if (SafeToSetTired())
				playerAnimator.PlayIndicatorAnimation(IndicatorAnimID.Tired);
				playerStateMachine.SetState(new PlayerExhausted(playerStateMachine, velocity, transform));
			}
		}
		private void UpdateDefense(ref PlayerStateMachine playerStateMachine, ref Transform transform, ref Vector3 velocity, 
			ref PlayerAnimator playerAnimator, ref IPlayerEquipment playerEquipment)
		{
			if (Defence <= 0 && IsArmored())
			{
				AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerArmorBrokenSFX);
				playerStateMachine.SetState(new PlayerBrokenEquipment(playerStateMachine, velocity, transform));
				IsArmored(false);
				playerEquipment.RemoveArmor();
				playerAnimator.SetAnimatorController(ref playerEquipment);
			}
		}



		public override bool IsArmored() => _armored;
		public override void IsArmored(bool b) => base.IsArmored(b);

		public int GetKillCount() => _killCount;
		public void IncrementKillCount(int value) => _killCount += value;

		public double GetMaxHealthLimit() => _maxHealth;

		public void SetRecentHealItemType(HealingItemType recentHealType) => _mostRecentHealType = recentHealType;
		public HealingItemType GetRecentHealItemType() => _mostRecentHealType;

	}
}


