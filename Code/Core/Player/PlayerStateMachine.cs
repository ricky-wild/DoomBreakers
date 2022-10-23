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
		IsSprinting = 33

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

		//public void UpdatePlayerStateBehaviours()
		//{
		//	switch(_state)
		//	{
		//		case state.IsIdle:
		//			//I need a reference to the player, may as well be done in the player class right? I need the vars from there.
		//			break;
		//	}
		//} 

		//https://faramira.com/implementing-a-finite-state-machine-using-c-in-unity-part-1/
	}
}


