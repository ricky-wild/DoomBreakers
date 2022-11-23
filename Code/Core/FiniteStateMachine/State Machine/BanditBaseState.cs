using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace DoomBreakers
{

	public class BanditBaseState : MonoBehaviour
	{
		//<summary>
		//All these variables are required for the various Bandit AI Behaviours. 
		//So we embody them within a state and use as appropriate with each
		//dervived player state we create and set.
		//</summary>

		protected int _banditID;
		protected EnemyStateMachine _stateMachine;
		protected Transform _transform;
		protected Vector3 _velocity;
		protected const float _maxJumpVelocity = 12.0f;
		protected float _targetVelocityX, _moveSpeed, _sprintSpeed, _jumpSpeed, _gravity,
			_targetVelocityY, _maxPowerStruckVelocityY, _maxPowerStruckVelocityX;
		protected bool _dodgedLeftFlag;
		protected int _quickAttackIncrement; //2+ variations of this animation.
		protected int _attackCooldownCounter;
		protected int _attackCooldownLimit;
		protected float _cooldownWaitTime, _idleWaitTime, _quickAtkWaitTime;
		protected ITimer _behaviourTimer, _cooldownTimer;

		public BanditBaseState(Vector3 velocity, int banditId)
		{
			_banditID = banditId;
			_velocity = velocity;//new Vector3();
			_moveSpeed = 4.0f;//3.75f;//3.5f;
			_sprintSpeed = 1.0f;
			_targetVelocityX = 1.0f;
			_jumpSpeed = 4.6f;// 4.0f;
			_gravity = wildlogicgames.DoomBreakers.GetGravity();
			_dodgedLeftFlag = false;
			_quickAtkWaitTime = 0.133f;

		}

		public virtual void UpdateBehaviour(ref Controller2D controller2D, ref Animator animator)
		{
			UpdateGravity(ref controller2D, ref animator);
			UpdateTransform(ref controller2D);
		}

		private void UpdateTransform(ref Controller2D controller2D)
		{
			//if(_controller2D == null) return;
			controller2D.Move(_velocity * Time.deltaTime, _velocity);
		}
		private void UpdateGravity(ref Controller2D controller2D, ref Animator animator)
		{
			//if (_controller2D == null) return;
			if (!controller2D.collisions.below)
			{
				//animator.Play("Fall");
				_velocity.y += _gravity * Time.deltaTime;
			}

			if (controller2D.collisions.below)
				_velocity.y = 0f;
		}
		public virtual void IsIdle(ref Animator animator) { }
		public virtual void IsWaiting(ref Animator animator) { }
		public virtual void IsPersueTarget(ref Animator animator) { }
	}
}

