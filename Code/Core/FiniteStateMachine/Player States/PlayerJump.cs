using UnityEngine;

namespace DoomBreakers
{
	public class PlayerJump : BaseState
	{
		public PlayerJump(StateMachine s, Vector3 v) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_behaviourTimer = new Timer();
			print("\nJump State.");
		}


		public override void IsJumping(ref Animator animator, ref Controller2D controller2D)
		{
			animator.Play("Jump");//, -1, 0.0f);//, -1, 0.0f);
								  //_behaviourTimer.StartTimer(0.001f);

			//if (_behaviourTimer.HasTimerFinished())
			//	_velocity.y += _jumpSpeed;
			_velocity.y += _jumpSpeed;
			if (_velocity.y >= _maxJumpVelocity)//(_maxJumpVelocity / 1.15f)) //Near peak of jump velocity, set falling state.
					_stateMachine.SetState(new PlayerFall(_stateMachine, _velocity));
			
			//base.UpdateBehaviour();
		}
	}
}