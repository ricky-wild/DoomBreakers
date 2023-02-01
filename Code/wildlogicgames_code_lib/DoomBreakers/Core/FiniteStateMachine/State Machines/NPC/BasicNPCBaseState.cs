using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
    public class BasicNPCBaseState : MonoBehaviour
	{
		protected int _npcID;
		protected NPCStateMachine _stateMachine;
		protected Transform _transform;
		protected Vector3 _velocity, _cachedVector3;
		protected const float _maxJumpVelocity = 12.0f;
		protected float _targetVelocityX, _moveSpeed, _sprintSpeed, _jumpSpeed, _gravity,
			_targetVelocityY, _maxPowerStruckVelocityY, _maxPowerStruckVelocityX, _attackDist, _randSpeedModifier;

		protected bool _detectPlatformEdge;
		protected float _idleWaitTime;
		protected ITimer _behaviourTimer;

		public BasicNPCBaseState(Vector3 velocity, int npcId)
		{
			_npcID = npcId;
			_velocity = velocity;//new Vector3();
			_moveSpeed = wildlogicgames.Utilities.GetRandomNumberInt(3, 5);//4.0f;//3.75f;//3.5f;
			_moveSpeed = _moveSpeed - 0.5f;
			_sprintSpeed = 1.2f;
			_targetVelocityX = 1.0f;
			_jumpSpeed = 4.6f;// 4.0f;
			_randSpeedModifier = wildlogicgames.Utilities.GetRandomNumberInt(1, 7);
			_randSpeedModifier = (_randSpeedModifier / 4f); //1/4=0.25f    7/4=1.75f
			_gravity = wildlogicgames.DoomBreakers.GetGravity();

			_detectPlatformEdge = true;
			_attackDist = 1.5f;
			_idleWaitTime = 0.5f;

		}

		public virtual void UpdateBehaviour(ref CharacterController2D controller2D, ref Animator animator, ref Transform transform, ref NPCStats npcStats)
		{

			if (controller2D._collisionDetail._platformEdge) _stateMachine.SetState(new NPCJump(_stateMachine, _velocity, transform, _npcID));
			if (controller2D._collisionDetail._collidedDirection[2]) _stateMachine.SetState(new NPCJump(_stateMachine, _velocity, transform, _npcID));
			if (controller2D._collisionDetail._collidedDirection[3]) _stateMachine.SetState(new NPCJump(_stateMachine, _velocity, transform, _npcID));

			UpdateGravity(ref controller2D, ref animator);
			UpdateTransform(ref controller2D);
		}

		private void UpdateTransform(ref CharacterController2D controller2D) => controller2D.UpdateMovement(_velocity * Time.deltaTime, Vector2.zero, _detectPlatformEdge);
		private void UpdateGravity(ref CharacterController2D controller2D, ref Animator animator)
		{
			//if (_controller2D == null) return;
			bool collisionBelow = controller2D._collisionDetail._collidedDirection[0];

			if (!collisionBelow) _velocity.y += _gravity * Time.deltaTime;

			if (collisionBelow) _velocity.y = 0f; //if (Mathf.Abs(_velocity.y) != 0) _velocity.y = 0f;
		}

		public virtual void IsIdle(ref NPCAnimator animator) { }
		public virtual void IsTravelling(ref NPCAnimator animator, ref NPCSprite npcSprite) { }
		public virtual void IsFleeing(ref NPCAnimator animator, ref NPCSprite npcSprite) { }
		public virtual void IsJumping(ref NPCAnimator animator, ref NPCSprite npcSprite) { }
		public virtual void IsWaiting(ref NPCAnimator animator, ref NPCSprite npcSprite) { }
		public virtual void IsHit(ref Animator animator, ref IBanditSprite banditSprite) { }
		public virtual void IsDying(ref Animator animator, ref IBanditSprite banditSprite) { }
		public virtual void IsDead(ref Animator animator, ref IBanditSprite banditSprite) { }
		public virtual void IsFalling(ref NPCAnimator animator, ref CharacterController2D controller2D, ref NPCSprite npcSprite) { }
		public virtual void IsDefending(ref Animator animator, ref CharacterController2D controller2D, ref IBanditSprite banditSprite) { }
	}
}
