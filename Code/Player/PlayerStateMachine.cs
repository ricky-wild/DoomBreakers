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
		IsHitByReleaseAttack = 15,
		IsHitByQuickAttack = 16,
		IsHitWhileDefending = 17,
		IsQuickHitWhileDefending = 18,
		IsHitByEnemyReleaseAttack = 19,
		IsSlamOnGroundByEnemy = 20,
		IsExhausted = 21,
		IsDodgePrepare = 22,
		IsDodgeRelease = 23,
		IsSlamOnGround = 24,
		IsLockedComboAttack = 25,
		IsGainedEquipment = 26,
		IsArmorBroken = 27,
		IsRespawning = 28,
		IsDialogue = 29

	};

	public class PlayerStateMachine : MonoBehaviour, IPlayerStateMachine//: MonoBehaviour
	{

		private state _state;

		public PlayerStateMachine(state state)
		{
			_state = state;
		}		
		public void SetPlayerState(state state)
		{
			print("\nPlayerStateMachine.cs= state being set too, "+ state);
			_state = state;
		}
		public state GetPlayerState()
		{
			return _state;
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


