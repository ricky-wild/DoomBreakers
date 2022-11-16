using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace DoomBreakers
{

	public class BaseState //: MonoBehaviour
	{
		//<summary>
		//All these variables are required for the various Player Behaviours. 
		//So we embody them within a state and use as appropriate with each
		//dervived player state we create and set.
		//</summary>

		protected StateMachine _stateMachine;
		//protected Animator _animator;
		//protected Controller2D _controller2D;
		protected Vector3 _velocity;
		//protected Transform _transform;
		protected float _targetVelocityX, _maxJumpVelocity, _moveSpeed, _sprintSpeed, _gravity;
		protected int _quickAttackIncrement; //4+ variations of this animation.
		protected bool _dodgedLeftFlag;//, _jumpedFlag;

		protected float _quickAtkWaitTime, _gainedEquipWaitTime;
		//protected ITimer _behaviourTimer, _dodgedTimer, _spriteColourSwapTimer;

		public BaseState(Vector3 velocity)
		{

			_velocity = velocity;//new Vector3();
			_moveSpeed = 3.75f;//3.5f;
			_sprintSpeed = 1.0f;
			_targetVelocityX = 1.0f;
			_maxJumpVelocity = 64.0f;//14.0f;//13.25f;
			_gravity = wildlogicgames.DoomBreakers.GetGravity();// *2;
			_quickAttackIncrement = 0;
			_dodgedLeftFlag = false;
			_quickAtkWaitTime = 0.133f;
			_gainedEquipWaitTime = 1.5f;



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
		public virtual void IsMoving(ref Animator animator, ref Vector2 input) { }
		public virtual void IsJumping(ref Animator animator, ref Controller2D controller2D) { }
		public virtual void IsFalling(ref Animator animator, ref Controller2D controller2D) { }
	}
}

