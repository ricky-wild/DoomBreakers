using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;


namespace DoomBreakers
{

	public class BasicEnemyBaseState : MonoBehaviour
	{
		//<summary>
		//All these variables are required for the various Bandit AI Behaviours. 
		//So we embody them within a state and use as appropriate with each
		//dervived player state we create and set.
		//</summary>

		protected int _enemyID;
		protected BasicEnemyStateMachine _stateMachine;
		protected Transform _transform;
		protected Vector3 _velocity, _cachedVector3;
		protected const float _maxJumpVelocity = 12.0f;
		protected float _targetVelocityX, _moveSpeed, _sprintSpeed, _jumpSpeed, _gravity,
			_targetVelocityY, _maxPowerStruckVelocityY, _maxPowerStruckVelocityX, _attackDist, _randSpeedModifier;
		protected bool _dodgedLeftFlag;
		protected bool _detectPlatformEdge;
		protected int _quickAttackIncrement; //2+ variations of this animation.
		protected int _attackCooldownCounter;
		protected int _attackCooldownLimit;
		protected float _cooldownWaitTime, _idleWaitTime, _quickAtkWaitTime;
		protected ITimer _behaviourTimer, _cooldownTimer;

		public BasicEnemyBaseState(Vector3 velocity, int enemyId)
		{
			_enemyID = enemyId;
			_velocity = velocity;//new Vector3();
			_moveSpeed = wildlogicgames.Utilities.GetRandomNumberInt(3, 5);//4.0f;//3.75f;//3.5f;
			_moveSpeed = _moveSpeed - 0.5f;
			_sprintSpeed = 1.0f;
			_targetVelocityX = 1.0f;
			_jumpSpeed = 4.6f;// 4.0f;
			_randSpeedModifier = wildlogicgames.Utilities.GetRandomNumberInt(1, 7);
			_randSpeedModifier = (_randSpeedModifier / 4f); //1/4=0.25f    7/4=1.75f
			_gravity = wildlogicgames.DoomBreakers.GetGravity();
			_dodgedLeftFlag = false;
			_detectPlatformEdge = true;
			_quickAtkWaitTime = 0.133f;
			_attackDist = 1.5f;

			_cooldownWaitTime = 3.0f;
			_idleWaitTime = 0.5f;
			_quickAtkWaitTime = 0.133f;
		}

		public virtual void UpdateBehaviour(ref CharacterController2D controller2D, ref Animator animator, ref Transform transform, ref BanditStats banditStats)
		{
			//if (this.GetType() == typeof(BanditHitByKnockAttack)) //Want player to purposely knock down pit. (make a 60/40 success rate check maybe)
			//	_detectPlatformEdge = false;

			if (controller2D._collisionDetail._platformEdge)
			{
				if (!banditStats.IsBludgeoning()) _stateMachine.SetState(new BanditJump(_stateMachine, _velocity, transform, _enemyID));
			}
			
			UpdateGravity(ref controller2D, ref animator);
			UpdateTransform(ref controller2D);
		}

		private void UpdateTransform(ref CharacterController2D controller2D)
		{

			controller2D.UpdateMovement(_velocity * Time.deltaTime, Vector2.zero, _detectPlatformEdge);
		}
		private void UpdateGravity(ref CharacterController2D controller2D, ref Animator animator)
		{
			//if (_controller2D == null) return;
			bool collisionBelow = controller2D._collisionDetail._collidedDirection[0];

			if (!collisionBelow)
			{
				//animator.Play("Fall");
				_velocity.y += _gravity * Time.deltaTime;
			}

			if (collisionBelow)
			{
				_velocity.y = 0f; //if (Mathf.Abs(_velocity.y) != 0) _velocity.y = 0f;
			}
		}
		public virtual void IsIdle(ref Animator animator, ref IBanditCollision banditCollider) { }
		public virtual void IsIdle(ref Animator animator) { }
		public virtual void IsJumping(ref Animator animator, ref IBanditSprite banditSprite) { }
		public virtual void IsWaiting(ref Animator animator) { }
		public virtual void IsPersueTarget(ref Animator animator, ref IBanditSprite banditSprite, ref IBanditCollision banditCollider, ref BanditStats banditStats) { }
		public virtual void IsFalling(ref Animator animator, ref CharacterController2D controller2D, ref IBanditSprite banditSprite) { }
		public virtual void IsDefending(ref Animator animator, ref CharacterController2D controller2D, ref IBanditSprite banditSprite) { }
		public virtual void IsQuickAttack(ref Animator animator, ref IBanditCollision banditCollider, ref IBanditSprite banditSprite, ref int quickAttackIncrement) { }
		public virtual void IsHoldAttack(ref Animator animator, ref IBanditSprite banditSprite) { }
		public virtual void IsReleaseAttack(ref Animator animator, ref IBanditCollision banditCollider, ref IBanditSprite banditSprite) { }
		public virtual void IsHitByPowerAttack(ref Animator animator, ref IBanditSprite banditSprite, float playerAttackChargeTime) { }
		public virtual void IsHitByQuickAttack(ref Animator animator, ref IBanditSprite banditSprite) { }
		public virtual void IsHitWhileDefending(ref Animator animator, ref CharacterController2D controller2D, ref IBanditSprite banditSprite) { }
		public virtual void IsHitByUpwardAttack(ref Animator animator, ref IBanditSprite banditSprite) { }
		public virtual void IsHitByKnockAttack(ref Animator animator, ref IBanditSprite banditSprite) { }
		public virtual void IsIdleBowman(ref Animator animator, ref IBanditCollision banditCollider) { }
		public virtual void IsAiming(ref Animator animator, ref IBanditCollision banditCollider,ref IBanditSprite banditSprite) { }
		public virtual void IsShootTarget(ref Animator animator, ref IBanditSprite banditSprite, ref IBanditCollision banditCollider) { }
		public virtual void IsHit(ref Animator animator, ref IBanditSprite banditSprite) { }
		public virtual void IsDying(ref Animator animator, ref IBanditSprite banditSprite) { }
		public virtual void IsDead(ref Animator animator, ref IBanditSprite banditSprite) { }
	}
}

