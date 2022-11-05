using System;
using UnityEngine;
using System.Collections;

namespace DoomBreakers
{

	public enum state
	{
		IsDead = 0,
		IsDying = 1,
		IsIdle = 2,
		IsMoving = 3,
		IsJumping = 4,
		IsMidAirAttack = 5,
		IsAttackPrepare = 6,
		IsAttackRelease = 7,
		IsQuickAttack = 8,
		IsUpwardAttack = 9,
		IsRunningQuickAttack = 10,
		IsKnockBackAtkPrepare = 11,
		IsKnockBackAttack = 12,
		IsDefencePrepare = 13,
		IsDefenceRelease = 14,
		IsDefenceMoving = 15,
		IsHitByReleaseAttack = 16,
		IsHitByQuickAttack = 17,
		IsHitWhileDefending = 18,
		IsQuickHitWhileDefending = 19,
		IsHitByEnemyReleaseAttack = 20,
		IsSlamOnGroundByEnemy = 21,
		IsExhausted = 22,
		IsDodgeLPrepare = 23,
		IsDodgeRPrepare = 24,
		IsDodgeRelease = 25,
		IsSlamOnGround = 26,
		IsLockedComboAttack = 27,
		IsGainedEquipment = 28,
		IsArmorBroken = 29,
		IsRespawning = 30,
		IsDialogue = 31,
		IsFalling = 32,
		IsSprinting = 33,
		IsWaiting = 34

	};

	public class PlayerStateMachine : IPlayerStateMachine//: MonoBehaviour
	{

		private state _state;

		public PlayerStateMachine(state state)
		{
			_state = state;
		}		
		public void SetPlayerState(state state)
		{
			//print("\nPlayerStateMachine.cs= state being set too, "+ state);
			if (!SafeToSetState(state)) //Guard clause
				return;
			_state = state;
		}
		public state GetPlayerState()
		{
			return _state;
		}

		private bool SafeToSetState(state state)
		{
			if (_state == state) //Guard clause don't assign the same state.
				return false;

			if (_state == state.IsFalling && state == state.IsJumping) //Prevents delayed jumping behaviour upon jump button spam
				return false;
			if (_state == state.IsFalling && state == state.IsMoving) //Prevents delayed jumping behaviour upon jump button spam
				return false;


			return true;
		}



		public bool IsIdle()
		{
			if (_state == state.IsIdle)
				return true;
			return false;
		}
		public bool IsMoving()
		{
			if (_state == state.IsMoving)
				return true;
			return false;
		}
		public bool IsSprinting()
		{
			if (_state == state.IsSprinting)
				return true;
			return false;
		}
		public bool IsJumping()
		{
			if (_state == state.IsJumping)
				return true;
			return false;
		}
		public bool IsFalling()
		{
			if (_state == state.IsFalling)
				return true;
			return false;
		}
		public bool IsQuickAttack()
		{
			if (_state == state.IsQuickAttack)
				return true;
			return false;
		}
		public bool IsPowerAttackPrepare()
		{
			if (_state == state.IsAttackPrepare)
				return true;
			return false;
		}
		public bool IsPowerAttackRelease()
		{
			if (_state == state.IsAttackRelease)
				return true;
			return false;
		}
		public bool IsKnockbackAttackPrepare()
		{
			if (_state == state.IsKnockBackAtkPrepare)
				return true;
			return false;
		}
		public bool IsKnockbackAttackRelease()
		{
			if (_state == state.IsKnockBackAttack)
				return true;
			return false;
		}
		public bool IsUpwardAttack()
		{
			if (_state == state.IsUpwardAttack)
				return true;
			return false;
		}
		public bool IsJumpingAttack()
		{
			if (_state == state.IsMidAirAttack)
				return true;
			return false;
		}
		public bool IsMovingAttack()
		{
			if (_state == state.IsRunningQuickAttack)
				return true;
			return false;
		}
		public bool IsDefendingPrepare()
		{
			if (_state == state.IsDefencePrepare)
				return true;
			return false;
		}
		public bool IsDefendingRelease()
		{
			if (_state == state.IsDefenceRelease)
				return true;
			return false;
		}
		public bool IsDefendingMoving()
		{
			if (_state == state.IsDefenceMoving)
				return true;
			return false;
		}
		public bool IsDodgeLeftPrepare()
		{
			if (_state == state.IsDodgeLPrepare)
				return true;
			return false;
		}
		public bool IsDodgeRightPrepare()
		{
			if (_state == state.IsDodgeRPrepare)
				return true;
			return false;
		}
		public bool IsDodgeRelease()
		{
			if (_state == state.IsDodgeRelease)
				return true;
			return false;
		}
		public bool IsQuickHitWhenDefending()
		{
			if (_state == state.IsQuickHitWhileDefending)
				return true;
			return false;
		}
		public bool IsPowerHitWhenDefending()
		{
			if (_state == state.IsHitWhileDefending)
				return true;
			return false;
		}
		public bool IsQuickAttackHit()
		{
			if (_state == state.IsHitByQuickAttack)
				return true;
			return false;
		}
		public bool IsPowerAttackHit()
		{
			if (_state == state.IsHitByReleaseAttack)
				return true;
			return false;
		}
		public bool IsImpactHit()
		{
			if (_state == state.IsSlamOnGround)
				return true;
			return false;
		}
		public bool IsGainedEquipment()
		{
			if (_state == state.IsGainedEquipment)
				return true;
			return false;
		}
		public bool IsArmorDestroyed()
		{
			if (_state == state.IsArmorBroken)
				return true;
			return false;
		}
		public bool IsRespawning()
		{
			if (_state == state.IsRespawning)
				return true;
			return false;
		}
		public bool IsWaiting()
		{
			if (_state == state.IsWaiting)
				return true;
			return false;
		}
		public bool IsExhausted()
		{
			if (_state == state.IsExhausted)
				return true;
			return false;
		}
		public bool IsDying()
		{
			if (_state == state.IsDying)
				return true;
			return false;
		}
		public bool IsDead()
		{
			if (_state == state.IsDead)
				return true;
			return false;
		}
	}
}


