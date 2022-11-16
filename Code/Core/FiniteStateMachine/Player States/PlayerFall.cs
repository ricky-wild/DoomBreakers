using UnityEngine;

namespace DoomBreakers
{
	public class PlayerFall : BaseState
	{
		public PlayerFall(StateMachine s, Vector3 v) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
		}
		public override void IsFalling(ref Animator animator, ref Controller2D controller2D)
		{
			animator.Play("Fall");
			if (controller2D.collisions.below) //Means we're finished jumping/falling.
			{
				_velocity.x = 0f;
				_velocity.y = 0f;
				_stateMachine.SetState(new PlayerIdle(_stateMachine, _velocity));
			}
			//base.UpdateBehaviour();
		}
	}
}

