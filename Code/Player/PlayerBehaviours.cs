using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
	//PlayerBehaviours.cs obj also requires Controller2D.cs & RaycastController.cs & BoxCollder2D
	//[RequireComponent(typeof(Controller2D))]
	public class PlayerBehaviours : MonoBehaviour, IPlayerBehaviours
	{
		private Controller2D _controller2D; 
		private Vector3 _velocity;
		private Transform _transform;
		private float _targetVelocityX, _moveSpeed, _sprintSpeed;

		public PlayerBehaviours(Transform t, Controller2D controller2D)
		{
			_controller2D = controller2D;
			_transform = t;
			_moveSpeed = 3.75f;//3.5f;
			_sprintSpeed = 1.0f;
		}

		private void Initialize()
		{
			if (this.GetComponent<Controller2D>() != null)
				_controller2D = this.GetComponent<Controller2D>();
			else
				print("\nPlayerBehaviours.cs= _controller2D Not Found/Assigned!");

			_velocity = new Vector3();
			_targetVelocityX = 1.0f;
		}
		private void Awake()
		{
		}
		void Start()
		{
		}
		public void IdleProcess()
		{
			_velocity.x = 0f;
			_velocity.y = 0f;
		}
		public void JumpProcess()
		{

		}
		public void QuickAttackProcess()
		{

		}
		void Update()
		{
			//UpdateTransform(); Called In Player.cs to pass PlayerInput.cs through.
		}
		public bool SafeToMove(IPlayerStateMachine playerStateMachine)
		{
			if (playerStateMachine.GetPlayerState() == state.IsDying)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsDead)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsGainedEquipment)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsArmorBroken)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsAttackPrepare)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsAttackRelease)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsDodgePrepare)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsDodgeRelease)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsSlamOnGround)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsSlamOnGroundByEnemy)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsLockedComboAttack)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsRespawning)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsExhausted)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsDialogue)
				return false;

			return true;
		}
		public bool SafeToSetIdle(IPlayerStateMachine playerStateMachine)
		{
			if (playerStateMachine.GetPlayerState() == state.IsQuickAttack)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsUpwardAttack)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsRunningQuickAttack)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsQuickHitWhileDefending)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsHitWhileDefending)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsMidAirAttack)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsJumping)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsGainedEquipment)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsArmorBroken)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsHitByReleaseAttack)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsHitByEnemyReleaseAttack)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsHitByQuickAttack)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsExhausted)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsRespawning)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsDefencePrepare)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsLockedComboAttack)
				return false;
			if (playerStateMachine.GetPlayerState() == state.IsDialogue)
				return false;

			return true;
		}
		private void DetectMovement(IPlayerStateMachine playerStateMachine)
		{
			if (!SafeToSetIdle(playerStateMachine))//Guard Clause
				return;

			if (_targetVelocityX == 0f && _controller2D.collisions.below)
			{
				playerStateMachine.SetPlayerState(state.IsIdle);
			}
			if (_targetVelocityX > 0f || _targetVelocityX < 0f && _controller2D.collisions.below)
			{
				playerStateMachine.SetPlayerState(state.IsMoving);
			}
		}
		public void UpdateMovement(Vector2 input, IPlayerStateMachine playerStateMachine)
		{
			if (!SafeToMove(playerStateMachine))//Guard Clause
			{
				input = Vector2.zero;
				UpdateTransform(input);
				return;
			}
			_targetVelocityX = (input.x * (_moveSpeed * _sprintSpeed));
			_velocity.x = _targetVelocityX;
			
			DetectMovement(playerStateMachine);

			UpdateTransform(input);
		}
		private void UpdateTransform(Vector2 input)
		{
			_controller2D.Move(_velocity * Time.deltaTime, input);
		}

	}
}

