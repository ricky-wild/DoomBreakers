﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;


namespace DoomBreakers
{

	public class BaseState : MonoBehaviour
	{
		//<summary>
		//All these variables are required for the various Player Behaviours. 
		//So we embody them within a state and use as appropriate with each
		//dervived player state we create and set.
		//</summary>

		protected StateMachine _stateMachine;
		protected Vector3 _velocity;
		protected const float _maxJumpVelocity = 12.0f;
		protected float _targetVelocityX, _moveSpeed, _sprintSpeed, _jumpSpeed, _gravity, _maxPowerStruckVelocityY, _maxPowerStruckVelocityX;
		protected int _quickAttackIncrement; //4+ variations of this animation.
		protected bool _dodgedLeftFlag;//, _jumpedFlag;

		protected float _quickAtkWaitTime, _gainedEquipWaitTime;
		protected ITimer _behaviourTimer;// _dodgedTimer, _spriteColourSwapTimer;

		public BaseState(Vector3 velocity)
		{

			_velocity = velocity;//new Vector3();
			_moveSpeed = 4.0f;//3.75f;//3.5f;
			_sprintSpeed = 1.0f;
			_targetVelocityX = 1.0f;
			_jumpSpeed = 4.6f;// 4.0f;
			_gravity = wildlogicgames.DoomBreakers.GetGravity();
			_quickAttackIncrement = 0;
			_dodgedLeftFlag = false;
			_quickAtkWaitTime = 0.133f;
			_gainedEquipWaitTime = 1.5f;



		}
		public virtual void UpdateBehaviour(ref CharacterController2D controller2D, ref Animator animator, ref BaseState state)
		{
			UpdateGravity(ref controller2D, ref animator, ref state);
			UpdateTransform(ref controller2D);		
		}
		private void UpdateTransform(ref CharacterController2D controller2D)
		{
			//if(_controller2D == null) return;
			controller2D.UpdateMovement(_velocity * Time.deltaTime, _velocity, false);
		}
		private void UpdateGravity(ref CharacterController2D controller2D, ref Animator animator, ref BaseState state)
		{
			//if (_controller2D == null) return;

			bool collisionBelow = controller2D._collisionDetail._collidedDirection[0];

			if (!collisionBelow)
			{
				//animator.Play("Fall");
				if (state.GetType() != typeof(PlayerDying))
					_velocity.y += _gravity * Time.deltaTime;
				else
					_velocity.y += (_gravity * Time.deltaTime)/4;
			}

			if (collisionBelow)
			{
				if(Mathf.Abs(_velocity.y) != 0)
				{
					_velocity.y = 0f;
					
				}

			}

			if (state.GetType() == typeof(PlayerDead)) _velocity.y = 0f;
		}
		public virtual void IsIdle(ref Animator animator, ref IPlayerSprite playerSprite) { }

		public virtual void IsExhausted(ref Animator animator, ref IPlayerSprite playerSprite) { }
		public virtual void IsGainedEquipment(ref Animator animator, ref IPlayerSprite playerSprite, ref IPlayerEquipment playerEquipment) { }
		public virtual void IsBrokenEquipment(ref Animator animator, ref IPlayerSprite playerSprite, ref IPlayerEquipment playerEquipment) { }
		public virtual void IsMoving(ref Animator animator, ref Vector2 input, ref IPlayerSprite playerSprite, ref PlayerCollision playerCollider) { }
		public virtual void IsSprinting(ref Animator animator, ref Vector2 input, ref IPlayerSprite playerSprite, ref PlayerCollision playerCollider) { }
		public virtual void IsJumping(ref Animator animator, ref CharacterController2D controller2D, ref Vector2 input, ref IPlayerSprite playerSprite) { }
		public virtual void IsFalling(ref Animator animator, ref CharacterController2D controller2D, ref Vector2 input) { }
		public virtual void IsDodging(ref Animator animator, ref CharacterController2D controller2D, ref Vector2 input, 
										bool dodgeLeft, ref IPlayerSprite playerSprite, ref PlayerCollision playerCollider) { }
		public virtual void IsDodged(ref Animator animator, ref CharacterController2D controller2D, ref Vector2 input) { }
		public virtual void IsQuickAttack(ref Animator animator, ref IPlayerSprite playerSprite, ref Vector2 input, ref int quickAttackIncrement) { }
		public virtual void IsUpwardAttack(ref Animator animator, ref IPlayerSprite playerSprite, ref Vector2 input) { }
		public virtual void IsKnockAttack(ref Animator animator, ref IPlayerSprite playerSprite, ref Vector2 input) { }
		public virtual void IsHoldAttack(ref Animator animator, ref IPlayerSprite playerSprite, ref Vector2 input) { }
		public virtual void IsReleaseAttack(ref Animator animator, ref IPlayerSprite playerSprite, ref Vector2 input) { }
		public virtual void IsDefending(ref Animator animator, ref Vector2 input) { }
		public virtual void IsHitByQuickAttack(ref Animator animator, ref IPlayerSprite playerSprite, ref Vector2 input) { }
		public virtual void IsHitByReleaseAttack(ref Animator animator, ref IPlayerSprite playerSprite, ref Vector2 input) { }
		public virtual void IsHitWhileDefending(ref Animator animator, ref Vector2 input) { }
		public virtual void IsDying(ref Animator animator, ref IPlayerSprite playerSprite) { }
		public virtual void IsDead(ref Animator animator, ref IPlayerSprite playerSprite) { }
	}
}

