using UnityEngine;

namespace DoomBreakers
{
	public class PlayerJump : BaseState
	{
		public PlayerJump(StateMachine s, Vector3 v) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
		}


		public override void IsJumping(ref Animator animator, ref Controller2D controller2D)
		{
			//if (!controller2D.collisions.below) return;
			animator.Play("Jump");//, -1, 0.0f);//, -1, 0.0f);
			_velocity.y += _maxJumpVelocity;
			if (_velocity.y >= (_maxJumpVelocity / 1.25f)) //Near peak of jump velocity, set falling state.
				_stateMachine.SetState(new PlayerFall(_stateMachine, _velocity));
			//base.UpdateBehaviour();
		}
	}
}